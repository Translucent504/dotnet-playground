using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;


using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Services;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace using_external_apis.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly CookbookContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, CookbookContext DataContext)
    {
      _logger = logger;
      _context = DataContext;
    }

    [HttpGet]
    public async Task<List<Dish>> GetAll()
    {
      return await _context.Dishes.ToListAsync();
    }

    [HttpGet("gmail")]
    public async Task<Profile> GetGmail()
    {
      var service = new DiscoveryService(new BaseClientService.Initializer
      {
        ApplicationName = "IDK lol",
        ApiKey = "AIzaSyAafDI3EmFtj3W3VskJILLum_IJ16RZZ24"
      });

      var gmailService = new GmailService(new BaseClientService.Initializer
      {
        ApplicationName = "my Gmail Service",
        ApiKey = "AIzaSyAafDI3EmFtj3W3VskJILLum_IJ16RZZ24",
      });

      var response = await service.Apis.List().ExecuteAsync();
      var gmailResponse = await gmailService.Users.GetProfile("me").ExecuteAsync();
      if (response.Items != null)
      {
        foreach (DirectoryList.ItemsData api in response.Items)
        {
          Console.WriteLine(api.Id + " - " + api.Title);
        }
      }

      return gmailResponse;
    }
  }

  public class Dish
  {
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Notes { get; set; }
    public int? Stars { get; set; }
    public List<DishIngredient> Ingredients { get; set; } = new List<DishIngredient>();
  }

  public class DishIngredient
  {
    public int Id { get; set; }

    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string UnitOfMeasure { get; set; } = string.Empty;

    // [Column(TypeName = "decimal(5, 2)")]
    public decimal Amount { get; set; }

    public int DishId { get; set; }
  }

  public class CookbookContext : DbContext
  {
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishIngredient> Ingredients { get; set; }

#pragma warning disable CS8618
    public CookbookContext(DbContextOptions<CookbookContext> options)
    : base(options)
    {

    }
  }
}
