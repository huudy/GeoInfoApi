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

        public DbSet<GeoInfo> GeoInfos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Language> Languages { get; set; }

        public void MarkAsModified(GeoInfo geoInfo)
        {
            Entry(geoInfo).State = EntityState.Modified;
        }

    }
}