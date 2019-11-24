using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoIpApi.Models.Dtos
{
    public class GeoInfoDto
    {
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
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public LocationDto LocationDto { get; set; }

    }
}