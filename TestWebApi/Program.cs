using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging:
using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog(logger);
Log.Logger = logger;

Log.Fatal("6 Fatal products");
Log.Error("5 Error");
Log.Warning("4 Warning");
Log.Information("3 Information");
Log.Debug("2 Debug");
Log.Verbose("1 Verbose");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<InMemoryDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb")
);

builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(ProductValidator));

builder.Services.AddRazorPages();

var app = builder.Build();

// if (!app.Environment.IsDevelopment())
{
    // builder.WebHost.UseUrls("http://*:80", "https://*.443");
    // builder.WebHost.UseUrls("http://*:5022");
    builder.WebHost.UseUrls("http://localhost:5022");
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ---------------UseDefaultFiles-------------------------------------------
{
    DefaultFilesOptions options = new DefaultFilesOptions();
    options.DefaultFileNames.Clear();
    options.DefaultFileNames.Add("index.html");
    app.UseDefaultFiles(options);
}

// ---------------UseStaticFiles-------------------------------------------
var cacheMaxAgeSec = 60 * 60 * 24 * 7; // 60s * 60m * 24h * 7d - неделя
cacheMaxAgeSec = 1;
app.UseStaticFiles(new StaticFileOptions{
    // FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "www_root")),
    // RequestPath = "",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
                "Cache-Control", $"public, max-age={cacheMaxAgeSec}");
    },
});
// ----------------------------------------------------------

// app.UseAuthorization();
// app.UseAuthentication();
app.UseRouting();
app.MapRazorPages();

app.AddEndPointsProduct();

app.Run();
