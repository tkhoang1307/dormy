using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dormy.WebService.Api.Core.Configurations
{
    public class OvernightAbsenceConfiguration : IEntityTypeConfiguration<OvernightAbsenceEntity>
    {
        public void Configure(EntityTypeBuilder<OvernightAbsenceEntity> builder)
        {
            builder
                .Property(overnightAbsence => overnightAbsence.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(overnightAbsence => overnightAbsence.User)
                .WithMany(user => user.OvernightAbsences)
                .HasForeignKey(overnightAbsence => overnightAbsence.UserId);

            builder
                .HasOne(overnightAbsence => overnightAbsence.Admin)
                .WithMany(admin => admin.OvernightAbsences)
                .HasForeignKey(overnightAbsence => overnightAbsence.AdminId);
        }
    }
}
