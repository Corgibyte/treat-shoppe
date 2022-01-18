using Microsoft.EntityFrameworkCore.Migrations;

namespace TreatShoppe.Migrations
{
    public partial class AddDatabaseSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "22e5205f-9162-400e-8421-b05e265d248d", "e8d08c40-712b-43f3-95c3-326b6195e409", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4735b809-a800-4ed1-80fc-f4a54737485f", "a7ad361d-2d85-4d48-b692-6a4eaf6602a9", "user", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22e5205f-9162-400e-8421-b05e265d248d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4735b809-a800-4ed1-80fc-f4a54737485f");
        }
    }
}
