using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using box_of_legends.Components;
using box_of_legends.Components.Account;
using box_of_legends.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>();

builder.Services.AddRazorComponents()
   .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(options =>
   {
       options.DefaultScheme = IdentityConstants.ApplicationScheme;
       options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
   })
   .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
   .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddSignInManager()
   .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

app.MapControllerRoute(
   name: "product",
   pattern: "product/{id}-{name}",
   defaults: new { controller = "Products", action = "Details" }
);

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

app.MapBlazorHub();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();