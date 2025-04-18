using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class setData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(2917), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Le", "MALE", false, "Admin", "Long", null, null, "admin", "0895940404", "admin" });

            migrationBuilder.InsertData(
                table: "HealthInsurances",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "ExpirationDate", "InsuranceCardNumber", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RegisteredHospital" },
                values: new object[] { new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(5794), new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "12312312", false, null, null, "Benh vien cho ray" });

            migrationBuilder.InsertData(
                table: "RoomServices",
                columns: new[] { "Id", "Cost", "CreatedBy", "CreatedDateUtc", "IsDeleted", "IsServiceIndicatorUsed", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceName", "RoomServiceType", "Unit" },
                values: new object[] { new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), 3000m, new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"), new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(2746), false, true, null, null, "Water", "WATER", "m3" });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Capacity", "CreatedBy", "CreatedDateUtc", "Description", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "Price", "RoomTypeName" },
                values: new object[] { new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"), 5, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(3218), "Room type normal", false, null, null, 100000m, "Normal" });

            migrationBuilder.InsertData(
                table: "Workplaces",
                columns: new[] { "Id", "Abbrevation", "Address", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "Name" },
                values: new object[] { new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"), "20000", "VNG Q1 HCM City", new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"), new DateTime(2025, 4, 18, 13, 38, 29, 385, DateTimeKind.Utc).AddTicks(5232), false, null, null, "VNG Block 1 HCM" });

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("d44951ae-1b0f-480b-934c-009d7dd8ba6d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(9588), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "HealthInsuranceId", "IsDeleted", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "NationalIdNumber", "Password", "PhoneNumber", "Status", "UserName", "WorkplaceId" },
                values: new object[] { new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(7471), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@gmail.com", "Van", "MALE", new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"), false, "Ba", null, null, "30239840329", "user", "0123456789", "ACTIVE", "user", new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7") });

            migrationBuilder.InsertData(
                table: "Guardians",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedDateUtc", "Email", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "Name", "PhoneNumber", "RelationshipToUser", "UserId" },
                values: new object[] { new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"), "Kien Giang", new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"), new DateTime(2025, 4, 18, 13, 38, 29, 385, DateTimeKind.Utc).AddTicks(9012), "bacba@gmail.com", false, null, null, "Bac Ba", "09737338939", "Cha", new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"));

            migrationBuilder.DeleteData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"));

            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("d44951ae-1b0f-480b-934c-009d7dd8ba6d"));

            migrationBuilder.DeleteData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"));

            migrationBuilder.DeleteData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"));

            migrationBuilder.DeleteData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"));
        }
    }
}
