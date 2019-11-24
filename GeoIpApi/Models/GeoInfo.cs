using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoIpApi.Models
{
    public class GeoInfo
    {
        [Key]
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        public string Continent_code { get; set; }
        public string Continent_name { get; set; }
        public string Country_name { get; set; }
        public string Region_code { get; set; }
        public string Region_name { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }       

    }
}