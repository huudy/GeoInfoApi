using System.Collections.Generic;

namespace GeoIpApi.Models.Dtos
{
    public class LocationDto
    {
        public int Geoname_Id { get; set; }
        public string Capital { get; set; }
        public List<LanguageDto> LanguagesDto { get; set; }
        public string Country_flag { get; set; }
        public string Country_flag_emoji { get; set; }
        public string Country_flag_emoji_unicode { get; set; }
        public string Calling_code { get; set; }
        public bool Is_eu { get; set; }
    }
}