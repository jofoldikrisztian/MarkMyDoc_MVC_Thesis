using Microsoft.EntityFrameworkCore.Migrations;

namespace MarkMyDoctor.Migrations
{
    public partial class FixedFlexibilityRatingName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FelxibilityRating",
                table: "Reviews",
                newName: "FlexibilityRating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlexibilityRating",
                table: "Reviews",
                newName: "FelxibilityRating");
        }
    }
}
