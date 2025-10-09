using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManager.Migrations
{
    /// <inheritdoc />
    public partial class AddColunmsInBillsAndOcorrencesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "bills",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "observation",
                table: "bill_ocorrences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "value_cents",
                table: "bill_ocorrences",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE bill_ocorrences AS bo
                SET value_cents = b.value_cents
                FROM bills AS b
                WHERE bo.bill_id = b.id;"
            );

            migrationBuilder.AlterColumn<long>(
                name: "value_cents",
                table: "bill_ocorrences",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "bills");

            migrationBuilder.DropColumn(
                name: "observation",
                table: "bill_ocorrences");

            migrationBuilder.DropColumn(
                name: "value_cents",
                table: "bill_ocorrences");
        }
    }
}
