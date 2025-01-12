using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;

namespace Dormy.WebService.Api.Startup
{
    public interface IUnitOfWork
    {
        public IAdminRepository AdminRepository { get; }

        public IBedRepository BedRepository { get; }
        
        public IBuildingRepository BuildingRepository { get; }

        public IContractExtensionRepository ContractExtensionRepository { get; }

        public IContractRepository ContractRepository { get; }

        public IGuardianRepository GuardianRepository { get; }

        public IHealthInsuranceRepository HealthInsuranceRepository { get; }

        public IInvoiceItemRepository InvoiceItemRepository { get; }

        public IInvoiceRepository InvoiceRepository { get; }    

        public INotificationRepository NotificationRepository { get; }  

        public IOvernightAbsenceRepository OvernightAbsenceRepository { get; }  

        public IParkingRequestRepository ParkingRequestRepository { get; }

        public IParkingSpotRepository ParkingSpotRepository { get; }

        public IRequestRepository RequestRepository { get; }    

        public IRoomRepository RoomRepository { get; }

        public IRoomServiceRepository RoomServiceRepository { get; }    

        public IRoomTypeRepository RoomTypeRepository { get; }

        public IRoomTypeServiceRepository RoomTypeServiceRepository { get; }

        public IServiceIndicatorRepository ServiceIndicatorRepository { get; }

        public ISettingRepository SettingRepository { get; }

        public IUserRepository UserRepository { get; }

        public IVehicleHistoryRepository VehicleHistoryRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        public IViolationRepository ViolationRepository { get; }

        public IWorkplaceRepository WorkplaceRepository { get; }

        Task SaveChangeAsync();
    }
}
