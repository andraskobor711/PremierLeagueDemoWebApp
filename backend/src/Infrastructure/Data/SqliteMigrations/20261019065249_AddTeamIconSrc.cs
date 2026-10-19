using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueWebApi.Infrastructure.Data.SqliteMigrations
{
    /// <inheritdoc />
    public partial class AddTeamIconSrc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconSrc",
                table: "Teams",
                type: "TEXT",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconSrc",
                table: "Teams");
        }
    }
}
