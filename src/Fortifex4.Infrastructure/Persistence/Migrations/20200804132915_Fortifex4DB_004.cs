using Microsoft.EntityFrameworkCore.Migrations;

namespace Fortifex4.Infrastructure.Persistence.Migrations
{
    public partial class Fortifex4DB_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceInUSD",
                table: "Currencies",
                type: "decimal(29,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(29,20)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceInUSD",
                table: "Currencies",
                type: "decimal(29,20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(29,18)");
        }
    }
}
