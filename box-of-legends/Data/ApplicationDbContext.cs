using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace box_of_legends.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<box_of_legends.Models.Cart> Cart { get; set; } = default!;
    public DbSet<box_of_legends.Models.CartLine> CartLine { get; set; } = default!;

    public DbSet<box_of_legends.Models.Product> Product { get; set; } = default!;

    public DbSet<box_of_legends.Models.Champion> Champion { get; set; } = default!;

    public DbSet<box_of_legends.Models.Category> Category { get; set; } = default!;
}