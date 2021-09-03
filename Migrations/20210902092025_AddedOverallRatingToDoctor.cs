using Microsoft.EntityFrameworkCore.Migrations;

namespace MarkMyDoctor.Migrations
{
    public partial class AddedOverallRatingToDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "OverallRating",
                table: "Doctor",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallRating",
                table: "Doctor");
        }
    }
}
