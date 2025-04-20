using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateContractFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rooms_RoomId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ApproverId",
                table: "Contracts");

            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("d6c06528-0fb2-45c4-b077-a44124c12e6b"));

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Contracts");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "Requests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "ContractExtensions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "ContractExtensions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_ContractExtensions_RoomId",
                table: "ContractExtensions",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractExtensions_Rooms_RoomId",
                table: "ContractExtensions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rooms_RoomId",
                table: "Requests",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractExtensions_Rooms_RoomId",
                table: "ContractExtensions");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rooms_RoomId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_ContractExtensions_RoomId",
                table: "ContractExtensions");

            migrationBuilder.DeleteData(
                table: "RoomTypeServices",
                keyColumn: "Id",
                keyValue: new Guid("a65a5c9c-38d6-42a4-bf72-6c63118df8ec"));

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "ContractExtensions");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ContractExtensions");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "Requests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "Contracts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "Contracts",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("49e51699-8249-4cc3-899b-bb0b9773e2c3"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 21, 14, 33, 957, DateTimeKind.Local).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("f851a1d0-d1d5-44c7-be81-2c204f0149ba"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 14, 14, 33, 958, DateTimeKind.Utc).AddTicks(1090));

            migrationBuilder.UpdateData(
                table: "HealthInsurances",
                keyColumn: "Id",
                keyValue: new Guid("aabe7f9b-92ba-44ee-a73c-cae62fdfabcc"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 21, 14, 33, 957, DateTimeKind.Local).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "RoomServices",
                keyColumn: "Id",
                keyValue: new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 21, 14, 33, 961, DateTimeKind.Local).AddTicks(5038));

            migrationBuilder.InsertData(
                table: "RoomTypeServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "IsDeleted", "LastUpdatedBy", "LastUpdatedDateUtc", "RoomServiceId", "RoomTypeId" },
                values: new object[] { new Guid("d6c06528-0fb2-45c4-b077-a44124c12e6b"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 4, 18, 21, 14, 33, 962, DateTimeKind.Local).AddTicks(1820), false, null, null, new Guid("24bddd89-6e61-4f8b-8522-7ef98ad58655"), new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9") });

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("af4a50d0-c16e-4804-81e1-4622513eeef9"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 21, 14, 33, 961, DateTimeKind.Local).AddTicks(5666));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3f86e0b9-868c-49fb-a0de-a527d467a673"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 21, 14, 33, 957, DateTimeKind.Local).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "Workplaces",
                keyColumn: "Id",
                keyValue: new Guid("3ebe6594-a32f-4b10-8c4e-401a1042a1e7"),
                column: "CreatedDateUtc",
                value: new DateTime(2025, 4, 18, 14, 14, 33, 957, DateTimeKind.Utc).AddTicks(7485));

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ApproverId",
                table: "Contracts",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rooms_RoomId",
                table: "Requests",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
