using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingSet.Migrations
{
    /// <inheritdoc />
    public partial class sturcturechangeinmettingmasterandmeetingdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meeting_Minutes_Details_Tbls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Agenda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discussion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meeting_Minutes_Details_Tbls", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meeting_Minutes_Details_Tbls");
        }
    }
}
