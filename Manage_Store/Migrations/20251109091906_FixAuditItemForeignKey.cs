using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage_Store.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditItemForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_audit_items_audit_sessions_AuditSessionId",
                table: "inventory_audit_items");

            migrationBuilder.DropIndex(
                name: "IX_inventory_audit_items_AuditSessionId",
                table: "inventory_audit_items");

            migrationBuilder.DropColumn(
                name: "AuditSessionId",
                table: "inventory_audit_items");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_audit_items_SessionId",
                table: "inventory_audit_items",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_audit_items_audit_sessions_SessionId",
                table: "inventory_audit_items",
                column: "SessionId",
                principalTable: "audit_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_audit_items_audit_sessions_SessionId",
                table: "inventory_audit_items");

            migrationBuilder.DropIndex(
                name: "IX_inventory_audit_items_SessionId",
                table: "inventory_audit_items");

            migrationBuilder.AddColumn<int>(
                name: "AuditSessionId",
                table: "inventory_audit_items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_audit_items_AuditSessionId",
                table: "inventory_audit_items",
                column: "AuditSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_audit_items_audit_sessions_AuditSessionId",
                table: "inventory_audit_items",
                column: "AuditSessionId",
                principalTable: "audit_sessions",
                principalColumn: "Id");
        }
    }
}
