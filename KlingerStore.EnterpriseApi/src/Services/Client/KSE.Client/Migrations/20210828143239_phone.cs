using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Client.Migrations
{
    public partial class phone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Phone",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Ddd = table.Column<string>(type: "char(2)", nullable: false),
                    Number = table.Column<string>(type: "varchar(9)", nullable: false),
                    PhoneType = table.Column<int>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Phone_TB_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TB_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Phone_ClientId",
                table: "TB_Phone",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Phone");
        }
    }
}
