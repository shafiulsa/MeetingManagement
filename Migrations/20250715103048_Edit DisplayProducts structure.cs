using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingSet.Migrations
{
    /// <inheritdoc />
    public partial class EditDisplayProductsstructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DisplayProduct",
                table: "DisplayProduct");

            migrationBuilder.RenameTable(
                name: "DisplayProduct",
                newName: "DisplayProducts");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "DisplayProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisplayProducts",
                table: "DisplayProducts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DisplayProducts",
                table: "DisplayProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DisplayProducts");

            migrationBuilder.RenameTable(
                name: "DisplayProducts",
                newName: "DisplayProduct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisplayProduct",
                table: "DisplayProduct",
                column: "Id");
        }
    }
}
