using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeoIpApi.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public int Geoname_Id { get; set; }
        public string Capital { get; set; }
        public List<Language> Languages { get; set; }
        public string Country_flag { get; set; }
        public string Country_flag_emoji { get; set; }
        public string Country_flag_emoji_unicode { get; set; }
        public string Calling_code { get; set; }
        public bool Is_eu { get; set; }
    }
}