using Microsoft.EntityFrameworkCore;

namespace BetaHardwareAPI.Models 
{
    public class Product
    {
          public int Id { get; set; }
          public string? Name { get; set; }
          public string? Code { get; set; }
          public string? ReleaseDate { get; set; }
          public double? Price { get; set; }
          public string? Description { get; set; }
          public double? StarRating { get; set; }
          public string? ImageUrl { get; set; }
    }

    public class ProductDb : DbContext
    {
        public ProductDb(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; } = null!;
    }    
}