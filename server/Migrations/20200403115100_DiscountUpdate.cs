using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class DiscountUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discounts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Type", "Value", "ValueType" },
                values: new object[,]
                {
                    { 1, "ExampleFlat", 2, 1 },
                    { 2, "ExamplePercentage", 5, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Discounts",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
