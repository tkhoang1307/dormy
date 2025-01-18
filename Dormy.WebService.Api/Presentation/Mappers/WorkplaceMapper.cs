using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class WorkplaceMapper
    {
        private readonly UserMapper _userMapper;

        public WorkplaceMapper()
        {
            _userMapper = new UserMapper();
        }

        public WorkplaceEntity MapToWorkplaceEntity(WorkplaceRequestModel request)
        {
            return new WorkplaceEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Abbrevation = request.Abbrevation,
                Address = request.Address,
                CreatedDateUtc = DateTime.UtcNow,
            };
        }

        public WorkplaceResponseModel MapToWorkplaceResponseModel(WorkplaceEntity entity)
        {
            return new WorkplaceResponseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Abbrevation = entity.Abbrevation,
                Address = entity.Address,
                CreatedBy = entity.CreatedBy,
                CreatedDateUtc = entity.CreatedDateUtc,
                isDeleted = entity.isDeleted,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                Users = entity.Users.Select(user => _userMapper.MapToUserResponseModel(user)).ToList(),
            };
        }
    }
}
