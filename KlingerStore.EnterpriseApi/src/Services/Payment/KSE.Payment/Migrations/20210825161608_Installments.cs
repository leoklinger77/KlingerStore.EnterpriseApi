using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Payment.Migrations
{
    public partial class Installments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_TaxaTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Installments = table.Column<int>(nullable: false),
                    Taxa = table.Column<double>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TaxaTransaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_TaxaTransaction");
        }
    }
}
