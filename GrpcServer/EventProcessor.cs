using System;
using System.Collections.Concurrent;
using System.Threading;
using AutoMapper;
using Grpc.Core;
using Ninject.Extensions.Logging;

namespace GrpcServer
{
    /// <summary>
    /// Base class for all event processors such as LogResultProcessor.
    /// See <a href="@opcua_events">Implementing OPC/UA Events</a>
    /// ToDo: Add a non-generic base interface that the GrpcClient can use to tag TopicType. Then have Ninject create instances of all the IEventProcessors in a dictionary indexed by topic type, with transient scope to be injected into the GrpcClient.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventProcessor<T> : IDisposable
    {
        protected ILogger _logger;
        protected IMapper _mapper;
        protected IServerStreamWriter<T> _responseStream;
        protected IDisposable _subscription;
        protected ConcurrentQueue<T> _messageQueue = new ConcurrentQueue<T>();

        // Amount of time to wait before checking cancellation token or dispose occurred.
        private const int TIME_OUT = 5000;

        private bool _disposeCalled = false;
        private ServerCallContext _serverCallContext;

        /// <summary>
        /// Flag for exiting thread processing messages.
        /// </summary>
        protected bool Done => _serverCallContext.CancellationToken.IsCancellationRequested && ! _disposeCalled;

        public EventProcessor(ILogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public virtual void Subscribe(ServerCallContext context, IServerStreamWriter<T> responseStream)
        {
            _serverCallContext = context;
            // Loop until cancellation or stream closed by client.
            ProcessMessages();
            _subscription?.Dispose();
        }

        protected void QueueMessage(T message)
        {
            _messageQueue.Enqueue(message);
            lock (_responseStream)
            {
                Monitor.Pulse(_responseStream);
            }
        }

        /// <summary>
        /// Using gRPC streams, you cannot process more than one asynchrous write at one time. The message queue holds
        /// the additional messages and this message processors ensure that only one message is sent at any given time.
        /// </summary>
        /// <param name="eventProcessor">This is essentially the "this" pointer back to a class instance for this static method.</param>
        protected void ProcessMessages()
        {
            try
            {
                while (!Done)
                {
                    while (!_messageQueue.IsEmpty)
                    {
                        if (_messageQueue.TryDequeue(out var message))
                        {
                            _responseStream.WriteAsync(message).Wait(TIME_OUT);
                        }
                    }

                    lock (_responseStream)
                    {
                        Monitor.Wait(_responseStream);
                    }
                }

            }
            catch (Exception)
            {
                _logger.Debug("ProcessMessages for event stream terminated.");
            }
        }

        public void Dispose()
        {
            // Exit ProcessMessages()
            _disposeCalled = true;
        }
    }
}