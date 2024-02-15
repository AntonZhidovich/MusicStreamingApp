using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceName",
                table: "Songs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Releases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceName",
                table: "Songs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Releases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "7bfe40e0-f945-487b-ae93-a07cfbdc87db",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 14, 13, 29, 30, 216, DateTimeKind.Local).AddTicks(6445));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326",
                column: "CreatedAt",
                value: new DateTime(2024, 2, 14, 13, 29, 30, 216, DateTimeKind.Local).AddTicks(6427));
        }
    }
}
