using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Releases_Name",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "7bfe40e0-f945-487b-ae93-a07cfbdc87db",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 26, 15, 19, 40, 580, DateTimeKind.Local).AddTicks(2781));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 26, 15, 19, 40, 580, DateTimeKind.Local).AddTicks(2768));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "7bfe40e0-f945-487b-ae93-a07cfbdc87db",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 15, 21, 49, 9, 443, DateTimeKind.Local).AddTicks(1311));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 15, 21, 49, 9, 443, DateTimeKind.Local).AddTicks(1245));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3e4cc735-f041-424d-9ada-f835a7c1978a",
                columns: new[] { "Email", "FirstName", "LastName", "Region" },
                values: new object[] { "yegor.kozlov02@mail.ru", "Yegor", "Kozlov", "Minsk" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "70d71f5a-a4ef-488a-b4e9-eb86f82481a8",
                columns: new[] { "Email", "FirstName", "LastName", "Region" },
                values: new object[] { "chester.bennington@outlook.com", "Chester", "Bennington", "California" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "76ef2410-4e2e-4542-a20b-b0f19dfd5d76",
                columns: new[] { "Email", "FirstName", "LastName", "Region" },
                values: new object[] { "dmitry.ivanov@gmail.com", "Dmitry", "Ivanov", "Italy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7b761e59-78f3-4862-b1ad-87065bc8f51b",
                columns: new[] { "Email", "FirstName", "LastName", "Region" },
                values: new object[] { "garritsen@gmail.com", "Martijn", "Garritsen", "Netherlands" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "d480a3c1-99aa-4775-819b-94e9183d0e21",
                columns: new[] { "Email", "FirstName", "LastName", "Region" },
                values: new object[] { "shinoda77@gmail.com", "Mike", "Shinoda", "California" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_Name",
                table: "Releases",
                column: "Name",
                unique: true);
        }
    }
}
