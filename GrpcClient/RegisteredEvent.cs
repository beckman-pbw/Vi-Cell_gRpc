using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcClient.Interfaces;
using GrpcService;
using Ninject.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming

namespace GrpcClient
{
    /// <summary>
    /// Represents a single registration for an event to the ScoutX gRPC server. Contains a reference
    /// to the gRPC stream, the OPC NodeState/NodeId, and the OPC SubscriptionId. When a gRPC message
    /// is received, the OPC node is updated. The OpcUser contains a collection of these Registered
    /// events. When the OpcUser disconnects, this method's Disposable cleans up the connection.
    /// </summary>
    public abstract class RegisteredEvent<T> : IRegisteredEvent
    {
        private AsyncServerStreamingCall<T> _streamingCall;
        private readonly ILogger _logger;
        protected IGrpcClient Client;
        protected CancellationTokenSource CancellationTokenSource { get; }
        protected bool _streamActive = true;

        protected RegisteredEvent(ILogger logger, IGrpcClient client)
        {
            _logger = logger;
            Client = client;
            CancellationTokenSource = new CancellationTokenSource();
        }

        protected bool OpenStreamFlag => _streamActive && CancellationTokenSource.IsCancellationRequested;

        protected AsyncServerStreamingCall<T> StreamingCall
        {
            get => _streamingCall;
            set
            {
                _streamingCall = value;
                Task.Run(ProcessEvents);
            }
        }

        protected virtual CallOptions MakeCallOptions()
        {
            return Client.ClientCredentialHelper.CreateCallOptions(CancellationTokenSource.Token);
        }

        protected RegistrationRequest MakeRequest(TopicTypeEnum topicType)
        {
            var request = new global::GrpcService.RegistrationRequest
            {
                ClientId = Client.Id,
            };
            request.TopicTypes.Add(topicType);
            return request;
        }

        /// <summary>
        /// Process all the event messages coming in on the stream
        /// </summary>
        /// <returns></returns>
        private async Task ProcessEvents()
        {
            try
            {
                while (await StreamingCall.ResponseStream.MoveNext(CancellationTokenSource.Token))
                {
                    var msg = StreamingCall.ResponseStream.Current;
                    OnMessage(msg);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.Cancelled)
            {
                CancellationTokenSource.Cancel(true);
            }
            catch (RpcException ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Derived classes override this method to update values, or generate events in the OPC/UA server.
        /// </summary>
        /// <param name="msg"></param>
        protected abstract void OnMessage(T msg);

        protected virtual void OnError(RpcException e) {}

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// Closes any open streams created in derived classes.
        /// </summary>
        public void Dispose()
        {
            try
            {
                _streamActive = false;
                CancellationTokenSource.Cancel();
                StreamingCall?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Warn($"Exception occurred during RegisteredEvent {this.GetType().FullName} cleanup.", ex);
            }
        }
    }
}