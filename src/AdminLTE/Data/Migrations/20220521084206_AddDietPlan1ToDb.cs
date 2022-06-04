using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminLTE.Data.Migrations
{
    public partial class AddDietPlan1ToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayNo = table.Column<int>(nullable: false),
                    Fats = table.Column<decimal>(nullable: false),
                    Carbohydrates = table.Column<decimal>(nullable: false),
                    Proteins = table.Column<decimal>(nullable: false),
                    WaterIntake = table.Column<decimal>(nullable: false),
                    RequiredCalorieIntake = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietPlan", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietPlan");
        }
    }
}
