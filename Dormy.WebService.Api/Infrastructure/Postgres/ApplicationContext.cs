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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());

        }
    }
}
