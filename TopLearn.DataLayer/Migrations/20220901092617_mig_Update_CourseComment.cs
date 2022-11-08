using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_Update_CourseComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_Courses_ErrorMessage",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_ErrorMessage",
                table: "CourseComments");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "CourseComments");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_CourseId",
                table: "CourseComments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_Courses_CourseId",
                table: "CourseComments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_Courses_CourseId",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_CourseId",
                table: "CourseComments");

            migrationBuilder.AddColumn<int>(
                name: "ErrorMessage",
                table: "CourseComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_ErrorMessage",
                table: "CourseComments",
                column: "ErrorMessage");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_Courses_ErrorMessage",
                table: "CourseComments",
                column: "ErrorMessage",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
