using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookPromoTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TargetPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    WasRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_BookId",
                table: "Alerts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CreatedAt",
                table: "Alerts",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");
        }
    }
}
