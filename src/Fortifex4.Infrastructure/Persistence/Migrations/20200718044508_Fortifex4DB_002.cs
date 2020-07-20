using Microsoft.EntityFrameworkCore.Migrations;

namespace Fortifex4.Infrastructure.Persistence.Migrations
{
    public partial class Fortifex4DB_002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocument_Projects_ProjectID",
                table: "ProjectDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatusLog_Projects_ProjectID",
                table: "ProjectStatusLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatusLog",
                table: "ProjectStatusLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDocument",
                table: "ProjectDocument");

            migrationBuilder.RenameTable(
                name: "ProjectStatusLog",
                newName: "ProjectStatusLogs");

            migrationBuilder.RenameTable(
                name: "ProjectDocument",
                newName: "ProjectDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStatusLog_ProjectID",
                table: "ProjectStatusLogs",
                newName: "IX_ProjectStatusLogs_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDocument_ProjectID",
                table: "ProjectDocuments",
                newName: "IX_ProjectDocuments_ProjectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatusLogs",
                table: "ProjectStatusLogs",
                column: "ProjectStatusLogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDocuments",
                table: "ProjectDocuments",
                column: "ProjectDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_Projects_ProjectID",
                table: "ProjectDocuments",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatusLogs_Projects_ProjectID",
                table: "ProjectStatusLogs",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocuments_Projects_ProjectID",
                table: "ProjectDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatusLogs_Projects_ProjectID",
                table: "ProjectStatusLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatusLogs",
                table: "ProjectStatusLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDocuments",
                table: "ProjectDocuments");

            migrationBuilder.RenameTable(
                name: "ProjectStatusLogs",
                newName: "ProjectStatusLog");

            migrationBuilder.RenameTable(
                name: "ProjectDocuments",
                newName: "ProjectDocument");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStatusLogs_ProjectID",
                table: "ProjectStatusLog",
                newName: "IX_ProjectStatusLog_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDocuments_ProjectID",
                table: "ProjectDocument",
                newName: "IX_ProjectDocument_ProjectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatusLog",
                table: "ProjectStatusLog",
                column: "ProjectStatusLogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDocument",
                table: "ProjectDocument",
                column: "ProjectDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocument_Projects_ProjectID",
                table: "ProjectDocument",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatusLog_Projects_ProjectID",
                table: "ProjectStatusLog",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
