using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.API.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Reviews_ReviewId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ReviewId",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Rating",
                newName: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MovieId",
                table: "Rating",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Movies_MovieId",
                table: "Rating",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Movies_MovieId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_MovieId",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Rating",
                newName: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ReviewId",
                table: "Rating",
                column: "ReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Reviews_ReviewId",
                table: "Rating",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
