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

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skills",
                newSchema: "PortfolioSchema");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "PortfolioSchema");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "Media",
                newSchema: "PortfolioSchema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Skills",
                schema: "PortfolioSchema",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "PortfolioSchema",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Media",
                schema: "PortfolioSchema",
                newName: "Media");
        }
    }
}
