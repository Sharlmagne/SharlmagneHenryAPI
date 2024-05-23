using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharlmagneHenryAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectSkillRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Skills_SkillId",
                schema: "PortfolioSchema",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SkillId",
                schema: "PortfolioSchema",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SkillId",
                schema: "PortfolioSchema",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ProjectSkills",
                schema: "PortfolioSchema",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkills", x => new { x.ProjectsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_ProjectSkills_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalSchema: "PortfolioSchema",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSkills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalSchema: "PortfolioSchema",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkills_SkillsId",
                schema: "PortfolioSchema",
                table: "ProjectSkills",
                column: "SkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSkills",
                schema: "PortfolioSchema");

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                schema: "PortfolioSchema",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SkillId",
                schema: "PortfolioSchema",
                table: "Projects",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Skills_SkillId",
                schema: "PortfolioSchema",
                table: "Projects",
                column: "SkillId",
                principalSchema: "PortfolioSchema",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
