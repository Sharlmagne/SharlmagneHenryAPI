using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharlmagneHenryAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioSchemaInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PortfolioSchema");

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "PortfolioSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Skills_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "PortfolioSchema",
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "PortfolioSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SkillId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "PortfolioSchema",
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Media",
                schema: "PortfolioSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "PortfolioSchema",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_ProjectId",
                schema: "PortfolioSchema",
                table: "Media",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SkillId",
                schema: "PortfolioSchema",
                table: "Projects",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ParentId",
                schema: "PortfolioSchema",
                table: "Skills",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media",
                schema: "PortfolioSchema");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "PortfolioSchema");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "PortfolioSchema");
        }
    }
}
