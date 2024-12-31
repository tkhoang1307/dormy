using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminResponseModel>> GetAllUser();
    }
}
