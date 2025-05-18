using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IInvoiceService _invoiceService;
        private ParkingSpotMapper _parkingSpotMapper;
        private readonly VehicleMapper _vehicleMapper;

        public ParkingSpotService(IUnitOfWork unitOfWork, IUserContextService userContextService, IInvoiceService invoiceService)
        {
            _unitOfWork = unitOfWork;
            _parkingSpotMapper = new ParkingSpotMapper();
            _userContextService = userContextService;
            _vehicleMapper = new VehicleMapper();
            _invoiceService = invoiceService;
        }

        public async Task<ApiResponse> AddNewParkingSpot(ParkingSpotRequestModel model)
        {
            var entity = _parkingSpotMapper.MapToParkingSpotEntity(model);

            entity.CreatedBy = _userContextService.UserId;
            entity.LastUpdatedBy = _userContextService.UserId;

            await _unitOfWork.ParkingSpotRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetCreated(entity.Id);
        }

        public async Task<ApiResponse> GetDetailParkingSpot(Guid id)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            var parkingSpotModel = _parkingSpotMapper.MapToParkingSpotModel(entity);

            var vehicleEntities = await _unitOfWork.VehicleRepository.GetAllAsync(x => x.ParkingSpotId == entity.Id, include: x => x.Include(x => x.User), isPaging: false);
            if (vehicleEntities != null && vehicleEntities.Count != 0)
            {
                if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
                {
                    vehicleEntities = vehicleEntities.Where(x => x.UserId == _userContextService.UserId).ToList();
                }
                parkingSpotModel.Vehicles = vehicleEntities.Select(v => _vehicleMapper.MapToVehicleResponseModel(v)).ToList();
            }

            var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(parkingSpotModel.CreatedBy, parkingSpotModel.LastUpdatedBy);

            parkingSpotModel.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
            parkingSpotModel.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

            return new ApiResponse().SetOk(parkingSpotModel);
        }

        public async Task<ApiResponse> GetParkingSpotBatch(GetBatchRequestModel model)
        {
            var entities = new List<ParkingSpotEntity>();

            entities = await _unitOfWork.ParkingSpotRepository.GetAllAsync(x => true);

            if (model.Ids.Count > 0)
            {
                entities = entities.Where(x => model.Ids.Contains(x.Id)).ToList();
            }

            var parkingSpotModels = new List<ParkingSpotResponseModel>();

            if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
            {
                parkingSpotModels = entities.Where(x => x.IsDeleted == false)
                                            .Select(x => _parkingSpotMapper.MapToParkingSpotModel(x))
                                            .ToList();
            }
            else
            {
                parkingSpotModels = entities.Select(x => _parkingSpotMapper.MapToParkingSpotModel(x)).ToList();

            }

            for (int i = 0; i < parkingSpotModels.Count; i++)
            {
                var parkingSpot = parkingSpotModels[i];

                var (createdUser, lastUpdatedUser) = await _unitOfWork.AdminRepository.GetAuthors(parkingSpot.CreatedBy, parkingSpot.LastUpdatedBy);

                parkingSpot.CreatedByCreator = UserHelper.ConvertAdminIdToAdminFullname(createdUser);
                parkingSpot.LastUpdatedByUpdater = UserHelper.ConvertAdminIdToAdminFullname(lastUpdatedUser);

                var vehicleEntities = await _unitOfWork.VehicleRepository.GetAllAsync(x => x.ParkingSpotId == parkingSpot.Id, include: x => x.Include(x => x.User), isPaging: false);
                if (vehicleEntities != null && vehicleEntities.Count != 0)
                {
                    if (_userContextService.UserRoles.FirstOrDefault() == Role.USER)
                    {
                        vehicleEntities = vehicleEntities.Where(x => x.UserId == _userContextService.UserId).ToList();
                    }
                    parkingSpot.Vehicles = vehicleEntities.Select(v => _vehicleMapper.MapToVehicleResponseModel(v)).ToList();
                }
            }

            return new ApiResponse().SetOk(parkingSpotModels);
        }

        public async Task<ApiResponse> UpdateParkingSpot(ParkingSpotUpdationRequestModel model)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            if (entity.CurrentQuantity >= model.CapacitySpots)
            {
                entity.Status = ParkingSpotStatusEnum.FULL;
            }

            entity.CapacitySpots = model.CapacitySpots;
            entity.ParkingSpotName = model.ParkingSpotName;
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> SoftDeleteParkingSpot(Guid id)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            entity.IsDeleted = true;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk(id);
        }

        public async Task<ApiResponse> UpdateStatusParkingSpot(ParkingSpotUpdateStatusRequestModel model)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(model.Id, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            entity.Status = (ParkingSpotStatusEnum)Enum.Parse(typeof(ParkingSpotStatusEnum), model.Status);
            entity.LastUpdatedBy = _userContextService.UserId;
            entity.LastUpdatedDateUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetAccepted(entity.Id);
        }

        public async Task<ApiResponse> CreateParkingSpotInvoiceForAllUsers(Guid parkingSpotId)
        {
            var entity = await _unitOfWork.ParkingSpotRepository.GetAsync(x => x.Id == parkingSpotId);

            if (entity == null)
            {
                return new ApiResponse().SetNotFound(parkingSpotId, message: string.Format(ErrorMessages.PropertyDoesNotExist, "Parking spot"));
            }

            var vehicleEntities = await _unitOfWork.VehicleRepository.GetAllAsync(x => x.ParkingSpotId == parkingSpotId, include: x => x.Include(x => x.User));
            if (vehicleEntities != null && vehicleEntities.Count != 0)
            {
                var settingEntity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == SettingKeyname.ParkingPriceKeyname);
                decimal parkingPrice = 0;
                if (settingEntity != null && settingEntity.IsApplied)
                {
                    if (!decimal.TryParse(settingEntity?.Value?.ToString(), out parkingPrice))
                    {
                        parkingPrice = 50000;
                    }
                }

                settingEntity = await _unitOfWork.SettingRepository.GetAsync(x => x.KeyName == SettingKeyname.ExtendDueDateForPaymentParkingFee);
                int extendedDueDate = 15;
                if (settingEntity != null && settingEntity.IsApplied)
                {
                    if (!int.TryParse(settingEntity?.Value?.ToString(), out extendedDueDate))
                    {
                        extendedDueDate = 15;
                    }
                }

                var listUserId = vehicleEntities.Select(x => x.UserId).ToList();
                foreach(var userId  in listUserId)
                {
                    var responseContract = await _unitOfWork.ContractRepository.GetAllAsync(x => x.UserId == userId);
                    var roomId = responseContract.FirstOrDefault().RoomId;
                    var responseCreateInvoice = await _invoiceService.CreateNewInvoice(new InvoiceRequestModel()
                    {
                        DueDate = DateTime.Now.AddDays(extendedDueDate).Date,
                        Type = InvoiceTypeEnum.PARKING_INVOICE.ToString(),
                        RoomId = roomId,
                        InvoiceItems = new List<InvoiceItemRequestModel>()
                            {
                                new InvoiceItemRequestModel()
                                {
                                    RoomServiceName = "Parking fee",
                                    Cost = parkingPrice,
                                    Unit = "month",
                                    Quantity = 1,
                                }
                            }
                    }, userIdForParkingInvoice: userId, parkingSpotName: entity.ParkingSpotName);

                    if (!responseCreateInvoice.IsSuccess)
                    {
                        return responseCreateInvoice;
                    }
                }

            }

            return new ApiResponse().SetCreated(entity.Id);
        }
    }
}
