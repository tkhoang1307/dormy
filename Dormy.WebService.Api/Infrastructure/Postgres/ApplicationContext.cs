using Dormy.WebService.Api.Core.Entities;
using Dormy.WebService.Api.Infrastructure.Postgres.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            //do nothing
        }

        protected ApplicationContext()
        {
            //do nothing
        }

        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<BuildingEntity> Buildings { get; set; }

        public DbSet<ContractEntity> Contracts { get; set; }

        public DbSet<ContractExtensionEntity> ContractExtensions { get; set; }
        public DbSet<GuardianEntity> Guardians { get; set; }
        public DbSet<HealthInsuranceEntity> HealthInsurances { get; set; }

        public DbSet<InvoiceEntity> Invoices { get; set; }

        public DbSet<InvoiceItemEntity> InvoiceItems { get; set; }
        public DbSet<InvoiceUserEntity> InvoiceUsers { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<OvernightAbsenceEntity> OvernightAbsences { get; set; }
        public DbSet<ParkingRequestEntity> ParkingRequests { get; set; }
        public DbSet<ParkingSpotEntity> ParkingSpots { get; set; }

        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoomServiceEntity> RoomServices { get; set; }
        public DbSet<RoomTypeEntity> RoomTypes { get; set; }
        public DbSet<RoomTypeServiceEntity> RoomTypeServices { get; set; }

        public DbSet<ServiceIndicatorEntity> ServiceIndicators { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<VehicleHistoryEntity> VehicleHistories { get; set; }
        public DbSet<ViolationEntity> Violations { get; set; }
        public DbSet<WorkplaceEntity> Workplaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new ContractExtensionConfiguration());
            modelBuilder.ApplyConfiguration(new GuardianConfiguration());
            modelBuilder.ApplyConfiguration(new HealthInsuranceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceUserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new OvernightAbsenceConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingSpotConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomServiceConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceIndicatorConfigurationn());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new ViolationConfiguration());
            modelBuilder.ApplyConfiguration(new WorkplaceConfiguration());
        }
    }
}
