using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class AdminMapper
    {
        public AdminEntity MapToAdminEntity(AdminRequestModel model)
        {
            return new AdminEntity()
            {
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                JobTitle = model.JobTitle,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = EncryptHelper.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                CreatedDateUtc = DateTime.UtcNow,
            };
        }

        public AdminResponseModel MapToAdminResponseModel(AdminEntity adminEntity)
        {
            return new AdminResponseModel()
            {
                Id = adminEntity.Id,
                DateOfBirth = adminEntity.DateOfBirth,
                Email = adminEntity.Email,
                FirstName = adminEntity.FirstName,
                LastName = adminEntity.LastName,
                Gender = adminEntity.Gender,
                JobTitle = adminEntity.JobTitle,
                PhoneNumber = adminEntity.PhoneNumber,
                UserName = adminEntity.UserName,
            };
        }
    }
}
