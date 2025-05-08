using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class HealthInsuranceConfiguration : IEntityTypeConfiguration<HealthInsuranceEntity>
    {
        public void Configure(EntityTypeBuilder<HealthInsuranceEntity> builder)
        {
            builder
                .Property(healthInsurance => healthInsurance.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
