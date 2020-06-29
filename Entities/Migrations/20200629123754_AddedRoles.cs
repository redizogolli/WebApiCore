using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "84e2c2c4-93bc-4a26-8a06-eb97340eda33", "99845231-1bb5-479b-8169-d821c4324605", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1eb35818-fe3d-4686-92c9-be676482cce6", "0e7b3167-b1b8-4d86-b3df-1458dce5c7ee", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "270a26d8-1793-4d86-89ff-2dc550754515", "42f87043-42b8-4eeb-89ff-936ec9021ff4", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1eb35818-fe3d-4686-92c9-be676482cce6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "270a26d8-1793-4d86-89ff-2dc550754515");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84e2c2c4-93bc-4a26-8a06-eb97340eda33");
        }
    }
}
