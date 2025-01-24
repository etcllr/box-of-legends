using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace box_of_legends.Migrations
{
    /// <inheritdoc />
    public partial class SeedMageChampionsAndSkins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertion de la catégorie "Mage"
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Mage" }
            );

            // Insertion des champions de la catégorie "Mage"
            migrationBuilder.InsertData(
                table: "Champion",
                columns: new[] { "Id", "Name", "CategoryId" },
                values: new object[,]
                {
                    { 1, "Ahri", 1 },
                    { 2, "Lux", 1 },
                    { 3, "Syndra", 1 },
                    { 4, "Veigar", 1 },
                    { 5, "Ziggs", 1 }
                }
            );

            // Insertion des skins pour chaque champion
            int skinId = 1;
            for (int championId = 1; championId <= 5; championId++)
            {
                for (int skinIndex = 1; skinIndex <= 30; skinIndex++)
                {
                    migrationBuilder.InsertData(
                        table: "Product",
                        columns: new[] { "Id", "Name", "Description", "Price", "SmallImage", "LargeImage", "ChampionId" },
                        values: new object[]
                        {
                            skinId++, // ID unique pour chaque skin
                            $"Champion {championId} Skin {skinIndex}",
                            $"Description for Champion {championId} Skin {skinIndex}",
                            skinIndex % 4 switch
                            {
                                0 => 975.0,
                                1 => 1350.0,
                                2 => 1820.0,
                                _ => 3250.0
                            }, // Prix cycliques
                            $"https://ddragon.leagueoflegends.com/cdn/img/champion/splash/Champion{championId}_{skinIndex}.jpg",
                            $"https://ddragon.leagueoflegends.com/cdn/img/champion/splash/Champion{championId}_{skinIndex}.jpg",
                            championId
                        }
                    );
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Suppression des skins
            for (int championId = 1; championId <= 5; championId++)
            {
                for (int skinIndex = 1; skinIndex <= 30; skinIndex++)
                {
                    int skinId = (championId - 1) * 30 + skinIndex;
                    migrationBuilder.DeleteData(
                        table: "Product",
                        keyColumn: "Id",
                        keyValue: skinId
                    );
                }
            }

            // Suppression des champions
            for (int championId = 1; championId <= 5; championId++)
            {
                migrationBuilder.DeleteData(
                    table: "Champion",
                    keyColumn: "Id",
                    keyValue: championId
                );
            }

            // Suppression de la catégorie "Mage"
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1
            );
        }
    }
}
