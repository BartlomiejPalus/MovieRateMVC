using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRateMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToRatingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId_UserId",
                table: "Ratings",
                columns: new[] { "MovieId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_MovieId_UserId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings",
                column: "MovieId");
        }
    }
}
