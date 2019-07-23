using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Team.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    RelationShip = table.Column<int>(nullable: false),
                    SonId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    Province = table.Column<int>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LatitudeAndLongitudes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    CommunicationId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatitudeAndLongitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LatitudeAndLongitudes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SportFreeModel = table.Column<int>(nullable: false),
                    StarTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StarPlace = table.Column<string>(nullable: false),
                    EndPlace = table.Column<string>(nullable: false),
                    Distance = table.Column<float>(nullable: false),
                    Calories = table.Column<float>(nullable: false),
                    Speed = table.Column<float>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Runs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Introduction = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    ApplicationStatus = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statisticals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SportFreeModel = table.Column<int>(nullable: false),
                    Distance = table.Column<float>(nullable: false),
                    Calories = table.Column<float>(nullable: false),
                    AllTime = table.Column<float>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statisticals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statisticals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Introduction = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    AgreedTime = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Introduction = table.Column<string>(nullable: true),
                    AllCount = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Sport = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "RunParticipantses",
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
                    table.PrimaryKey("PK_RunParticipantses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunParticipantses_RunTeams_RunTeamId",
                        column: x => x.RunTeamId,
                        principalTable: "RunTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunTeamRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(type: "date", nullable: false),
                    Distance = table.Column<float>(nullable: false),
                    Calories = table.Column<float>(nullable: false),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTeamRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTeamRecords_RunTeams_RunTeamId",
                        column: x => x.RunTeamId,
                        principalTable: "RunTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunTeamWeekChartBuffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Distance = table.Column<float>(nullable: false),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTeamWeekChartBuffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTeamWeekChartBuffers_RunTeams_RunTeamId",
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
                    Distance = table.Column<float>(nullable: false),
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
                    Ranking = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Distance = table.Column<float>(nullable: false),
                    RunTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTimeWeekChartses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTimeWeekChartses_RunTeams_RunTeamId",
                        column: x => x.RunTeamId,
                        principalTable: "RunTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participantses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participantses_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LatitudeAndLongitudes_UserId",
                table: "LatitudeAndLongitudes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantses_TeamId",
                table: "Participantses",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunApplicants_RunTeamId",
                table: "RunApplicants",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunParticipantses_RunTeamId",
                table: "RunParticipantses",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_UserId",
                table: "Runs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RunTeamRecords_RunTeamId",
                table: "RunTeamRecords",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunTeams_UserId",
                table: "RunTeams",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RunTeamWeekChartBuffers_RunTeamId",
                table: "RunTeamWeekChartBuffers",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunTimeDailyChartses_RunTeamId",
                table: "RunTimeDailyChartses",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RunTimeWeekChartses_RunTeamId",
                table: "RunTimeWeekChartses",
                column: "RunTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Statisticals_UserId",
                table: "Statisticals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLists_UserId",
                table: "TeamLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_UserId",
                table: "Teams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "LatitudeAndLongitudes");

            migrationBuilder.DropTable(
                name: "Participantses");

            migrationBuilder.DropTable(
                name: "RunApplicants");

            migrationBuilder.DropTable(
                name: "RunParticipantses");

            migrationBuilder.DropTable(
                name: "Runs");

            migrationBuilder.DropTable(
                name: "RunTeamRecords");

            migrationBuilder.DropTable(
                name: "RunTeamWeekChartBuffers");

            migrationBuilder.DropTable(
                name: "RunTimeDailyChartses");

            migrationBuilder.DropTable(
                name: "RunTimeWeekChartses");

            migrationBuilder.DropTable(
                name: "Statisticals");

            migrationBuilder.DropTable(
                name: "TeamLists");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "RunTeams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
