using Microsoft.EntityFrameworkCore;
using ShopCSharp_API.Models;

namespace ShopCSharp_API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
  }
}