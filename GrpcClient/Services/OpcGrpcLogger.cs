using System;
using Grpc.Core.Logging;

namespace GrpcClient.Services
{
    public class OpcGrpcLogger : ILogger
    {
        private readonly Ninject.Extensions.Logging.ILoggerFactory _loggerFactory;
        private readonly Ninject.Extensions.Logging.ILogger _logger;

        /// <summary>
        /// Default constructor usually used by Ninject to provide an instance with a gRPC logger.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="logger"></param>
        public OpcGrpcLogger(Ninject.Extensions.Logging.ILoggerFactory loggerFactory, Ninject.Extensions.Logging.ILogger logger)
        {
            _loggerFactory = loggerFactory;
            _logger = logger;
        }

        /// <summary>
        ///  Called from ForType&lt;T&gt;() to create a logger for a specific type.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="classType"></param>
        public OpcGrpcLogger(Ninject.Extensions.Logging.ILoggerFactory loggerFactory, Type classType)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.GetLogger(classType);
        }

        public ILogger ForType<T>()
        {
            var classType = typeof(T);
            return new OpcGrpcLogger(_loggerFactory, classType);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            _logger.DebugException(message, ex);
        }

        public void Debug(string format, params object[] formatArgs)
        {
            _logger.Debug(string.Format(format, formatArgs));
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            _logger.InfoException(message, ex);
        }

        public void Info(string format, params object[] formatArgs)
        {
            _logger.Info(string.Format(format, formatArgs));
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Warning(string format, params object[] formatArgs)
        {
            _logger.Warn(string.Format(format, formatArgs));
        }

        public void Warning(Exception exception, string message)
        {
            _logger.WarnException(message, exception);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string format, params object[] formatArgs)
        {
            _logger.Error(string.Format(format, formatArgs));
        }

        public void Error(Exception exception, string message)
        {
            _logger.ErrorException(message, exception);
        }

        #region Additional Logging options

        public void Info(Exception exception, string message)
        {
            _logger.InfoException(message, exception);
        }

        public void Debug(Exception exception, string message)
        {
            _logger.DebugException(message, exception);
        }

        #endregion

    }
}