using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AdminResponseModel>> GetAllUser()
        {
            var adminEntities = await _unitOfWork.AdminRepository.GetAllAsync(x => true);
            var results = adminEntities.Select(x =>
                new AdminResponseModel
                {
                    Id = x.Id,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    JobTitle = x.JobTitle,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToList();
            return results;
        }
    }
}
