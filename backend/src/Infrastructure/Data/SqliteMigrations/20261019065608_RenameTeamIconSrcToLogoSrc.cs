using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueWebApi.Infrastructure.Data.SqliteMigrations
{
    /// <inheritdoc />
    public partial class RenameTeamIconSrcToLogoSrc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconSrc",
                table: "Teams",
                newName: "LogoSrc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoSrc",
                table: "Teams",
                newName: "IconSrc");
        }
    }
}
