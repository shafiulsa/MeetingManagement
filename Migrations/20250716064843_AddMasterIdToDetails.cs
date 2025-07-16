using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingSet.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterIdToDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Meeting_Minutes_Details_Tbls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Meeting_Minutes_Details_Tbls");
        }
    }
}
