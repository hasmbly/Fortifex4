using Microsoft.EntityFrameworkCore.Migrations;

namespace Fortifex4.Infrastructure.Persistence.Migrations
{
    public partial class Fortifex4DB_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume24h",
                table: "Currencies",
                type: "decimal(29,15)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(29,10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume24h",
                table: "Currencies",
                type: "decimal(29,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(29,15)");
        }
    }
}
