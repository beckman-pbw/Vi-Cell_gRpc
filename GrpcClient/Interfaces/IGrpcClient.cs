using System;
using GrpcService;

namespace GrpcClient.Interfaces
{
    public interface IGrpcClient : IDisposable
    {
        string Id { get; }
        GrpcServices.GrpcServicesClient ServicesClient { get; }
        ClientCredentialHelper ClientCredentialHelper { get; set; }

        /// <summary>
        /// Initializes the gRPC client connection to the ScoutX gRPC server. If no
        /// credentials are passed, then the HTTP header will not be populated with
        /// an Authorization property. Invocation of methods requiring credentials
        /// will fail.
        /// </summary>
        /// <param name="username">Username provided by the OPC client. Maybe null or empty.</param>
        /// <param name="password">Password provided by the OPC client. Maybe null or empty.</param>
        void Init(string username, string password);

        VcbResultRequestLock SendRequestRequestLock(RequestRequestLock request);
        VcbResultReleaseLock SendRequestReleaseLock(RequestReleaseLock request);
        VcbResultDeleteCellType SendRequestDeleteCellType(RequestDeleteCellType request);
        VcbResultCreateCellType SendRequestCreateCellType(RequestCreateCellType request);
        VcbResultGetSampleResults SendRequestGetSampleResults(RequestGetSampleResults request);
        VcbResult SendRequestCreateQualityControl(RequestCreateQualityControl request);
        VcbResult SendRequestDeleteSampleResults(RequestDeleteSampleResults request);
        VcbResult SendRequestStartSample(RequestStartSample request);

        VcbResultStartExport SendStartExport(RequestStartExport request);
        VcbResultRetrieveBulkDataBlock SendRetrieveBulkDataBlock(RequestRetrieveBulkDataBlock request);

        VcbResult SendRequestStartSampleSet(RequestStartSampleSet request);
        VcbResultEjectStage SendRequestEjectStage(RequestEjectStage request);
        VcbResult SendRequestStop(RequestStop request);
        VcbResult SendRequestPause(RequestPause request);
        VcbResult SendRequestResume(RequestResume request);
        VcbResultExportConfig SendRequestExportConfig(RequestExportConfig request);
        VcbResult SendRequestImportConfig(RequestImportConfig request);
        VcbResultGetCellTypes SendRequestGetCellTypes(RequestGetCellTypes request);
        VcbResultGetQualityControls SendRequestGetQualityControls(RequestGetQualityControls request);
        VcbResultGetDiskSpace SendRequestGetAvailableDiskSpace(RequestGetAvailableDiskSpace request);
        VcbResult SendRequestCleanFluidics(RequestCleanFluidics request);
        VcbResultReagentVolume SendRequestGetReagentVolume(RequestGetReagentVolume request);
        VcbResult SendRequestSetReagentVolume(RequestSetReagentVolume request);
        VcbResult SendRequestAddReagentVolume(RequestAddReagentVolume request);
        VcbResult SendRequestShutdownOrReboot(RequestShutdownOrReboot request);
        VcbResult SendRequestDeleteCampaignData(RequestDeleteCampaignData request);
        VcbResultStartExport SendRequestStartLogDataExport(RequestStartLogDataExport request);
        VcbResult SendRequestPrimeReagents(RequestPrimeReagents request);
        VcbResult SendRequestCancelPrimeReagents(RequestCancelPrimeReagents request);
        VcbResult SendRequestPurgeReagents(RequestPurgeReagents request);
        VcbResult SendRequestCancelPurgeReagents(RequestCancelPurgeReagents request);
        VcbResult SendRequestDecontaminate(RequestDecontaminate request);
        VcbResult SendRequestCancelDecontaminate(RequestCancelDecontaminate request);
        
        void AddRegisteredEvent(IRegisteredEvent registeredEvent);
    }
}
