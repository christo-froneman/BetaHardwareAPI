using Microsoft.OpenApi.Models;
using BetaHardwareAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Products") ?? "Data Source=Products.db";

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// In-memory Db
// builder.Services.AddDbContext<ProductDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<ProductDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "BetaHardware API",
         Description = "Selling the products you need",
         Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
//if (app.Environment.IsDevelopment())
//{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetaHardware API V1");
    });    
//}

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/products", async (ProductDb db) => await db.Products.ToListAsync());

app.MapPost("/product", async (ProductDb db, Product product) =>
{
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Created($"/product/{product.Id}", product);
});

app.MapGet("/product/{id}", async (ProductDb db, int id) => await db.Products.FindAsync(id));

app.MapPut("/product/{id}", async (ProductDb db, Product updateproduct, int id) =>
{
      var product = await db.Products.FindAsync(id);
      if (product is null) return Results.NotFound();
      product.Name = updateproduct.Name;
      product.Code = updateproduct.Code;
      product.ReleaseDate = updateproduct.ReleaseDate;
      product.Price = updateproduct.Price;
      product.Description = updateproduct.Description;
      product.StarRating = updateproduct.StarRating;
      product.ImageUrl = updateproduct.ImageUrl;
      await db.SaveChangesAsync();
      return Results.NoContent();
});

app.MapDelete("/product/{id}", async (ProductDb db, int id) =>
{
   var product = await db.Products.FindAsync(id);
   if (product is null)
   {
      return Results.NotFound();
   }
   db.Products.Remove(product);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
