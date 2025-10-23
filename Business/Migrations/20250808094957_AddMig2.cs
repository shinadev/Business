using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Migrations
{
    /// <inheritdoc />
    public partial class AddMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavMenus_NavMenus_ParentId",
                table: "NavMenus");

            migrationBuilder.DropIndex(
                name: "IX_NavMenus_ParentId",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "FooterMenus");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "TopBarMenus",
                newName: "YouTubeUrl");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TopBarMenus",
                newName: "TwitterUrl");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "NavMenus",
                newName: "MenuName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "NavMenus",
                newName: "Controller");

            migrationBuilder.RenameColumn(
                name: "IsDropdown",
                table: "NavMenus",
                newName: "IsButton");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "TopBarMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "NavMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DropdownGroup",
                table: "NavMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteId",
                table: "NavMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteSlug",
                table: "NavMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutContent",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutTitle",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttributionText",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttributionUrl",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Copyright",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FooterLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkType = table.Column<int>(type: "int", nullable: false),
                    FooterMenuId = table.Column<int>(type: "int", nullable: false),
                    FooterMenuId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterLink_FooterMenus_FooterMenuId",
                        column: x => x.FooterMenuId,
                        principalTable: "FooterMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FooterLink_FooterMenus_FooterMenuId1",
                        column: x => x.FooterMenuId1,
                        principalTable: "FooterMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FooterSocial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterSocial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterSocial_FooterMenus_FooterMenuId",
                        column: x => x.FooterMenuId,
                        principalTable: "FooterMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FooterLink_FooterMenuId",
                table: "FooterLink",
                column: "FooterMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_FooterLink_FooterMenuId1",
                table: "FooterLink",
                column: "FooterMenuId1");

            migrationBuilder.CreateIndex(
                name: "IX_FooterSocial_FooterMenuId",
                table: "FooterSocial",
                column: "FooterMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterLink");

            migrationBuilder.DropTable(
                name: "FooterSocial");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "TopBarMenus");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "DropdownGroup",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "RouteSlug",
                table: "NavMenus");

            migrationBuilder.DropColumn(
                name: "AboutContent",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "AboutTitle",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "AttributionText",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "AttributionUrl",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Copyright",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "FooterMenus");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "FooterMenus");

            migrationBuilder.RenameColumn(
                name: "YouTubeUrl",
                table: "TopBarMenus",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "TwitterUrl",
                table: "TopBarMenus",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "MenuName",
                table: "NavMenus",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "IsButton",
                table: "NavMenus",
                newName: "IsDropdown");

            migrationBuilder.RenameColumn(
                name: "Controller",
                table: "NavMenus",
                newName: "Title");

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "TopBarMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "TopBarMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "NavMenus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FooterMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "FooterMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NavMenus_ParentId",
                table: "NavMenus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavMenus_NavMenus_ParentId",
                table: "NavMenus",
                column: "ParentId",
                principalTable: "NavMenus",
                principalColumn: "Id");
        }
    }
}
