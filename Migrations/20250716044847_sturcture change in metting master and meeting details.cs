using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingSet.Migrations
{
    /// <inheritdoc />
    public partial class sturcturechangeinmettingmasterandmeetingdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agenda",
                table: "Meeting_Minutes_Master_Tbls");

            migrationBuilder.DropColumn(
                name: "Decision",
                table: "Meeting_Minutes_Master_Tbls");

            migrationBuilder.DropColumn(
                name: "Discussion",
                table: "Meeting_Minutes_Master_Tbls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agenda",
                table: "Meeting_Minutes_Master_Tbls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Decision",
                table: "Meeting_Minutes_Master_Tbls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discussion",
                table: "Meeting_Minutes_Master_Tbls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
