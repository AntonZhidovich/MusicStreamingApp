using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubscriptionService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TariffPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MaxPlaylistsCount = table.Column<int>(type: "int", nullable: false),
                    MonthFee = table.Column<double>(type: "float", nullable: false),
                    AnnualFee = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    TariffPlanId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextFeeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Fee = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_TariffPlans_TariffPlanId",
                        column: x => x.TariffPlanId,
                        principalTable: "TariffPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TariffPlans",
                columns: new[] { "Id", "AnnualFee", "Description", "MaxPlaylistsCount", "MonthFee", "Name" },
                values: new object[,]
                {
                    { "1ece6d0a-a08b-4839-a1c5-efe06496df64", 59.990000000000002, "Set playlists for every important moment of your life to share it with you favorite artists.", 25, 6.9900000000000002, "Push the boundaries" },
                    { "7dad33ee-b174-4c27-972e-ddd969145d52", 29.989999999999998, "A minimal plan for those who simply enjoy music.", 3, 2.9900000000000002, "Base" },
                    { "83e7cb90-520c-45c2-b1f2-222974cb74c5", 35.990000000000002, "Tariff plan for multifaceted personality who sets the mood of the day with personal playlists.", 7, 3.9900000000000002, "Enhanced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TariffPlanId",
                table: "Subscriptions",
                column: "TariffPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserName",
                table: "Subscriptions",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TariffPlans_Name",
                table: "TariffPlans",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TariffPlans");
        }
    }
}
