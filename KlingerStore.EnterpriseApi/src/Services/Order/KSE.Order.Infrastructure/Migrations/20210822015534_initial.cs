using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Order.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "MySequel",
                startValue: 1000L);

            migrationBuilder.CreateTable(
                name: "TB_Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(type: "varchar(100)", nullable: false),
                    Percentage = table.Column<decimal>(nullable: true),
                    DiscountValue = table.Column<decimal>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    TypeDiscountVoucher = table.Column<int>(nullable: false),
                    ShelfLife = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Used = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Voucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Code = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR MySequel"),
                    ClientId = table.Column<Guid>(nullable: false),
                    VoucherId = table.Column<Guid>(nullable: true),
                    VoucherUsed = table.Column<bool>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Order_TB_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "TB_Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_OrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Image = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_OrderItem_TB_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TB_Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ShippingAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: false),
                    Street = table.Column<string>(type: "varchar(200)", nullable: false),
                    Number = table.Column<string>(type: "varchar(20)", nullable: false),
                    Complement = table.Column<string>(type: "varchar(200)", nullable: false),
                    District = table.Column<string>(type: "varchar(200)", nullable: false),
                    ZipCode = table.Column<string>(type: "char(8)", nullable: false),
                    City = table.Column<string>(type: "varchar(200)", nullable: false),
                    State = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ShippingAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ShippingAddress_TB_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TB_Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Order_VoucherId",
                table: "TB_Order",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_OrderItem_OrderId",
                table: "TB_OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ShippingAddress_OrderId",
                table: "TB_ShippingAddress",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_OrderItem");

            migrationBuilder.DropTable(
                name: "TB_ShippingAddress");

            migrationBuilder.DropTable(
                name: "TB_Order");

            migrationBuilder.DropTable(
                name: "TB_Voucher");

            migrationBuilder.DropSequence(
                name: "MySequel");
        }
    }
}
