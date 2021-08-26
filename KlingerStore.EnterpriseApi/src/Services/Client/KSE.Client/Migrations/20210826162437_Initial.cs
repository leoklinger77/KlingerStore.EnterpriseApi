using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Client.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_TB_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Address_TB_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TB_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Address_ClientId",
                table: "TB_Address",
                column: "ClientId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Address");

            migrationBuilder.DropTable(
                name: "TB_Client");
        }
    }
}
