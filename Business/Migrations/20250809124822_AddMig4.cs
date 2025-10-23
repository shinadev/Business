using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Migrations
{
    /// <inheritdoc />
    public partial class AddMig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CaptionSmall = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CaptionLarge = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Button1Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Button1Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Button2Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Button2Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSection", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeSection");
        }
    }
}
