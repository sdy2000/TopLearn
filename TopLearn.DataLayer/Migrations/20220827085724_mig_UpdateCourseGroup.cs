using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_UpdateCourseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_ParentId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "CourseGroup");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_ParentId",
                table: "CourseGroup",
                newName: "IX_CourseGroup_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseGroup",
                table: "CourseGroup",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseGroup_CourseGroup_ParentId",
                table: "CourseGroup",
                column: "ParentId",
                principalTable: "CourseGroup",
                principalColumn: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseGroup_CourseGroup_ParentId",
                table: "CourseGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseGroup",
                table: "CourseGroup");

            migrationBuilder.RenameTable(
                name: "CourseGroup",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_CourseGroup_ParentId",
                table: "Courses",
                newName: "IX_Courses_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_ParentId",
                table: "Courses",
                column: "ParentId",
                principalTable: "Courses",
                principalColumn: "GroupId");
        }
    }
}
