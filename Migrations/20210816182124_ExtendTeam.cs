using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueWebApp.Migrations
{
    public partial class ExtendTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Teams",
                newName: "UsedName");

            migrationBuilder.AddColumn<string>(
                name: "OfficialWebsite",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficialWebsite",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "UsedName",
                table: "Teams",
                newName: "ShortName");
        }
    }
}
