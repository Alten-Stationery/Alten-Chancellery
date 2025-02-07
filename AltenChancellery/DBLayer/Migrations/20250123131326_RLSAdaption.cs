using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBLayer.Migrations
{
    /// <inheritdoc />
    public partial class RLSAdaption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemOffice_ItemId",
                table: "ItemOffice");

            migrationBuilder.AddColumn<string>(
                name: "RlsId",
                table: "Office",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRls",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ItemOffice_ItemId_OfficeId",
                table: "ItemOffice",
                columns: new[] { "ItemId", "OfficeId" });

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alert_ItemOffice_ItemId_OfficeId",
                        columns: x => new { x.ItemId, x.OfficeId },
                        principalTable: "ItemOffice",
                        principalColumns: new[] { "ItemId", "OfficeId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Office_RlsId",
                table: "Office",
                column: "RlsId",
                unique: true,
                filter: "[RlsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_ItemId_OfficeId",
                table: "Alert",
                columns: new[] { "ItemId", "OfficeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Office_AspNetUsers_RlsId",
                table: "Office",
                column: "RlsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Office_AspNetUsers_RlsId",
                table: "Office");

            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Office_RlsId",
                table: "Office");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ItemOffice_ItemId_OfficeId",
                table: "ItemOffice");

            migrationBuilder.DropColumn(
                name: "RlsId",
                table: "Office");

            migrationBuilder.DropColumn(
                name: "IsRls",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOffice_ItemId",
                table: "ItemOffice",
                column: "ItemId");
        }
    }
}
