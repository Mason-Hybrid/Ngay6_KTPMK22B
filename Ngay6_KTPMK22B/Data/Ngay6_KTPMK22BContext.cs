using Microsoft.EntityFrameworkCore;
using Ngay6_KTPMK22B.Models;

public class Ngay6_KTPMK22BContext(DbContextOptions<Ngay6_KTPMK22BContext> options) : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Genre> Genre { get; set; } = default!;
    public DbSet<Movie> Movie { get; set; } = default!;
}
