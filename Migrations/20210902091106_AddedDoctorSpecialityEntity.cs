using Microsoft.EntityFrameworkCore.Migrations;

namespace MarkMyDoctor.Migrations
{
    public partial class AddedDoctorSpecialityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorsId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialitiesId",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality");

            migrationBuilder.RenameColumn(
                name: "SpecialitiesId",
                table: "DoctorSpeciality",
                newName: "SpecialityId");

            migrationBuilder.RenameColumn(
                name: "DoctorsId",
                table: "DoctorSpeciality",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpeciality_SpecialitiesId",
                table: "DoctorSpeciality",
                newName: "IX_DoctorSpeciality_SpecialityId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DoctorSpeciality",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpeciality_DoctorId",
                table: "DoctorSpeciality",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorId",
                table: "DoctorSpeciality",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialityId",
                table: "DoctorSpeciality",
                column: "SpecialityId",
                principalTable: "Speciality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialityId",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSpeciality_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DoctorSpeciality");

            migrationBuilder.RenameColumn(
                name: "SpecialityId",
                table: "DoctorSpeciality",
                newName: "SpecialitiesId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "DoctorSpeciality",
                newName: "DoctorsId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpeciality_SpecialityId",
                table: "DoctorSpeciality",
                newName: "IX_DoctorSpeciality_SpecialitiesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality",
                columns: new[] { "DoctorsId", "SpecialitiesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorsId",
                table: "DoctorSpeciality",
                column: "DoctorsId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialitiesId",
                table: "DoctorSpeciality",
                column: "SpecialitiesId",
                principalTable: "Speciality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
