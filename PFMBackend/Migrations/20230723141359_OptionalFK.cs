using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFMBackend.Migrations
{
    public partial class OptionalFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions");

            migrationBuilder.AlterColumn<string>(
                name: "Catcode",
                table: "transactions",
                type: "character varying(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions");

            migrationBuilder.AlterColumn<string>(
                name: "Catcode",
                table: "transactions",
                type: "character varying(32)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
