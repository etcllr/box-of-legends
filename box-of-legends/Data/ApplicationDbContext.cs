using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace box_of_legends.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
   
    public DbSet<box_of_legends.Models.Cart> Cart { get; set; } = default!;
    public DbSet<box_of_legends.Models.CartLine> CartLine { get; set; } = default!;
    public DbSet<box_of_legends.Models.Product> Product { get; set; } = default!;
    public DbSet<box_of_legends.Models.Champion> Champion { get; set; } = default!;
    public DbSet<box_of_legends.Models.Category> Category { get; set; } = default!;
}