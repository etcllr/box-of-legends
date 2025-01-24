using box_of_legends.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, DataDragonService dataService, ILogger<Program> logger)
    {
        if (!context.Product.Any())
        {
            try
            {
                // Récupération de toutes les données
                var (categories, champions, products) = await dataService.GetChampionSkins();
                logger.LogInformation($"Récupération de {categories.Count} catégories, {champions.Count} champions et {products.Count} skins");

                // Ajout des catégories d'abord
                context.Category.AddRange(categories);
                await context.SaveChangesAsync();
                logger.LogInformation("Catégories ajoutées avec succès");

                // Puis ajout des champions
                context.Champion.AddRange(champions);
                await context.SaveChangesAsync();
                logger.LogInformation("Champions ajoutés avec succès");

                // Enfin, ajout des produits
                context.Product.AddRange(products);
                await context.SaveChangesAsync();
                logger.LogInformation("Produits ajoutés avec succès");
                
                logger.LogInformation("Base de données initialisée avec succès");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de l'initialisation de la base de données");
                throw;
            }
        }
    }
}