using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

public static class EndPointsProduct
{

    public static void AddEndPointsProduct(this WebApplication app)
    {
        app.MapGet("/products", (InMemoryDbContext context) =>
        {
            Log.Fatal("------products");
            Log.Fatal("6 Fatal products");
            Log.Error("5 Error");
            Log.Warning("4 Warning");
            Log.Information("3 Information");
            Log.Debug("2 Debug");
            Log.Verbose("1 Verbose");

            return Results.Ok(context.Products.ToList()); // context.Products.ToList();
        })
        .WithName("GetProducts")
        .WithOpenApi();

        app.MapGet("/product/{id}", (int id, InMemoryDbContext context) =>
        {
            var product = context.Products.FirstOrDefault(x => x.Id == id);
            return product != null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProduct")
        .WithOpenApi();

        app.MapPost("/product", async (IValidator<Product> validator, [FromBody] Product _product, InMemoryDbContext context) =>
        {
            var product = context.Products.FirstOrDefault(x => x.Id == _product.Id);
            if (product != null)
            {
                if (_product.Name != "") product.Name = _product.Name;
                if (_product.Price > 0) product.Price = _product.Price;
                if (_product.BuyerId != null) product.BuyerId = _product.BuyerId;
                if (_product.BuyerEmail != null) product.BuyerEmail = _product.BuyerEmail;

                var validattion = await validator.ValidateAsync(product);
                if (!validattion.IsValid)
                {
                    return Results.ValidationProblem(validattion.ToDictionary());
                }

                context.SaveChanges();
                return Results.Ok(product);
            }
            else
            {
                context.Products.Add(_product);

                var validattion = await validator.ValidateAsync(_product);
                if (!validattion.IsValid)
                {
                    return Results.ValidationProblem(validattion.ToDictionary());
                }

                context.SaveChanges();
                return Results.Ok(_product);
            }
        })
        .WithName("AddEditProduct")
        .WithOpenApi();
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = -1;

    public int? BuyerId { get; set; }
    public string? BuyerEmail { get; set; }
}

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(4).MaximumLength(8); // .WithMessage("Длина имени должна быть от 4 до 8");
        RuleFor(x => x.Name).Must(nam => Regex.IsMatch(nam, "^[A-ZА-Я][a-zа-я]+$")); //.WithMessage("Первая заглавная остальные маленькие");
        RuleFor(x => x.Price).GreaterThan(10); //.WithMessage("Только за деньги(>10) Да ");
        RuleFor(x => x.BuyerEmail).EmailAddress(); //.WithMessage("Некорректная почта");
    }
}