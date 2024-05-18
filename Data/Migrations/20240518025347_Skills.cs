using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharlmagneHenryAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Skills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Skills_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SkillId",
                table: "Projects",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ParentId",
                table: "Skills",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Skills_SkillId",
                table: "Projects",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Skills_SkillId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SkillId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Projects");
        }
    }
}
