using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatenoti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications");

            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("4cf862f7-b455-4f29-b0be-2c6da92d1e7c"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminId",
                table: "Notifications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 24, 39, 393, DateTimeKind.Local).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 24, 39, 394, DateTimeKind.Utc).AddTicks(973));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 24, 39, 393, DateTimeKind.Local).AddTicks(6876));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 24, 39, 397, DateTimeKind.Local).AddTicks(7806));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("62543c06-8345-4772-8bc7-589a9cc385e1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 20, 19, 24, 39, 398, DateTimeKind.Local).AddTicks(718), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 24, 39, 397, DateTimeKind.Local).AddTicks(8047));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 24, 39, 393, DateTimeKind.Local).AddTicks(8817));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 24, 39, 393, DateTimeKind.Utc).AddTicks(6579));

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications");

            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("62543c06-8345-4772-8bc7-589a9cc385e1"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminId",
                table: "Notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 10, 25, 625, DateTimeKind.Local).AddTicks(8727));

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 10, 25, 626, DateTimeKind.Utc).AddTicks(4767));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 10, 25, 626, DateTimeKind.Local).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 10, 25, 629, DateTimeKind.Local).AddTicks(8539));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("4cf862f7-b455-4f29-b0be-2c6da92d1e7c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 20, 19, 10, 25, 630, DateTimeKind.Local).AddTicks(1678), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 10, 25, 629, DateTimeKind.Local).AddTicks(8880));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 19, 10, 25, 626, DateTimeKind.Local).AddTicks(3125));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 20, 12, 10, 25, 626, DateTimeKind.Utc).AddTicks(1018));

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Admins_AdminId",
                table: "Notifications",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
