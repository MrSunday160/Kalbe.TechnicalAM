using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kalbe.TechnicalAM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "Users",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Nickname");
        }
    }
}
