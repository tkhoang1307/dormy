using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatenotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("a65a5c9c-38d6-42a4-bf72-6c63118df8ec"));

            migrationBuilder.AddColumn<string>(
                name: "NotificationType",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 8, 58, 424, DateTimeKind.Local).AddTicks(7927));

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 425, DateTimeKind.Utc).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 8, 58, 425, DateTimeKind.Local).AddTicks(653));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 8, 58, 432, DateTimeKind.Local).AddTicks(377));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("c2318d88-52cb-4553-80f3-0a86ac9fc3e9"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 20, 19, 8, 58, 432, DateTimeKind.Local).AddTicks(8310), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 8, 58, 432, DateTimeKind.Local).AddTicks(1403));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 8, 58, 425, DateTimeKind.Local).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 425, DateTimeKind.Utc).AddTicks(328));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("c2318d88-52cb-4553-80f3-0a86ac9fc3e9"));

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 17, 38, 40, 681, DateTimeKind.Local).AddTicks(1633));

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 10, 38, 40, 681, DateTimeKind.Utc).AddTicks(8093));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 17, 38, 40, 681, DateTimeKind.Local).AddTicks(4747));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 17, 38, 40, 684, DateTimeKind.Local).AddTicks(8581));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("a65a5c9c-38d6-42a4-bf72-6c63118df8ec"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 20, 17, 38, 40, 685, DateTimeKind.Local).AddTicks(1172), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 17, 38, 40, 684, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 17, 38, 40, 681, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 10, 38, 40, 681, DateTimeKind.Utc).AddTicks(4443));
        }
    }
}
