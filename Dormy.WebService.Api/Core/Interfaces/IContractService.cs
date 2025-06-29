﻿using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IContractService
    {
        Task<ApiResponse> Register(RegisterRequestModel model);
        Task<ApiResponse> UpdateContractStatus(Guid id, ContractStatusEnum status);
        Task<ApiResponse> AddNewContract(ContractRequestModel model);
        Task<ApiResponse> GetSingleContract(Guid id);
        Task<ApiResponse> GetContractBatch(GetBatchRequestModel model);
        Task<ApiResponse> GetInitialRegistrationData();
        Task<ApiResponse> GetAllRoomTypesData();
        Task<ApiResponse> GetInitialCreateContractData();
        Task<ApiResponse> GetInitialExtendContractData(Guid contractId);
        Task<ApiResponse> SearchBuildingsAndRoomsByGenderAndRoomType(SearchBuildingAndRoomRequestModel model);
        Task<ApiResponse> SendContractEmail(Guid contractExtensionId, bool isContractExtension = true);
    }
}
