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
        public DbSet<BedEntity> Beds { get; set; }
        public DbSet<BuildingEntity> Buildings { get; set; }
        public DbSet<GuardianEntity> Guardians { get; set; }
        public DbSet<HealthInsuranceEntity> HealthInsurances { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<OvernightAbsenceEntity> OvernightAbsences { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoomServiceEntity> RoomServices { get; set; }
        public DbSet<RoomTypeEntity> RoomTypes { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ViolationEntity> Violations { get; set; }
        public DbSet<WorkplaceEntity> Workplaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new BedConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new GuardianConfiguration());
            modelBuilder.ApplyConfiguration(new HealthInsuranceConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new OvernightAbsenceConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomServiceConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ViolationConfiguration());
            modelBuilder.ApplyConfiguration(new WorkplaceConfiguration());
        }
    }
}
