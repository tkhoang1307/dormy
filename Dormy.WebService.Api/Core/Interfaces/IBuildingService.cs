﻿using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IBuildingService
    {
        Task<ApiResponse> CreateBuilding(BuildingCreationRequestModel model);
        Task<ApiResponse> CreateBuildingBatch(List<BuildingCreationRequestModel> models);
        Task<ApiResponse> GetBuildingById(Guid id);
        Task<ApiResponse> GetBuildingBatch(GetBatchRequestModel request);
        Task<ApiResponse> UpdateBuilding(BuildingUpdationRequestModel model);
        Task<ApiResponse> SoftDeleteBuildingById(Guid id);
    }
}
