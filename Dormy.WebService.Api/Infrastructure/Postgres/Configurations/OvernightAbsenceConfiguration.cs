using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class OvernightAbsenceConfiguration : IEntityTypeConfiguration<OvernightAbsenceEntity>
    {
        public void Configure(EntityTypeBuilder<OvernightAbsenceEntity> builder)
        {
            builder
                .Property(overnightAbsence => overnightAbsence.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(overnightAbsence => overnightAbsence.Status)
                .HasConversion<string>();

            builder
                .HasOne(overnightAbsence => overnightAbsence.User)
                .WithMany(user => user.OvernightAbsences)
                .HasForeignKey(overnightAbsence => overnightAbsence.UserId);

            builder
                .HasOne(overnightAbsence => overnightAbsence.Approver)
                .WithMany(admin => admin.OvernightAbsences)
                .HasForeignKey(overnightAbsence => overnightAbsence.ApproverId);
        }
    }
}
