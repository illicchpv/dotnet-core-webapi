using Microsoft.EntityFrameworkCore;

public class InMemoryDbContext : DbContext
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Buyer> Buyers { get; set; }
  public DbSet<BuyerCart> BuyerCarts { get; set; }


  public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
  {
    Database.EnsureCreated();   // создаем базу данных при первом обращении
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Product>().HasData(
        new Product { Id = 1, Name = "зелень", Price = 37 },
        new Product { Id = 2, Name = "овощи ", Price = 41 },
        new Product { Id = 3, Name = "фрукты", Price = 55 },
        new Product { Id = 4, Name = "копчености", Price = 66 },
        new Product { Id = 5, Name = "сосиски", Price = 77 },
        new Product { Id = 6, Name = "хлеб", Price = 88 },
        new Product { Id = 7, Name = "лаваш", Price = 99 },
        new Product { Id = 8, Name = "сыр", Price = 11 },
        new Product { Id = 9, Name = "кетчуп", Price = 22 }
    );


  }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = -1;
}


public class Buyer{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string BuyerEmail { get; set; } = "";
}

public class BuyerCart{
    public int Id { get; set; }
    public int BuyerId { get; set; }
    public int ProductId { get; set; }
    public int ProductCount { get; set; } = 1;
}


