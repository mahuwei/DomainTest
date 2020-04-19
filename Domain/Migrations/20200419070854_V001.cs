using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class V001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(maxLength: 10, nullable: false),
                    RowFlag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Memo = table.Column<string>(maxLength: 200, nullable: true),
                    No = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address_Street = table.Column<string>(maxLength: 30, nullable: true),
                    Address_City = table.Column<string>(maxLength: 20, nullable: true),
                    Address_ZipCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(maxLength: 10, nullable: false),
                    RowFlag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Memo = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyId = table.Column<Guid>(nullable: false),
                    MobileNo = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Address_Street = table.Column<string>(maxLength: 30, nullable: true),
                    Address_City = table.Column<string>(maxLength: 20, nullable: true),
                    Address_ZipCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
