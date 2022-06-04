using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminLTE.Data.Migrations
{
    public partial class AddProfileToDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    Hypertension = table.Column<string>(nullable: true),
                    Diabetes = table.Column<string>(nullable: true),
                    LIverKidneyDiseases = table.Column<string>(nullable: true),
                    OtherDiseases = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
