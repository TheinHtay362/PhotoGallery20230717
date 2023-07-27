using Microsoft.EntityFrameworkCore;
using PhotoGallery20230717.Models;

namespace PhotoGallery20230717.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<PhotoModel> Photos { get; set; }
    }
}
