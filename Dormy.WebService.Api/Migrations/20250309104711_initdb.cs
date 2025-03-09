using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TotalFloors = table.Column<int>(type: "integer", nullable: false),
                    GenderRestriction = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthInsurances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InsuranceCardNumber = table.Column<string>(type: "text", nullable: false),
                    RegisteredHospital = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthInsurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkingSpotName = table.Column<string>(type: "text", nullable: false),
                    CapacitySpots = table.Column<int>(type: "integer", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomServiceName = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    RoomServiceType = table.Column<string>(type: "text", nullable: false),
                    IsServiceIndicatorUsed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Abbrevation = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParameterBool = table.Column<bool>(type: "boolean", nullable: false),
                    ParameterDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypeServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypeServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTypeServices_RoomServices_RoomServiceId",
                        column: x => x.RoomServiceId,
                        principalTable: "RoomServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTypeServices_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    FloorNumber = table.Column<int>(type: "integer", nullable: false),
                    TotalUsedBed = table.Column<int>(type: "integer", nullable: false),
                    TotalAvailableBed = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    NationalIdNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    WorkplaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    HealthInsuranceId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_HealthInsurances_HealthInsuranceId",
                        column: x => x.HealthInsuranceId,
                        principalTable: "HealthInsurances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceName = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AmountBeforePromotion = table.Column<decimal>(type: "numeric", nullable: false),
                    AmountAfterPromotion = table.Column<decimal>(type: "numeric", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceIndicators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    OldIndicator = table.Column<decimal>(type: "numeric", nullable: false),
                    NewIndicator = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RoomServiceName = table.Column<string>(type: "text", nullable: false),
                    RoomServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceIndicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceIndicators_RoomServices_RoomServiceId",
                        column: x => x.RoomServiceId,
                        principalTable: "RoomServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceIndicators_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    NumberExtension = table.Column<int>(type: "integer", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Admins_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guardians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    RelationshipToUser = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guardians_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OvernightAbsences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvernightAbsences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OvernightAbsences_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OvernightAbsences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RequestType = table.Column<string>(type: "text", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Admins_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    VehicleType = table.Column<string>(type: "text", nullable: false),
                    ParkingSpotId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ViolationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Penalty = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomServiceName = table.Column<string>(type: "text", nullable: false),
                    RoomServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceUsers_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractExtensions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractExtensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractExtensions_Admins_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractExtensions_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkingSpotId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingRequests_Admins_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingRequests_ParkingSpots_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkingSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingRequests_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkingSpotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleHistories_ParkingSpots_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkingSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleHistories_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("7dc1f8a5-f467-47b1-a6fb-acdf5a99c49a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 9, 17, 47, 11, 228, DateTimeKind.Local).AddTicks(7509), new DateTime(2025, 3, 9, 17, 47, 11, 228, DateTimeKind.Local).AddTicks(7515), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractExtensions_ApproverId",
                table: "ContractExtensions",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractExtensions_ContractId",
                table: "ContractExtensions",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ApproverId",
                table: "Contracts",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RoomId",
                table: "Contracts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_UserId",
                table: "Guardians",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceUsers_InvoiceId",
                table: "InvoiceUsers",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceUsers_UserId",
                table: "InvoiceUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RoomId",
                table: "Invoices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AdminId",
                table: "Notifications",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OvernightAbsences_AdminId",
                table: "OvernightAbsences",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_OvernightAbsences_UserId",
                table: "OvernightAbsences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRequests_ApproverId",
                table: "ParkingRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRequests_ParkingSpotId",
                table: "ParkingRequests",
                column: "ParkingSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRequests_UserId",
                table: "ParkingRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRequests_VehicleId",
                table: "ParkingRequests",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApproverId",
                table: "Requests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RoomId",
                table: "Requests",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeServices_RoomServiceId",
                table: "RoomTypeServices",
                column: "RoomServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeServices_RoomTypeId",
                table: "RoomTypeServices",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndicators_RoomId",
                table: "ServiceIndicators",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceIndicators_RoomServiceId",
                table: "ServiceIndicators",
                column: "RoomServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_AdminId",
                table: "Settings",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HealthInsuranceId",
                table: "Users",
                column: "HealthInsuranceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkplaceId",
                table: "Users",
                column: "WorkplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleHistories_ParkingSpotId",
                table: "VehicleHistories",
                column: "ParkingSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleHistories_VehicleId",
                table: "VehicleHistories",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_UserId",
                table: "Violations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractExtensions");

            migrationBuilder.DropTable(
                name: "Guardians");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "InvoiceUsers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OvernightAbsences");

            migrationBuilder.DropTable(
                name: "ParkingRequests");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "RoomTypeServices");

            migrationBuilder.DropTable(
                name: "ServiceIndicators");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "VehicleHistories");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "RoomServices");

            migrationBuilder.DropTable(
                name: "ParkingSpots");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropTable(
                name: "HealthInsurances");

            migrationBuilder.DropTable(
                name: "Workplaces");
        }
    }
}
