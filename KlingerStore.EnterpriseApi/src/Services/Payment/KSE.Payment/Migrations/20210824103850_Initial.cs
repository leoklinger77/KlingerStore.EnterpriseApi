using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Payment.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    TypePayment = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    CodeAuthorization = table.Column<string>(type: "varchar(255)", nullable: true),
                    BrandCart = table.Column<string>(type: "varchar(255)", nullable: true),
                    TotalValue = table.Column<decimal>(nullable: false),
                    CostTransaction = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateTransaction = table.Column<DateTime>(nullable: false),
                    TID = table.Column<string>(type: "varchar(255)", nullable: true),
                    NSU = table.Column<string>(type: "varchar(255)", nullable: true),
                    PaymentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Transaction_TB_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "TB_Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Transaction_PaymentId",
                table: "TB_Transaction",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Transaction");

            migrationBuilder.DropTable(
                name: "TB_Payment");
        }
    }
}
