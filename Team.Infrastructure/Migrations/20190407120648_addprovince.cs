using Microsoft.EntityFrameworkCore.Migrations;

namespace Team.Infrastructure.Migrations
{
    public partial class addprovince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Province",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Province",
                table: "Users");
        }
    }
}
