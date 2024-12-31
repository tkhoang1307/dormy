﻿// <auto-generated />
using System;
using Dormy.WebService.Api.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.AdminEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.BedEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BedNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Beds");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.BuildingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("FloorNumber")
                        .HasColumnType("integer");

                    b.Property<string>("GenderRestriction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.GuardianEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<string>("RelationshipToUser")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Guardians");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.HealthInsuranceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InsuranceCardNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<string>("RegisteredHospital")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("HealthInsurances");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.NotificationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("isRead")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.OvernightAbsenceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("UserId");

                    b.ToTable("OvernightAbsences");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<int>("FloorNumber")
                        .HasColumnType("integer");

                    b.Property<string>("RoomNumer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RoomTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomServiceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RoomServices");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomUtilityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("ElectricityFee")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ElectricityUsage")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Month")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("WaterFee")
                        .HasColumnType("numeric");

                    b.Property<decimal>("WaterUsage")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("RoomUtilities");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.SettingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ParameterBool")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ParameterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<Guid>("GuardianId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("HealthInsuranceId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NationalIdNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WorkplaceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GuardianId")
                        .IsUnique();

                    b.HasIndex("HealthInsuranceId")
                        .IsUnique();

                    b.HasIndex("WorkplaceId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.ViolationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Penalty")
                        .HasColumnType("numeric");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ViolationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Violations");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.WorkplaceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Abbrevation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LastUpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("RecordVersion")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Workplaces");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.BedEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.RoomEntity", "Room")
                        .WithMany("Beds")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.NotificationEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.AdminEntity", "Admin")
                        .WithMany("Notifications")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dormy.WebService.Api.Core.Entities.UserEntity", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.OvernightAbsenceEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.AdminEntity", "Admin")
                        .WithMany("OvernightAbsences")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dormy.WebService.Api.Core.Entities.UserEntity", "User")
                        .WithMany("OvernightAbsences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.BuildingEntity", "Building")
                        .WithMany("Rooms")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dormy.WebService.Api.Core.Entities.RoomTypeEntity", "RoomType")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.SettingEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.AdminEntity", "Admin")
                        .WithMany("Settings")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.UserEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.GuardianEntity", "Guardian")
                        .WithOne("User")
                        .HasForeignKey("Dormy.WebService.Api.Core.Entities.UserEntity", "GuardianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dormy.WebService.Api.Core.Entities.HealthInsuranceEntity", "HealthInsurance")
                        .WithOne("User")
                        .HasForeignKey("Dormy.WebService.Api.Core.Entities.UserEntity", "HealthInsuranceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dormy.WebService.Api.Core.Entities.WorkplaceEntity", "Workplace")
                        .WithOne("User")
                        .HasForeignKey("Dormy.WebService.Api.Core.Entities.UserEntity", "WorkplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guardian");

                    b.Navigation("HealthInsurance");

                    b.Navigation("Workplace");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.ViolationEntity", b =>
                {
                    b.HasOne("Dormy.WebService.Api.Core.Entities.UserEntity", "User")
                        .WithMany("Violations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.AdminEntity", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("OvernightAbsences");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.BuildingEntity", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.GuardianEntity", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.HealthInsuranceEntity", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomEntity", b =>
                {
                    b.Navigation("Beds");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.RoomTypeEntity", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.UserEntity", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("OvernightAbsences");

                    b.Navigation("Violations");
                });

            modelBuilder.Entity("Dormy.WebService.Api.Core.Entities.WorkplaceEntity", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}