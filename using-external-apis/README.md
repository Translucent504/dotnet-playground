# Using external Apis

The idea is to understand how to use third party apis. Like in python I can use `requests` or in javascript I can use `fetch` or `axios` or some client library of the api provider.

## Entity Framework

### Prerequisites

- `dotnet tool update --global dotnet-ef --version 3.1.12`
- Have some database server running in the background. I'm using Postgres inside docker on localhost.
- Add the Nuget Packages to `.csproj`
  ```
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="3.1.11"/>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="3.1.12"/>
  ```
- Add the Model classes.
- Create DbContext where Tables are represented as DbSets by using the Model Classes.

  ```csharp
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
  ```

- Register the DbContext with Dependency Injection inside `Startup.cs`
  ```csharp
    services.AddDbContext<CookbookContext>(options =>
        options.
        UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())) // prints sql queries to console.
        .UseNpgsql(Configuration.GetConnectionString("CookbookContext")));
  ```
- Add the ConnectionString to the `appsettings.Development.json`
  ```json
  {
    "ConnectionStrings": {
      "CookbookContext": "host=localhost;database=postgres;user id=postgres;password=docker;"
    }
  }
  ```
- Use the DbContext inside whatever controllers etc.
- Actually generate the code to create the tables by using `dotnet ef migrations add initial`.
  - Can also use `dotnet ef migrations script` to generate the SQL script version.
- Write the migration to the database with `dotnet ef database update`.
