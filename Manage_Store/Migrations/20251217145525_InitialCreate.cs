using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage_Store.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "categories",
                newName: "CategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "enum('admin','staff', 'customer', 'manager')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('admin','staff', 'customer')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "categories",
                newName: "category_name");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "enum('admin','staff', 'customer')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('admin','staff', 'customer', 'manager')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
