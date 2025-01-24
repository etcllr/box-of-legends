using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace box_of_legends.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Champion_ChampionId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ChampionId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ChampionId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LargeImage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SmallImage",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cart",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "ChampionId",
                table: "Product",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LargeImage",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SmallImage",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ChampionId",
                table: "Product",
                column: "ChampionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Champion_ChampionId",
                table: "Product",
                column: "ChampionId",
                principalTable: "Champion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
