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
        new Product { Id = 4, Name = "копчености", Price = 66 },
        new Product { Id = 5, Name = "сосиски", Price = 77 },
        new Product { Id = 6, Name = "хлеб", Price = 88 },
        new Product { Id = 7, Name = "лаваш", Price = 99 },
        new Product { Id = 1, Name = "зелень", Price = 37 },
        new Product { Id = 2, Name = "овощи ", Price = 41 },
        new Product { Id = 3, Name = "фрукты", Price = 55 },
        new Product { Id = 8, Name = "сыр", Price = 11 },
        new Product { Id = 9, Name = "кетчуп", Price = 22 }
    );

    modelBuilder.Entity<BuyerCart>().HasData(
        new BuyerCart { Id = 1, BuyerId = 1, ProductId = 3 },
        new BuyerCart { Id = 2, BuyerId = 1, ProductId = 9 },

        new BuyerCart { Id = 3, BuyerId = 2, ProductId = 6 },

        new BuyerCart { Id = 4, BuyerId = 4, ProductId = 1 },
        new BuyerCart { Id = 5, BuyerId = 4, ProductId = 2, ProductCount = 2 },
        new BuyerCart { Id = 6, BuyerId = 4, ProductId = 3 },
        new BuyerCart { Id = 7, BuyerId = 4, ProductId = 4 },
        new BuyerCart { Id = 8, BuyerId = 4, ProductId = 5 },
        new BuyerCart { Id = 9, BuyerId = 4, ProductId = 6 },
        new BuyerCart { Id = 10, BuyerId = 4, ProductId = 7 },
        new BuyerCart { Id = 11, BuyerId = 4, ProductId = 8 },
        new BuyerCart { Id = 12, BuyerId = 4, ProductId = 9 }
    );

    modelBuilder.Entity<Buyer>().HasData(
        new Buyer { Id = 1, Name = "Иванов", BuyerEmail = "ivanov@mair.ru" },
        new Buyer { Id = 2, Name = "Петров", BuyerEmail = "petrov@mair.ru" },
        new Buyer { Id = 3, Name = "Сидоров", BuyerEmail = "sidorov@mair.ru" },
        new Buyer { Id = 4, Name = "Азуров", BuyerEmail = "azurov@mair.ru" }
    );
    
  }

    public List<Buyer_Product> getBuyer_Products(int BuyerId){
        return BuyerCarts.Where(el => el.BuyerId == BuyerId).Join(
            Products,
            el => el.ProductId,
            pr => pr.Id,
            (el, pr) => new Buyer_Product{
                BuyerId = el.BuyerId,
                ProductCount = el.ProductCount,

                Name = pr.Name,
                Price = pr.Price,
                ProductId = el.ProductId, 

                Sum = el.ProductCount * pr.Price,
            }
        ).ToList<Buyer_Product>();
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

public class Buyer_Product {
    public int BuyerId { get; set; }
    public int ProductCount { get; set; }

    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int ProductId { get; set; }

    public decimal Sum { get; set; }
}
