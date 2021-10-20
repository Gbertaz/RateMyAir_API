using AutoMapper;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.DTO.Queries;
using RateMyAir.Entities.Models;

namespace RateMyAir.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AirQuality, PollutionQueryDto>();
            CreateMap<AirQualityDtoIn, AirQuality>();
            CreateMap<AirQuality, AirQualityDtoOut>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AirQualityId));
        }
    }
}