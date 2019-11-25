using GeoIpApi.Models;
using System.Data.Entity;
namespace GeoIpApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() :
          base("DbConnectionString")
        {
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<GeoInfo> GeoInfos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}