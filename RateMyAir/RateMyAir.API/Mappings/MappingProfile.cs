using AutoMapper;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Models;

namespace RateMyAir.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AirDataDtoIn, AirData>();
            CreateMap<AirData, AirDataDtoOut>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AirDataID))
                .ForMember(dest => dest.OutdoorTemp, opt => opt.MapFrom(src => src.Temperature1))
                .ForMember(dest => dest.IndoorTemp, opt => opt.MapFrom(src => src.Temperature0))
                .ForMember(dest => dest.IndoorHumidity, opt => opt.MapFrom(src => src.Humidity))
                .ForMember(dest => dest.IndoorPressure, opt => opt.MapFrom(src => src.Pressure))
                .ForMember(dest => dest.IndoorDewPoint, opt => opt.MapFrom(src => src.DewPoint))
                .ForMember(dest => dest.IndoorPm25, opt => opt.MapFrom(src => src.Pm25))
                .ForMember(dest => dest.IndoorPm10, opt => opt.MapFrom(src => src.Pm10));
        }
    }
}