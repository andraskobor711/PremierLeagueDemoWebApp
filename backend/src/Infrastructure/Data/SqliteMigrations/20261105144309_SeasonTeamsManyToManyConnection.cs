using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueWebApi.Infrastructure.Data.SqliteMigrations
{
    /// <inheritdoc />
    public partial class SeasonTeamsManyToManyConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SeasonTeams",
                columns: table => new
                {
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTeams", x => new { x.SeasonId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_SeasonTeams_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTeams_TeamId",
                table: "SeasonTeams",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonTeams");

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
    }
}
