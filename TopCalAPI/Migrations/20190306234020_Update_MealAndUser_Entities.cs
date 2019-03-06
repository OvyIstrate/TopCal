using Microsoft.EntityFrameworkCore.Migrations;

namespace TopCalAPI.Migrations
{
    public partial class Update_MealAndUser_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Meals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Meals",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "varchar(30",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_ApplicationUserId",
                table: "Meals",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_AspNetUsers_ApplicationUserId",
                table: "Meals",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_AspNetUsers_ApplicationUserId",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_ApplicationUserId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Meals");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);
        }
    }
}
