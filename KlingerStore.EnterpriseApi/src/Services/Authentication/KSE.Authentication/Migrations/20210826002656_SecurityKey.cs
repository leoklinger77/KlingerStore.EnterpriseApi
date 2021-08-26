using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KSE.Authentication.Migrations
{
    public partial class SecurityKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    KeyId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    JwsAlgorithm = table.Column<string>(nullable: true),
                    JweAlgorithm = table.Column<string>(nullable: true),
                    JweEncryption = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    JwkType = table.Column<int>(nullable: false),
                    IsRevoked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityKeys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityKeys");
        }
    }
}
