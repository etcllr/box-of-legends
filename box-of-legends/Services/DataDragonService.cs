using System.Text.Json;
using box_of_legends.Models;

public class DataDragonService
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://ddragon.leagueoflegends.com/cdn";
    private readonly string _version = "15.2.1";

    // Les catégories prédéfinies de League of Legends
    private readonly Dictionary<string, Category> _categories = new()
    {
        { "Assassin", new Category { Name = "Assassin" } },
        { "Fighter", new Category { Name = "Fighter" } },
        { "Mage", new Category { Name = "Mage" } },
        { "Marksman", new Category { Name = "Marksman" } },
        { "Support", new Category { Name = "Support" } },
        { "Tank", new Category { Name = "Tank" } }
    };

    public DataDragonService()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        _client = new HttpClient(handler);
    }

    // On retourne maintenant un tuple avec les trois collections : catégories, champions et produits
    public async Task<(List<Category> Categories, List<Champion> Champions, List<Product> Products)> GetChampionSkins()
    {
        try
        {
            var championsDict = new Dictionary<string, Champion>();
            var products = new List<Product>();
            
            var championsResponse = await _client.GetAsync($"{BaseUrl}/{_version}/data/en_US/champion.json");
            championsResponse.EnsureSuccessStatusCode();
            var championsJson = await championsResponse.Content.ReadAsStringAsync();
            
            var champions = JsonSerializer.Deserialize<ChampionListResponse>(championsJson, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (champions?.Data == null)
            {
                throw new Exception("Impossible de récupérer les données des champions");
            }

            Console.WriteLine($"Nombre de champions trouvés : {champions.Data.Count}");

            foreach (var champion in champions.Data.Values)
            {
                try
                {
                    // On assigne une catégorie existante au champion
                    // Pour cet exemple, on utilise "Mage" par défaut, mais vous pouvez adapter selon les données de l'API
                    if (!championsDict.ContainsKey(champion.Id))
                    {
                        championsDict[champion.Id] = new Champion 
                        {
                            Name = champion.Name,
                            Category = _categories["Mage"] // On utilise la référence à la catégorie existante
                        };
                    }

                    var detailedChampResponse = await _client.GetAsync($"{BaseUrl}/{_version}/data/en_US/champion/{champion.Id}.json");
                    detailedChampResponse.EnsureSuccessStatusCode();
                    var detailedChampJson = await detailedChampResponse.Content.ReadAsStringAsync();

                    var detailedChamp = JsonSerializer.Deserialize<ChampionDetailResponse>(detailedChampJson, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (detailedChamp?.Data == null || !detailedChamp.Data.ContainsKey(champion.Id))
                    {
                        continue;
                    }

                    foreach (var skin in detailedChamp.Data[champion.Id].Skins)
                    {
                        if (skin.Num == 0) continue;

                        products.Add(new Product
                        {
                            Name = skin.Name,
                            Description = champion.Lore ?? "Description non disponible",
                            Price = 975.0,
                            SmallImage = $"{BaseUrl}/img/champion/{champion.Id}.png",
                            LargeImage = $"{BaseUrl}/img/champion/splash/{champion.Id}_{skin.Num}.jpg",
                            Champion = championsDict[champion.Id]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du traitement du champion {champion.Id}: {ex.Message}");
                }
            }

            return (_categories.Values.ToList(), championsDict.Values.ToList(), products);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur globale dans GetChampionSkins: {ex.Message}");
            throw;
        }
    }
}