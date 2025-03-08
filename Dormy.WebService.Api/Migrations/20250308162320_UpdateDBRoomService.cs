using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBRoomService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceUsers_Invoices_InvoiceId",
                table: "invoiceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceUsers_Users_UserId",
                table: "invoiceUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_invoiceUsers",
                table: "invoiceUsers");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("159fb5f2-aff8-4075-ad17-b73ea64d6940"));

            migrationBuilder.RenameTable(
                name: "invoiceUsers",
                newName: "InvoiceUsers");

            migrationBuilder.RenameIndex(
                name: "IX_invoiceUsers_UserId",
                table: "InvoiceUsers",
                newName: "IX_InvoiceUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_invoiceUsers_InvoiceId",
                table: "InvoiceUsers",
                newName: "IX_InvoiceUsers_InvoiceId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ServiceIndicators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsServiceIndicatorUsed",
                table: "RoomServices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RoomServiceType",
                table: "RoomServices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "Contracts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceUsers",
                table: "InvoiceUsers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("c62a7e77-107e-4056-a3b6-9fed7c5189e3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 8, 23, 23, 20, 412, DateTimeKind.Local).AddTicks(7484), new DateTime(2025, 3, 8, 23, 23, 20, 412, DateTimeKind.Local).AddTicks(7490), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceUsers_Invoices_InvoiceId",
                table: "InvoiceUsers",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceUsers_Users_UserId",
                table: "InvoiceUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceUsers_Invoices_InvoiceId",
                table: "InvoiceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceUsers_Users_UserId",
                table: "InvoiceUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceUsers",
                table: "InvoiceUsers");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("c62a7e77-107e-4056-a3b6-9fed7c5189e3"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceIndicators");

            migrationBuilder.DropColumn(
                name: "IsServiceIndicatorUsed",
                table: "RoomServices");

            migrationBuilder.DropColumn(
                name: "RoomServiceType",
                table: "RoomServices");

            migrationBuilder.RenameTable(
                name: "InvoiceUsers",
                newName: "invoiceUsers");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceUsers_UserId",
                table: "invoiceUsers",
                newName: "IX_invoiceUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceUsers_InvoiceId",
                table: "invoiceUsers",
                newName: "IX_invoiceUsers_InvoiceId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_invoiceUsers",
                table: "invoiceUsers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("159fb5f2-aff8-4075-ad17-b73ea64d6940"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 2, 16, 22, 16, 10, 688, DateTimeKind.Local).AddTicks(7276), new DateTime(2025, 2, 16, 22, 16, 10, 688, DateTimeKind.Local).AddTicks(7282), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceUsers_Invoices_InvoiceId",
                table: "invoiceUsers",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceUsers_Users_UserId",
                table: "invoiceUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
