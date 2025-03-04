using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Dormy.WebService.Api.Infrastructure.Postgres.Repositories;

namespace Dormy.WebService.Api.Startup
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IAdminRepository AdminRepository { get; }

        public IBuildingRepository BuildingRepository { get; }

        public IContractExtensionRepository ContractExtensionRepository { get; }

        public IContractRepository ContractRepository { get; }

        public IGuardianRepository GuardianRepository { get; }

        public IHealthInsuranceRepository HealthInsuranceRepository { get; }

        public IInvoiceItemRepository InvoiceItemRepository { get; }

        public IInvoiceRepository InvoiceRepository { get; }

        public IInvoiceUserRepository InvoiceUserRepository { get; }

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

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            AdminRepository = new AdminRepository(_context);
            BuildingRepository = new BuildingRepository(_context);
            ContractRepository = new ContractRepository(_context);
            ContractExtensionRepository = new ContractExtensionRepository(_context);
            GuardianRepository = new GuardianRepository(_context);
            HealthInsuranceRepository  = new HealthInsuranceRepository(_context);
            InvoiceItemRepository  = new InvoiceItemRepository(_context);
            InvoiceRepository = new InvoiceRepository(_context);
            InvoiceUserRepository = new InvoiceUserRepository(_context);
            NotificationRepository = new NotificationRepository(_context);
            OvernightAbsenceRepository = new OvernightAbsenceRepository(_context);
            ParkingRequestRepository = new ParkingRequestRepository(_context);
            ParkingSpotRepository  = new ParkingSpotRepository(_context);
            RequestRepository  = new RequestRepository(_context);
            RoomRepository = new RoomRepository(_context);
            RoomServiceRepository = new RoomServiceRepository(_context);
            RoomTypeRepository = new RoomTypeRepository(_context);
            RoomTypeServiceRepository = new RoomTypeServiceRepository(_context);
            ServiceIndicatorRepository = new ServiceIndicatorRepository(_context);
            SettingRepository  = new SettingRepository(_context);
            UserRepository = new UserRepository(_context);
            VehicleHistoryRepository = new VehicleHistoryRepository(_context);
            VehicleRepository = new VehicleRepository(_context);
            ViolationRepository = new ViolationRepository(_context);
            WorkplaceRepository = new WorkplaceRepository(_context);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
