using Microsoft.EntityFrameworkCore.Migrations;

namespace Fortifex4.Infrastructure.Persistence.Migrations
{
    public partial class Fortifex4DB_005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFromCoinMarketCap",
                table: "Currencies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromCoinMarketCap",
                table: "Currencies");
        }
    }
}
