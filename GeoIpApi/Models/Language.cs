using System.ComponentModel.DataAnnotations;

namespace GeoIpApi.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Native { get; set; }
    }
}