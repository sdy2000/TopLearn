using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_UpdateUserDiscountCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDiscountCodes_Descounts_DescountDiscountId",
                table: "UserDiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_UserDiscountCodes_DescountDiscountId",
                table: "UserDiscountCodes");

            migrationBuilder.DropColumn(
                name: "DescountDiscountId",
                table: "UserDiscountCodes");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountCodes_DiscountId",
                table: "UserDiscountCodes",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDiscountCodes_Descounts_DiscountId",
                table: "UserDiscountCodes",
                column: "DiscountId",
                principalTable: "Descounts",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDiscountCodes_Descounts_DiscountId",
                table: "UserDiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_UserDiscountCodes_DiscountId",
                table: "UserDiscountCodes");

            migrationBuilder.AddColumn<int>(
                name: "DescountDiscountId",
                table: "UserDiscountCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountCodes_DescountDiscountId",
                table: "UserDiscountCodes",
                column: "DescountDiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDiscountCodes_Descounts_DescountDiscountId",
                table: "UserDiscountCodes",
                column: "DescountDiscountId",
                principalTable: "Descounts",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
