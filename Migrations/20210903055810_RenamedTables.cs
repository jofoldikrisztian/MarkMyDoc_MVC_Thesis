using Microsoft.EntityFrameworkCore.Migrations;

namespace MarkMyDoctor.Migrations
{
    public partial class RenamedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Doctor_DoctorId",
                table: "DoctorFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacility_Facility_FacilityId",
                table: "DoctorFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialityId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_Facility_City_CityId",
                table: "Facility");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Doctor_DoctorId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSpeciality_SpecialityId",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facility",
                table: "Facility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility");

            migrationBuilder.DropIndex(
                name: "IX_DoctorFacility_DoctorId",
                table: "DoctorFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Speciality",
                newName: "Specialities");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Facility",
                newName: "Facilities");

            migrationBuilder.RenameTable(
                name: "DoctorFacility",
                newName: "DoctorFacilities");

            migrationBuilder.RenameTable(
                name: "Doctor",
                newName: "Doctors");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_User_Name",
                table: "Users",
                newName: "IX_Users_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Speciality_Name",
                table: "Specialities",
                newName: "IX_Specialities_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_DoctorId",
                table: "Reviews",
                newName: "IX_Reviews_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Facility_Name",
                table: "Facilities",
                newName: "IX_Facilities_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Facility_CityId",
                table: "Facilities",
                newName: "IX_Facilities_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorFacility_FacilityId",
                table: "DoctorFacilities",
                newName: "IX_DoctorFacilities_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_City_Name",
                table: "Cities",
                newName: "IX_Cities_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialities",
                table: "Specialities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorFacilities",
                table: "DoctorFacilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpeciality_SpecialityId_DoctorId",
                table: "DoctorSpeciality",
                columns: new[] { "SpecialityId", "DoctorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFacilities_DoctorId_FacilityId",
                table: "DoctorFacilities",
                columns: new[] { "DoctorId", "FacilityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacilities_Doctors_DoctorId",
                table: "DoctorFacilities",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorFacilities_Facilities_FacilityId",
                table: "DoctorFacilities",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Doctors_DoctorId",
                table: "DoctorSpeciality",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Specialities_SpecialityId",
                table: "DoctorSpeciality",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Cities_CityId",
                table: "Facilities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Doctors_DoctorId",
                table: "Reviews",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacilities_Doctors_DoctorId",
                table: "DoctorFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorFacilities_Facilities_FacilityId",
                table: "DoctorFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Doctors_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Specialities_SpecialityId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Cities_CityId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Doctors_DoctorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSpeciality_SpecialityId_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialities",
                table: "Specialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorFacilities",
                table: "DoctorFacilities");

            migrationBuilder.DropIndex(
                name: "IX_DoctorFacilities_DoctorId_FacilityId",
                table: "DoctorFacilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Specialities",
                newName: "Speciality");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Facilities",
                newName: "Facility");

            migrationBuilder.RenameTable(
                name: "Doctors",
                newName: "Doctor");

            migrationBuilder.RenameTable(
                name: "DoctorFacilities",
                newName: "DoctorFacility");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Name",
                table: "User",
                newName: "IX_User_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_Name",
                table: "Speciality",
                newName: "IX_Speciality_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_DoctorId",
                table: "Review",
                newName: "IX_Review_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_Name",
                table: "Facility",
                newName: "IX_Facility_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_CityId",
                table: "Facility",
                newName: "IX_Facility_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorFacilities_FacilityId",
                table: "DoctorFacility",
                newName: "IX_DoctorFacility_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_Name",
                table: "City",
                newName: "IX_City_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facility",
                table: "Facility",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorFacility",
                table: "DoctorFacility",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpeciality_SpecialityId",
                table: "DoctorSpeciality",
                column: "SpecialityId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_City_CityId",
                table: "Facility",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Doctor_DoctorId",
                table: "Review",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
