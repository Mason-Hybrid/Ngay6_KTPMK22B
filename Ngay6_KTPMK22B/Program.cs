using Microsoft.EntityFrameworkCore;
using Ngay6_KTPMK22B.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Ngay6_KTPMK22BContext") ?? throw new InvalidOperationException("Connection string 'Ngay6_KTPMK22BContext' not found.");

builder.Services.AddDbContext<Ngay6_KTPMK22BContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Ngay6_KTPMK22BContext>();

    try
    {
        Console.WriteLine("Applying migrations...");
        // This will run all pending migrations, including seed data
        context.Database.Migrate();
        Console.WriteLine("Migrations applied successfully!");

        // Check if genres were seeded
        var genreCount = context.Genre.Count();
        Console.WriteLine($"Total genres in database: {genreCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
