using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormy.WebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("159fb5f2-aff8-4075-ad17-b73ea64d6940"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "Contracts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "DateOfBirth", "Email", "FirstName", "Gender", "IsDeleted", "JobTitle", "LastName", "LastUpdatedBy", "LastUpdatedDateUtc", "Password", "PhoneNumber", "UserName" },
                values: new object[] { new Guid("f8072fbf-9d7d-4994-980f-a185f7fd881f"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 3, 4, 2, 45, 43, 945, DateTimeKind.Local).AddTicks(3078), new DateTime(2025, 3, 4, 2, 45, 43, 945, DateTimeKind.Local).AddTicks(3114), "hungdv190516@gmail.com", "Admin", "MALE", false, "Admin", "", null, null, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts",
                column: "ApproverId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Admins_ApproverId",
                table: "Contracts");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("f8072fbf-9d7d-4994-980f-a185f7fd881f"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ApproverId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
        }
    }
}
