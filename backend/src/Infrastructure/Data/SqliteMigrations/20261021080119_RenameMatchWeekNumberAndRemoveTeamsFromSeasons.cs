using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueWebApi.Infrastructure.Data.SqliteMigrations
{
    /// <inheritdoc />
    public partial class RenameMatchWeekNumberAndRemoveTeamsFromSeasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonTeam");

            migrationBuilder.RenameColumn(
                name: "MatchWeekNumber",
                table: "Matches",
                newName: "GameWeekNumber");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Seasons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_TeamId",
                table: "Seasons",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Teams_TeamId",
                table: "Seasons",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Teams_TeamId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_TeamId",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Seasons");

            migrationBuilder.RenameColumn(
                name: "GameWeekNumber",
                table: "Matches",
                newName: "MatchWeekNumber");

            migrationBuilder.CreateTable(
                name: "SeasonTeam",
                columns: table => new
                {
                    SeasonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTeam", x => new { x.SeasonsId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_SeasonTeam_Seasons_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTeam_TeamsId",
                table: "SeasonTeam",
                column: "TeamsId");
        }
    }
}
