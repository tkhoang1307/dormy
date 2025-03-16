using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class EditDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractExtensions_Admins_ApproverId",
                table: "ContractExtensions");

            migrationBuilder.DropForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRequests_Admins_ApproverId",
                table: "ParkingRequests");

            migrationBuilder.DropIndex(
                name: "IX_OvernightAbsences_AdminId",
                table: "OvernightAbsences");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("7dc1f8a5-f467-47b1-a6fb-acdf5a99c49a"));

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "OvernightAbsences");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "ParkingRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "OvernightAbsences",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Invoices",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomServiceId",
                table: "InvoiceItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "NewIndicator",
                table: "InvoiceItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OldIndicator",
                table: "InvoiceItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "ContractExtensions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_OvernightAbsences_ApproverId",
                table: "OvernightAbsences",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractExtensions_Admins_ApproverId",
                table: "ContractExtensions",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OvernightAbsences_Admins_ApproverId",
                table: "OvernightAbsences",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRequests_Admins_ApproverId",
                table: "ParkingRequests",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractExtensions_Admins_ApproverId",
                table: "ContractExtensions");

            migrationBuilder.DropForeignKey(
                name: "FK_OvernightAbsences_Admins_ApproverId",
                table: "OvernightAbsences");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRequests_Admins_ApproverId",
                table: "ParkingRequests");

            migrationBuilder.DropIndex(
                name: "IX_OvernightAbsences_ApproverId",
                table: "OvernightAbsences");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "OvernightAbsences");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "NewIndicator",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "OldIndicator",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "ParkingRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "OvernightAbsences",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomServiceId",
                table: "InvoiceItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "InvoiceItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "ContractExtensions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("7dc1f8a5-f467-47b1-a6fb-acdf5a99c49a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 9, 17, 47, 11, 228, DateTimeKind.Local).AddTicks(7509), new DateTime(2025, 3, 9, 17, 47, 11, 228, DateTimeKind.Local).AddTicks(7515), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_OvernightAbsences_AdminId",
                table: "OvernightAbsences",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractExtensions_Admins_ApproverId",
                table: "ContractExtensions",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRequests_Admins_ApproverId",
                table: "ParkingRequests",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
