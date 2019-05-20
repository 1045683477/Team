using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Team.Infrastructure.Migrations
{
    public partial class AddRunTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_RunTeamRecords_RunTeamId",
                table: "RunTeamRecords",
                column: "RunTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RunTeamRecords");
        }
    }
}
