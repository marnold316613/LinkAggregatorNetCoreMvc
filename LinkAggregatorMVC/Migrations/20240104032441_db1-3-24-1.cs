using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkAggregatorMVC.Migrations
{
    /// <inheritdoc />
    public partial class db13241 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CategoriesLookup",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "CategoriesLookup");
        }
    }
}
