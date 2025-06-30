using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DetailFilm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailFilms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriptionLongue = table.Column<string>(type: "text", nullable: false),
                    DateSortieFilm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateSortiePlateforme = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AgeMinimum = table.Column<int>(type: "integer", nullable: false),
                    Acteurs = table.Column<string>(type: "text", nullable: false),
                    Realisateurs = table.Column<string>(type: "text", nullable: false),
                    FilmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailFilms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailFilms");
        }
    }
}
