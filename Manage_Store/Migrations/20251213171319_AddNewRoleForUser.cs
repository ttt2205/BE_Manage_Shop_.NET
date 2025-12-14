using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage_Store.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRoleForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "enum('admin','staff', 'customer')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('admin','staff')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "enum('admin','staff')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('admin','staff', 'customer')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
