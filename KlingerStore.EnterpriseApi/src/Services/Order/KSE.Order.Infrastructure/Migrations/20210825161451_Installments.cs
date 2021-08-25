using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Order.Infrastructure.Migrations
{
    public partial class Installments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Installments",
                table: "TB_Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Jurus",
                table: "TB_Order",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Installments",
                table: "TB_Order");

            migrationBuilder.DropColumn(
                name: "Jurus",
                table: "TB_Order");
        }
    }
}
