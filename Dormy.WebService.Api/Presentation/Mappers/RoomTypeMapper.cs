using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Mappers
{
    public class RoomTypeMapper
    {
        public RoomTypeEntity MapToRoomTypeEnity(RoomTypeRequestModel model)
        {
            return new RoomTypeEntity
            {
                Id = Guid.NewGuid(),
                Capacity = model.Capacity,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
                Description = model.Description,
                Price = model.Price,
                RoomTypeName = model.RoomTypeName,
                RoomTypeServices = model.RoomServiceIds.Select(id => MapToRoomTypeServiceEntity(id)).ToList(),
            };
        }

        public RoomTypeResponseModel MapToRoomTypeResponseModel(RoomTypeEntity entity)
        {
            return new RoomTypeResponseModel
            {
                Id = entity.Id,
                Capacity = entity.Capacity,
                CreatedDateUtc = entity.CreatedDateUtc,
                Description = entity.Description,
                Price = entity.Price,
                RoomTypeName = entity.RoomTypeName,
                LastUpdatedBy = entity.LastUpdatedBy,
                LastUpdatedDateUtc = entity.LastUpdatedDateUtc,
                CreatedBy = entity.CreatedBy,
                IsDeleted = entity.IsDeleted,
                RoomServices = MapToRoomServiceModels(entity),
            };
        }
        public RoomTypeRegistrationResponseModel MapToRoomTypeRegistrationResponseModel(RoomTypeResponseModel source)
        {
            return new RoomTypeRegistrationResponseModel
            {
                Id = source.Id,
                Capacity = source.Capacity,
                Description = source.Description,
                Price = source.Price,
                RoomTypeName = source.RoomTypeName,
                RoomServices = source.RoomServices.Select(x => new RoomServiceRegistrationResponseModel
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    Unit = x.Unit,
                    RoomServiceName = x.RoomServiceName,
                }).ToList(),
            };
        }

        public RoomTypeServiceEntity MapToRoomTypeServiceEntity(Guid roomServiceId)
        {
            return new RoomTypeServiceEntity
            {
                RoomServiceId = roomServiceId,
                CreatedDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
            };
        }

        public List<RoomServiceResponseModel> MapToRoomServiceModels(RoomTypeEntity source)
        {
            if (source.RoomTypeServices == null || source.RoomTypeServices.Count == 0)
            {
                return [];
            }

            var roomServices = source.RoomTypeServices.Select(x => x.RoomService).DistinctBy(x => x.Id).ToList();

            return roomServices.Select(x => new RoomServiceResponseModel
            {
                Id = x.Id,
                Cost = x.Cost,
                Unit = x.Unit,
                RoomServiceName = x.RoomServiceName,
                CreatedBy = x.CreatedBy,
                IsDeleted = x.IsDeleted,
                CreatedDateUtc = x.CreatedDateUtc,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdatedDateUtc = x.LastUpdatedDateUtc,
            }).ToList();
        }
    }
}
