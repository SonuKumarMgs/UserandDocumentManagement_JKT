using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserandDocumentManagement_JKT.Migrations
{
    /// <inheritdoc />
    public partial class Init04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngestionStatuses_UploadDocumentDocuments_UploadDocumentsId",
                table: "IngestionStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadDocumentDocuments_Users_OwnerId",
                table: "UploadDocumentDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadDocumentDocuments",
                table: "UploadDocumentDocuments");

            migrationBuilder.RenameTable(
                name: "UploadDocumentDocuments",
                newName: "UploadDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_UploadDocumentDocuments_OwnerId",
                table: "UploadDocuments",
                newName: "IX_UploadDocuments_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadDocuments",
                table: "UploadDocuments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsersProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersProfiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_IngestionStatuses_UploadDocuments_UploadDocumentsId",
                table: "IngestionStatuses",
                column: "UploadDocumentsId",
                principalTable: "UploadDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UploadDocuments_Users_OwnerId",
                table: "UploadDocuments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngestionStatuses_UploadDocuments_UploadDocumentsId",
                table: "IngestionStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadDocuments_Users_OwnerId",
                table: "UploadDocuments");

            migrationBuilder.DropTable(
                name: "UsersProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadDocuments",
                table: "UploadDocuments");

            migrationBuilder.RenameTable(
                name: "UploadDocuments",
                newName: "UploadDocumentDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_UploadDocuments_OwnerId",
                table: "UploadDocumentDocuments",
                newName: "IX_UploadDocumentDocuments_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadDocumentDocuments",
                table: "UploadDocumentDocuments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngestionStatuses_UploadDocumentDocuments_UploadDocumentsId",
                table: "IngestionStatuses",
                column: "UploadDocumentsId",
                principalTable: "UploadDocumentDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UploadDocumentDocuments_Users_OwnerId",
                table: "UploadDocumentDocuments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
