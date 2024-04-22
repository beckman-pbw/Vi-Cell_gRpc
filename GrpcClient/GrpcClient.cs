using Grpc.Core;
using GrpcClient.Interfaces;
using GrpcService;
using System;
using System.Collections.Concurrent;
using System.IO;
using Grpc.Core.Interceptors;
using Grpc.Core.Logging;

namespace GrpcClient
{
    public class OpcUaGrpcClient : IGrpcClient
    {
        private readonly ILogger _logger;
        private readonly Interceptor _clientInterceptor;
        private GrpcServices.GrpcServicesClient _client;
        private static readonly ConcurrentBag<IRegisteredEvent> _registeredEvents = new ConcurrentBag<IRegisteredEvent>();

        public ClientCredentialHelper ClientCredentialHelper { get; set; }

        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Added this to support creating events in the OpcUa server that mapped the Grpc calls to the OpcUa RegisteredEvent(s).
        /// ToDo: Is it helpful to have an IGrpcClient interface, or let the interface for testing be defined in the OPC/UA solution?
        /// </summary>
        public GrpcServices.GrpcServicesClient ServicesClient => _client;

        public OpcUaGrpcClient(ILogger logger)
        {
            _logger = logger;
        }

        public OpcUaGrpcClient(ILogger logger, Interceptor clientInterceptor)
        {
            _logger = logger;
            _clientInterceptor = clientInterceptor;
        }

        public void Init(string username, string password)
        {
            try
            {
                _logger?.Debug("GrpcClient: Init, clientId:" + Id + ", username: " + username);
                var channel = new Channel("127.0.0.1", 22222, ChannelCredentials.Insecure);
                _client = (null != _clientInterceptor) ?
                    new GrpcServices.GrpcServicesClient(channel.Intercept(_clientInterceptor))
                    : new GrpcServices.GrpcServicesClient(channel);

                ClientCredentialHelper = new ClientCredentialHelper(Id, username, password);
            }
            catch (Exception e)
            {
                _logger?.Error(e, "GrpcClient failed to initialize.");
            }
        }

        public VcbResultRequestLock SendRequestRequestLock(RequestRequestLock request)
        {
            _logger?.Debug("GrpcClient: RequestRequestLock");
            var response = _client.RequestLock(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultReleaseLock SendRequestReleaseLock(RequestReleaseLock request)
        {
            _logger?.Debug("GrpcClient: RequestReleaseLock");
            var response = _client.ReleaseLock(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultCreateCellType SendRequestCreateCellType(RequestCreateCellType request)
        {
            _logger?.Debug("GrpcClient: RequestCreateCellType");
            var response = _client.CreateCellType(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultDeleteCellType SendRequestDeleteCellType(RequestDeleteCellType request)
        {
            _logger?.Debug("GrpcClient: RequestDeleteCellType");
            var response = _client.DeleteCellType(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestCreateQualityControl(RequestCreateQualityControl request)
        {
            _logger?.Debug("GrpcClient: RequestCreateQualityControl");
            var response = _client.CreateQualityControl(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultGetSampleResults SendRequestGetSampleResults(RequestGetSampleResults request)
        {
            _logger?.Debug("GrpcClient: RequestGetSampleResults");
            var response = _client.GetSampleResults(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestDeleteSampleResults(RequestDeleteSampleResults request)
        {
            _logger?.Debug("GrpcClient: RequestDeleteSampleResults");
            var response = _client.DeleteSampleResults(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestStartSample(RequestStartSample request)
        {
            _logger?.Debug("GrpcClient: RequestStartSample");
            var response = _client.StartSample(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultStartExport SendStartExport(RequestStartExport request)
        {
            _logger?.Debug("GrpcClient: StartExport");
            var response = _client.StartExport(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultRetrieveBulkDataBlock SendRetrieveBulkDataBlock(RequestRetrieveBulkDataBlock request)
        {
            _logger?.Debug("GrpcClient: RetrieveBulkDataBlock");
            var response = _client.RetrieveBulkDataBlock(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestStartSampleSet(RequestStartSampleSet request)
        {
            _logger?.Debug("GrpcClient: RequestStartSampleSet");
            var response = _client.StartSampleSet(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultEjectStage SendRequestEjectStage(RequestEjectStage request)
        {
            _logger?.Debug("GrpcClient: RequestEjectStage");
            var response = _client.EjectStage(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestStop(RequestStop request)
        {
            _logger?.Debug("GrpcClient: RequestStop");
            var response = _client.Stop(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestPause(RequestPause request)
        {
            _logger?.Debug("GrpcClient: RequestPause");
            var response = _client.Pause(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestResume(RequestResume request)
        {
            _logger?.Debug("GrpcClient: RequestResume");
            var response = _client.Resume(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultExportConfig SendRequestExportConfig(RequestExportConfig request)
        {
            _logger?.Debug("GrpcClient: RequestExportConfig");
            var response = _client.ExportConfig(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult LoginRemoteUser(RequestLoginUser request)
        {
            _logger?.Debug("GrpcClient: LoginRemoteUser");
            var response = _client.LoginRemoteUser(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult LogoutRemoteUser(RequestLogoutUser request)
        {
            _logger?.Debug("GrpcClient: LogoutRemoteUser");
            var response = _client.LogoutRemoteUser(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestImportConfig(RequestImportConfig request)
        {
            _logger?.Debug("GrpcClient: RequestImportConfig");
            var response = _client.ImportConfig(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultGetCellTypes SendRequestGetCellTypes(RequestGetCellTypes request)
        {
            _logger?.Debug("GrpcClient: RequestGetCellTypes");
            var response = _client.GetCellTypes(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultGetQualityControls SendRequestGetQualityControls(RequestGetQualityControls request)
        {
            _logger?.Debug("GrpcClient: RequestGetQualityControls");
            var response = _client.GetQualityControls(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResultGetDiskSpace SendRequestGetAvailableDiskSpace(RequestGetAvailableDiskSpace request)
        {
            _logger?.Debug("GrpcClient: RequestGetAvailableDiskSpace");
            var response = _client.GetAvailableDiskSpace(request, ClientCredentialHelper.CreateCallOptions());
            return response;
        }

        public VcbResult SendRequestCleanFluidics(RequestCleanFluidics request)
        {
	        _logger?.Debug("GrpcClient: RequestCleanFluidics");
			var response = _client.CleanFluidics(request, ClientCredentialHelper.CreateCallOptions());
			return response;
        }
		
        public void AddRegisteredEvent(IRegisteredEvent registeredEvent)
        {
            _registeredEvents.Add(registeredEvent);
        }

        public VcbResultReagentVolume SendRequestGetReagentVolume(RequestGetReagentVolume request)
        {
	        _logger?.Debug("GrpcClient: RequestGetReagentVolume");
	        var response = _client.GetReagentVolume (request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

        public VcbResult SendRequestSetReagentVolume(RequestSetReagentVolume request)
        {
	        _logger?.Debug("GrpcClient: RequestSetReagentVolume");
	        var response = _client.SetReagentVolume(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

		public VcbResult SendRequestAddReagentVolume(RequestAddReagentVolume request)
		{
			_logger?.Debug("GrpcClient: RequestAddReagentVolume");
			var response = _client.AddReagentVolume(request, ClientCredentialHelper.CreateCallOptions());
			return response;
		}
        
		public VcbResult SendRequestShutdownOrReboot(RequestShutdownOrReboot request)
        {
	        _logger?.Debug("GrpcClient: RequestShutdownOrReboot");
	        _logger?.Debug($"GrpcClient: {request.Operation}");
	        var response = _client.ShutdownOrReboot(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

		public VcbResult SendRequestDeleteCampaignData(RequestDeleteCampaignData request)
		{
			_logger?.Debug("GrpcClient: RequestDeleteCampaignData");
			var response = _client.DeleteCampaignData(request, ClientCredentialHelper.CreateCallOptions());
			return response;
		}

		public VcbResultStartExport SendRequestStartLogDataExport(RequestStartLogDataExport request)
		{
			_logger?.Debug("GrpcClient: RequestStartLogDatatExport");
			var response = _client.StartLogDataExport(request, ClientCredentialHelper.CreateCallOptions());
			return response;
		}

        public VcbResult SendRequestPrimeReagents(RequestPrimeReagents request)
        {
	        _logger?.Debug("GrpcClient: RequestPrimeReagents");
			var response = _client.PrimeReagents(request, ClientCredentialHelper.CreateCallOptions());
			return response;
        }

        public VcbResult SendRequestCancelPrimeReagents(RequestCancelPrimeReagents request)
        {
	        _logger?.Debug("GrpcClient: RequestCancelPrimeReagents");
	        var response = _client.CancelPrimeReagents(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

        public VcbResult SendRequestPurgeReagents(RequestPurgeReagents request)
        {
	        _logger?.Debug("GrpcClient: RequestPurgeReagents");
			var response = _client.PurgeReagents(request, ClientCredentialHelper.CreateCallOptions());
			return response;
        }

        public VcbResult SendRequestCancelPurgeReagents(RequestCancelPurgeReagents request)
        {
	        _logger?.Debug("GrpcClient: RequestCancelPurgeReagents");
	        var response = _client.CancelPurgeReagents(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

        public VcbResult SendRequestDecontaminate(RequestDecontaminate request)
        {
	        _logger?.Debug("GrpcClient: RequestDecontaminate");
	        var response = _client.Decontaminate(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

        public VcbResult SendRequestCancelDecontaminate(RequestCancelDecontaminate request)
        {
	        _logger?.Debug("GrpcClient: RequestCancelDecontaminate");
	        var response = _client.CancelDecontaminate(request, ClientCredentialHelper.CreateCallOptions());
	        return response;
        }

		public void Dispose()
        {
            foreach (var registeredEvent in _registeredEvents)
            {
                registeredEvent.Dispose();
            }
        }
    }
}