using System;
using System.Threading;
using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Text;
using Grpc.Core;
using Org.BouncyCastle.Utilities.Encoders;

// ReSharper disable InconsistentNaming

namespace GrpcClient
{
    /// <summary>
    /// Helper class that creates and caches Basic-Auth credentials and gRPC Metadata used when invoking gRPC methods. The MakeOptions() method merges the method parameters with the cached values to create the CallOptions structure used when invoking
    /// the gRPC method.
    /// </summary>
    public class ClientCredentialHelper
    {
        /// <summary>
        /// The prefix for basic auth as used in the authorization header. This library
        /// assumes that the both the username and password are UTF-8 encoded before
        /// being turned into a base64 string.
        /// </summary>
        public const string BASIC_AUTH = "Basic";

        public const string HTTP_AUTHORIZATION = "Authorization";

        private readonly string _cnxId;
        private readonly string _username;
        private readonly string _password;
        private readonly Metadata _metadata;
        private readonly CallCredentials _callCredentials;

        public ClientCredentialHelper(string cnxId, string username, string password)
        {

            if (!string.IsNullOrWhiteSpace(cnxId) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                _cnxId = cnxId;
                _username = username;
                _password = password;

                // Only populate the credentials if the username and password were provided. May not be provided if call does not require a user (system events). 
                var authorization = EncodeBasicAuth(_cnxId, _username, _password).ToString();
                _metadata = new Metadata { { HTTP_AUTHORIZATION, authorization } };
                _callCredentials = new BasicAuthCredentials(_metadata);
            }
            else
            {
                _metadata = new Metadata();
            }
        }

        public string CnxId { get { return _cnxId; } }
        public string Username { get { return _username; } }

        /// <summary>
        /// Each gRPC call needs CallOptions. Given a cancellationToken, this method constructs the CallOptions from
        /// any provided BasicAuth credentials.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token associated with the request (either method or registration for an event).</param>
        /// <returns></returns>
        public CallOptions CreateCallOptions(CancellationToken cancellationToken = default)
        {
            return new CallOptions(_metadata, null, cancellationToken, WriteOptions.Default, null, _callCredentials);
        }


        private AuthenticationHeaderValue EncodeBasicAuth(string cnxid, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(cnxid) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string authstr = cnxid + (Char)(253) + username + (Char)(253) + password;
            var byteArray = Encoding.Unicode.GetBytes(authstr);
            var authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return authorization;
        }
    }
}
