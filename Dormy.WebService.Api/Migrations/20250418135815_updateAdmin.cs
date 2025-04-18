using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("d44951ae-1b0f-480b-934c-009d7dd8ba6d"));

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                columns: new[] { "CreatedDateUtc", "Password" },
                values: new object[] { new DateTime(2025, 4, 18, 20, 58, 13, 963, DateTimeKind.Local).AddTicks(810), "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" });

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 13, 58, 13, 963, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 58, 13, 963, DateTimeKind.Local).AddTicks(4853));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 58, 13, 968, DateTimeKind.Local).AddTicks(2313));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("56422101-f0a3-45ab-b641-3a53c8912f96"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 58, 13, 968, DateTimeKind.Local).AddTicks(6425), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 58, 13, 968, DateTimeKind.Local).AddTicks(2591));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 58, 13, 963, DateTimeKind.Local).AddTicks(7409));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 13, 58, 13, 963, DateTimeKind.Utc).AddTicks(3831));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("56422101-f0a3-45ab-b641-3a53c8912f96"));

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                columns: new[] { "CreatedDateUtc", "Password" },
                values: new object[] { new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(2917), "admin" });

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 13, 38, 29, 385, DateTimeKind.Utc).AddTicks(9012));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(5794));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(2746));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("d44951ae-1b0f-480b-934c-009d7dd8ba6d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(9588), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 38, 29, 390, DateTimeKind.Local).AddTicks(3218));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 20, 38, 29, 385, DateTimeKind.Local).AddTicks(7471));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 13, 38, 29, 385, DateTimeKind.Utc).AddTicks(5232));
        }
    }
}
