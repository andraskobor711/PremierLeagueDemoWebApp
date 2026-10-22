using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueWebApi.Infrastructure.Data.SqliteMigrations
{
    /// <inheritdoc />
    public partial class RenameMatchweekToMatchWeekNumberOnMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Matchweek",
                table: "Matches",
                newName: "MatchWeekNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchWeekNumber",
                table: "Matches",
                newName: "Matchweek");
        }
    }
}
