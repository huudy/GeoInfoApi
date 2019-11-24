using AutoMapper;
using GeoIpApi.Models;
using GeoIpApi.Models.Dtos;

namespace GeoIpApi.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<Location, LocationDto>().ForMember(dto => dto.LanguagesDto, gi => gi.MapFrom(src => src.Languages));
            CreateMap<GeoInfo, GeoInfoDto>().ForMember(dto => dto.LocationDto, gi => gi.MapFrom(src => src.Location));
        }
    }
}