using Microsoft.EntityFrameworkCore.Migrations;

namespace MarkMyDoctor.Migrations
{
    public partial class AddedDoctorFacilityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Doctor_DoctorsId",
                table: "DoctorFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Facility_FacilitiesId",
                table: "DoctorFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility");

            migrationBuilder.RenameColumn(
                name: "FacilitiesId",
                table: "DoctorFacility",
                newName: "FacilityId");

            migrationBuilder.RenameColumn(
                name: "DoctorsId",
                table: "DoctorFacility",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorFacility_FacilitiesId",
                table: "DoctorFacility",
                newName: "IX_DoctorFacility_FacilityId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DoctorFacility",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFacility_DoctorId",
                table: "DoctorFacility",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacility_Doctor_DoctorId",
                table: "DoctorFacility",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacility_Facility_FacilityId",
                table: "DoctorFacility",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Doctor_DoctorId",
                table: "DoctorFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Facility_FacilityId",
                table: "DoctorFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility");

            migrationBuilder.DropIndex(
                name: "IX_DoctorFacility_DoctorId",
                table: "DoctorFacility");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DoctorFacility");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "DoctorFacility",
                newName: "FacilitiesId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "DoctorFacility",
                newName: "DoctorsId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorFacility_FacilityId",
                table: "DoctorFacility",
                newName: "IX_DoctorFacility_FacilitiesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility",
                columns: new[] { "DoctorsId", "FacilitiesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacility_Doctor_DoctorsId",
                table: "DoctorFacility",
                column: "DoctorsId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacility_Facility_FacilitiesId",
                table: "DoctorFacility",
                column: "FacilitiesId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
