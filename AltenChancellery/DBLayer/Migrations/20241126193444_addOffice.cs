using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBLayer.Migrations
{
    /// <inheritdoc />
    public partial class addOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficeId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Office_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId",
                principalTable: "Office",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Office_OfficeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "AspNetUsers");
        }
    }
}
