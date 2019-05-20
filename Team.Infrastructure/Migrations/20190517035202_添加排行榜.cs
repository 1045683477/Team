using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Team.Infrastructure.Migrations
{
    public partial class 添加排行榜 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunTeamState",
                table: "Users",
                newName: "RunTeamId");

            migrationBuilder.RenameColumn(
                name: "Kcal",
                table: "Statisticals",
                newName: "Calorie");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "RunTeams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RunApplicants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunApplicants_RunTeams_RunTeamId",
                        column: x => x.RunTeamId,
                        principalTable: "RunTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunTimeDailyChartses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ranking = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(type: "date", nullable: false),
                    ClockIn = table.Column<int>(nullable: false),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTimeDailyChartses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTimeDailyChartses_RunTeams_RunTeamId",
                        column: x => x.RunTeamId,
                        principalTable: "RunTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunTimeWeekChartses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: false),
                    Ranking = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    ClockIn = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTimeWeekChartses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RunApplicants_RunTeamId",
                table: "RunApplicants",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunTimeDailyChartses_RunTeamId",
                table: "RunTimeDailyChartses",
                column: "RunTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RunApplicants");

            migrationBuilder.DropTable(
                name: "RunTimeDailyChartses");

            migrationBuilder.DropTable(
                name: "RunTimeWeekChartses");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "RunTeams");

            migrationBuilder.RenameColumn(
                name: "RunTeamId",
                table: "Users",
                newName: "RunTeamState");

            migrationBuilder.RenameColumn(
                name: "Calorie",
                table: "Statisticals",
                newName: "Kcal");
        }
    }
}
