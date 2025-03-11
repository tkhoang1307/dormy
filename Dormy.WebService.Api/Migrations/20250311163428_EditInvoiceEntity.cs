using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class EditInvoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("7dc1f8a5-f467-47b1-a6fb-acdf5a99c49a"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminId",
                table: "OvernightAbsences",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomServiceId",
                table: "InvoiceItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("69ddb490-cfb9-4416-9b89-570da765f9fe"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 11, 23, 34, 28, 417, DateTimeKind.Local).AddTicks(8593), new DateTime(2025, 3, 11, 23, 34, 28, 417, DateTimeKind.Local).AddTicks(8598), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("69ddb490-cfb9-4416-9b89-570da765f9fe"));

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Invoices");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminId",
                table: "OvernightAbsences",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomServiceId",
                table: "InvoiceItems",
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

            migrationBuilder.AddForeignKey(
                name: "FK_OvernightAbsences_Admins_AdminId",
                table: "OvernightAbsences",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
