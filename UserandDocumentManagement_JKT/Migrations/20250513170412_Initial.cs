using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserandDocumentManagement_JKT.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngestionStatuses_UploadDocuments_UploadDocumentsId",
                table: "IngestionStatuses");

            migrationBuilder.DropTable(
                name: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_IngestionStatuses_UploadDocumentsId",
                table: "IngestionStatuses");

            migrationBuilder.DropColumn(
                name: "UploadDocumentsId",
                table: "IngestionStatuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UploadDocumentsId",
                table: "IngestionStatuses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UsersProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngestionStatuses_UploadDocumentsId",
                table: "IngestionStatuses",
                column: "UploadDocumentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngestionStatuses_UploadDocuments_UploadDocumentsId",
                table: "IngestionStatuses",
                column: "UploadDocumentsId",
                principalTable: "UploadDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
