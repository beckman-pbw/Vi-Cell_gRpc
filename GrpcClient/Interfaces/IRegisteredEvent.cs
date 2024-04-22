using System;

namespace GrpcClient.Interfaces
{
    /// <summary>
    /// The Register method opens a stream to receive event messages from the gRPC server using a defined gRPC call.
    ///  The Dispose method closes any open stream created by an earlier Register() call. Both are implemented in derived classes.
    /// </summary>
    public interface IRegisteredEvent : IDisposable
    {
        /// <summary>
        /// Call the Grpc call to request an event stream, which will return an AsyncServerStreamingCall for that event type.
        /// </summary>
        void Register();
    }
}