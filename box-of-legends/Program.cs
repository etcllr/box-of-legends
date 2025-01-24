using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using box_of_legends.Components;
using box_of_legends.Components.Account;
using box_of_legends.Data;

// Déclaration de la classe Program en public
public class Program
{
    // Point d'entrée principal de l'application
    public static async Task Main(string[] args) // Notez le changement en async Task
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        // Initialisation de la base de données
        await InitializeDatabase(app);

        ConfigureApp(app);

        await app.RunAsync(); // Utilisation de RunAsync pour la cohérence
    }

    // Méthode pour organiser la configuration des services
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services
            .AddScoped<AuthenticationStateProvider,
                IdentityRevalidatingAuthenticationStateProvider>();
        builder.Services.AddScoped<DataDragonService>();


        // Configuration de l'authentification
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        // Configuration de la base de données
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString), ServiceLifetime.Scoped);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Configuration Identity
        builder.Services.AddIdentityCore<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
    }

    private static async Task InitializeDatabase(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                await context.Database.EnsureCreatedAsync(); // Version asynchrone
                await DbInitializer.Initialize(context,
                    services.GetRequiredService<DataDragonService>(), logger);
            }
            catch (Exception ex)
            {
                // Récupération du logger pour les erreurs d'initialisation
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex,
                    "Une erreur s'est produite lors de l'initialisation de la base de données.");
                throw; // Relance l'exception pour arrêter l'application en cas d'échec critique
            }
        }
    }

    // Méthode pour organiser la configuration du pipeline HTTP
    private static void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapAdditionalIdentityEndpoints();
    }
}