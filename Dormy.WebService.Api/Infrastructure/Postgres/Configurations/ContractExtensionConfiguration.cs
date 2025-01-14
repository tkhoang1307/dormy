﻿using Dormy.WebService.Api.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.Infrastructure.Postgres.Configurations
{
    public class ContractExtensionConfiguration : IEntityTypeConfiguration<ContractExtensionEntity>
    {
        public void Configure(EntityTypeBuilder<ContractExtensionEntity> builder)
        {
            builder
                .Property(contractExtension => contractExtension.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(contractExtension => contractExtension.Approver)
                .WithMany(admin => admin.ContractExtensions)
                .HasForeignKey(contractExtension => contractExtension.ApproverId);
        }
    }
}
