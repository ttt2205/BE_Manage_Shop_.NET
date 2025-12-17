// using Microsoft.EntityFrameworkCore.Migrations;

// #nullable disable

// namespace Manage_Store.Migrations
// {
//     /// <inheritdoc />
//     public partial class AddForeignKeyToCustomer : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.AddColumn<int>(
//                 name: "user_id",
//                 table: "customers",
//                 type: "int",
//                 nullable: false,
//                 defaultValue: 0);

//             migrationBuilder.CreateIndex(
//                 name: "IX_customers_user_id",
//                 table: "customers",
//                 column: "user_id",
//                 unique: true);

//             migrationBuilder.AddForeignKey(
//                 name: "FK_customers_users_user_id",
//                 table: "customers",
//                 column: "user_id",
//                 principalTable: "users",
//                 principalColumn: "id",
//                 onDelete: ReferentialAction.Cascade);
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropForeignKey(
//                 name: "FK_customers_users_user_id",
//                 table: "customers");

//             migrationBuilder.DropIndex(
//                 name: "IX_customers_user_id",
//                 table: "customers");

//             migrationBuilder.DropColumn(
//                 name: "user_id",
//                 table: "customers");
//         }
//     }
// }
