using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gamification.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthFieldsToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    XpReward = table.Column<int>(type: "integer", nullable: false),
                    RuleType = table.Column<int>(type: "integer", nullable: false),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    RuleParam = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    TotalXp = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAchievements",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievementId = table.Column<string>(type: "text", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAchievements", x => new { x.PlayerId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_PlayerAchievements_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XpTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XpTransactions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Code", "Description", "IsSecret", "RuleParam", "RuleType", "Title", "XpReward" },
                values: new object[,]
                {
                    { "COLLECTOR_500", "Набрано 500 XP", false, 500, 1, "Коллекционер 500", 50 },
                    { "COLLECTOR_5000", "Набрано 5000 XP", false, 5000, 1, "Коллекционер 5000", 250 },
                    { "DEDICATED", "10 действий с источником lesson", false, 10, 2, "Преданный", 60 },
                    { "FIRST_STEPS", "Игрок зарегистрирован", false, 1, 0, "Первый шаг", 10 },
                    { "LEVEL_10", "Достигнут 10-й уровень", false, 10, 0, "Уровень 10", 300 },
                    { "LEVEL_2", "Достигнут 2-й уровень", false, 2, 0, "Уровень 2", 25 },
                    { "LEVEL_5", "Достигнут 5-й уровень", false, 5, 0, "Уровень 5", 100 },
                    { "MARATHON_7", "7 действий с любым источником", false, 7, 2, "Марафонец", 40 },
                    { "SECRET", "Секретное достижение", true, 7, 0, "Секретное", 200 },
                    { "SHOPAHOLIC", "5 действий с источником purchase", false, 5, 2, "Шопоголик", 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_XpTransactions_PlayerId",
                table: "XpTransactions",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "PlayerAchievements");

            migrationBuilder.DropTable(
                name: "XpTransactions");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
