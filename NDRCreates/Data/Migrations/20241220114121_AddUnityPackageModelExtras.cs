using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDRCreates.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUnityPackageModelExtras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UnityPackage",
                newName: "ThumbnailPath");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "UnityPackage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "UnityPackage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UnityPackage",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UnityPackage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "UnityPackage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnityPackage_UserId",
                table: "UnityPackage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnityPackage_AspNetUsers_UserId",
                table: "UnityPackage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnityPackage_AspNetUsers_UserId",
                table: "UnityPackage");

            migrationBuilder.DropIndex(
                name: "IX_UnityPackage_UserId",
                table: "UnityPackage");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "UnityPackage");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "UnityPackage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UnityPackage");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "UnityPackage");

            migrationBuilder.RenameColumn(
                name: "ThumbnailPath",
                table: "UnityPackage",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "UnityPackage",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
