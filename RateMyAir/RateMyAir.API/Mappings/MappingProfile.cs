using AutoMapper;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Models;

namespace RateMyAir.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AirQualityDtoIn, AirQuality>();
            CreateMap<AirQuality, AirQualityDtoOut>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AirQualityId));
        }
    }
}