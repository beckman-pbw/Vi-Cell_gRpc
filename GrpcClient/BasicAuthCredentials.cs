using System;
using Grpc.Core;

namespace GrpcClient
{
    /// <summary>
    /// The basic auth security header (CallCredentials) simply add a set of predefined
    /// headers to the call. This does not do much, except act as a placeholder in the
    /// CallOptions. The metadata in the CallOptions are used to populate the HTTP header.
    /// </summary>
    public class BasicAuthCredentials : CallCredentials
    {
        private Metadata _extraHeaders;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="extraHeaders">Should not be null.</param>
        public BasicAuthCredentials(Metadata extraHeaders)
        {
            _extraHeaders = extraHeaders;
        }

        /// <summary>
        /// Do nothing. Required by the defined interface
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="state"></param>
        public override void InternalPopulateConfiguration(CallCredentialsConfiguratorBase configurator, object state)
        {
        }
    }
}