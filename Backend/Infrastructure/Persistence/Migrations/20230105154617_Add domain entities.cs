using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Adddomainentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyPhrases",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPhrases", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Url = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteKeyPhrases",
                columns: table => new
                {
                    KeyPhraseName = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(400)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteKeyPhrases", x => new { x.WebsiteUrl, x.KeyPhraseName });
                    table.ForeignKey(
                        name: "FK_WebsiteKeyPhrases_KeyPhrases_KeyPhraseName",
                        column: x => x.KeyPhraseName,
                        principalTable: "KeyPhrases",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteKeyPhrases_Websites_WebsiteUrl",
                        column: x => x.WebsiteUrl,
                        principalTable: "Websites",
                        principalColumn: "Url",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteKeyPhrases_KeyPhraseName",
                table: "WebsiteKeyPhrases",
                column: "KeyPhraseName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteKeyPhrases");

            migrationBuilder.DropTable(
                name: "KeyPhrases");

            migrationBuilder.DropTable(
                name: "Websites");
        }
    }
}
