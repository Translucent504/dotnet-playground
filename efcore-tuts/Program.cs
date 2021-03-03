// using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Console;

var factory = new CookbookContextFactory();
using var context = factory.CreateDbContext();
var a = context.Dishes.Count();
System.Console.WriteLine(a);
System.Console.WriteLine("Lets add something");
context.Dishes.Add(new Dish { Title = "my Dish", Notes = "This is good dish", Stars = 4 });
await context.SaveChangesAsync();
System.Console.WriteLine(context.Dishes.Count());
class Dish
{
  public int Id { get; set; }

  [MaxLength(100)]
  public string Title { get; set; } = string.Empty;

  [MaxLength(1000)]
  public string? Notes { get; set; }

  public int? Stars { get; set; }

  public List<DishIngredient> Ingredients { get; set; } = new();
}

class DishIngredient
{
  public int Id { get; set; }

  [MaxLength(100)]
  public string Description { get; set; } = string.Empty;

  [MaxLength(50)]
  public string UnitOfMeasure { get; set; } = string.Empty;

  [Column(TypeName = "decimal(5, 2)")]
  public decimal Amount { get; set; }

  public Dish? Dish { get; set; }

  public int? DishId { get; set; }
}

class CookbookContext : DbContext
{
  public DbSet<Dish> Dishes { get; set; }

  public DbSet<DishIngredient> Ingredients { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  public CookbookContext(DbContextOptions<CookbookContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

class CookbookContextFactory : IDesignTimeDbContextFactory<CookbookContext>
{
  public CookbookContext CreateDbContext(string[]? args = null)
  {
    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    var optionsBuilder = new DbContextOptionsBuilder<CookbookContext>();
    optionsBuilder
        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
        .UseNpgsql<CookbookContext>(configuration["ConnectionStrings:DefaultConnection"]);

    return new CookbookContext(optionsBuilder.Options);
  }
}
