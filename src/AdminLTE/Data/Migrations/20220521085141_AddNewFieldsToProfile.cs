using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminLTE.Data.Migrations
{
    public partial class AddNewFieldsToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalorieIntake",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForDietPlan",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaterIntake",
                table: "Profile",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalorieIntake",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ReasonForDietPlan",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "WaterIntake",
                table: "Profile");
        }
    }
}
