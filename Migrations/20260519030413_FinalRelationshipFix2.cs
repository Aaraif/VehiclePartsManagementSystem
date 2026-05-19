using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiclePartsManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FinalRelationshipFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Parts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_SupplierId",
                table: "Parts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Suppliers_SupplierId",
                table: "Parts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Suppliers_SupplierId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_SupplierId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Parts");
        }
    }
}