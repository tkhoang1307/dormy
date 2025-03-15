using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IOvernightAbsenceService
    {
        Task<ApiResponse> AddOvernightAbsence(OvernightAbsentRequestModel model);
        Task<ApiResponse> GetDetailOvernightAbsence(Guid id);
        Task<ApiResponse> GetOvernightAbsenceBatch(GetBatchRequestModel model);
        Task<ApiResponse> UpdateOvernightAbsence(OvernightAbsentUpdationRequestModel model);
        Task<ApiResponse> UpdateStatusOvernightAbsence(Guid id, OvernightAbsenceStatusEnum status);
        Task<ApiResponse> SoftDeleteOvernightAbsence(Guid id);
    }
}
