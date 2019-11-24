using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoIpApi.Models.Dtos
{
    public class LanguageDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Native { get; set; }
    }
}