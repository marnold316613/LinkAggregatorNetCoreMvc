using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkAggregatorMVC.Migrations
{
    /// <inheritdoc />
    public partial class changecategorylookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkCategories_CategoriesLookup_CategoriesLookupID",
                table: "LinkCategories");

            migrationBuilder.DropIndex(
                name: "IX_LinkCategories_CategoriesLookupID",
                table: "LinkCategories");

            migrationBuilder.RenameColumn(
                name: "CategoriesLookupID",
                table: "LinkCategories",
                newName: "CategoriesLookup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoriesLookup",
                table: "LinkCategories",
                newName: "CategoriesLookupID");

            migrationBuilder.CreateIndex(
                name: "IX_LinkCategories_CategoriesLookupID",
                table: "LinkCategories",
                column: "CategoriesLookupID");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkCategories_CategoriesLookup_CategoriesLookupID",
                table: "LinkCategories",
                column: "CategoriesLookupID",
                principalTable: "CategoriesLookup",
                principalColumn: "ID");
        }
    }
}
