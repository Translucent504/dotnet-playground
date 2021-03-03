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

// exp 1

var newDish = new Dish { Title = "foo", Notes = "bar" };
context.Dishes.Add(newDish);
await context.SaveChangesAsync();
newDish.Notes = "Baz";
await context.SaveChangesAsync();

await EntityStates(factory);
await ChangeTracking(factory);
static async Task EntityStates(CookbookContextFactory factory)
{
  using var context = factory.CreateDbContext();

  var newDish = new Dish { Title = "title", Notes = "notes" };
  var state = context.Entry(newDish).State;

  context.Dishes.Add(newDish);
  state = context.Entry(newDish).State;

}

static async Task ChangeTracking(CookbookContextFactory factory)
{
  using var dbContext = factory.CreateDbContext();

  var newDish = new Dish { Title = "foo", Notes = "bar" };
  dbContext.Dishes.Add(newDish);
  await dbContext.SaveChangesAsync();
  newDish.Notes = "Baz";

  var entry = dbContext.Entry(newDish);
  var originalValue = entry.OriginalValues[nameof(Dish.Notes)].ToString(); // original value is the value that entityframework knows is in the database. It compares the values of the object with the original value to see if it has changed compared to the database. It


}

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
