using Microsoft.AspNetCore.Mvc;
using RateMyAir.API.Attributes;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Exceptions;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.API.Controllers.v1
{
    [ApiKey]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityService _airQualityService;
        private readonly ILoggerService _logger;

        public AirQualityController(IAirQualityService airQualityService, ILoggerService logger)
        {
            _airQualityService = airQualityService;
            _logger = logger;
        }

        [HttpGet("airquality/{airQualityId}", Name = "GetAirQualityById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQualityById(int airQualityId)
        {
            var dtoOut = await _airQualityService.GetAirQualityByIdAsync(airQualityId, false);
            if (dtoOut == null)
            {
                _logger.LogInfo($"AirQuality with id: {airQualityId} doesn't exist.");
                throw new NotFoundException();
            }
            return Ok(new Response<AirQualityDtoOut>(dtoOut));
        }

        [HttpGet("airquality/last", Name = "GetLastAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLastAirQuality()
        {
            var last = await _airQualityService.GetLastAirQualityAsync(false);
            return Ok(new Response<AirQualityDtoOut>(last));
        }

        [HttpGet("airquality/dailyindex", Name = "GetDailyAirQualityIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDailyAirQualityIndex([FromQuery] GetAirQualityParameters filter)
        {
            if(filter.ValidDateRange == false)
            {
                _logger.LogError($"GetDailyAirQualityIndex: FromDate {filter.FromDate} can't be less than ToDate {filter.ToDate}.");
                throw new BadRequestException("FromDate can't be less than ToDate");
            }

            var airQualityIndex = await _airQualityService.GetDailyAirQualityIndexAsync(filter);
            return Ok(new Response<List<AirQualityIndexDtoOut>>(airQualityIndex));
        }

        [HttpGet("airquality", Name = "GetAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQuality([FromQuery] GetAirQualityParameters filter)
        {
            if (filter.ValidDateRange == false)
            {
                _logger.LogError($"GetAirQualityIndex: FromDate {filter.FromDate} can't be less than ToDate {filter.ToDate}.");
                throw new BadRequestException("FromDate can't be less than ToDate");
            }

            var pagedAirQuality = await _airQualityService.GetPagedAirQualityAsync(filter);
            int totalRecords = await _airQualityService.CountAirQualityAsync(filter);
            return Ok(new PagedResponse<List<AirQualityDtoOut>>(pagedAirQuality, filter.PageNumber, filter.PageSize, totalRecords));
        }

        [HttpPost("airquality", Name = "InsertAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> InsertAirQuality([FromBody] AirQualityDtoIn model)
        {
            if (model == null)
            {
                _logger.LogError("AirQualityDtoIn object sent from client is null.");
                throw new BadRequestException("AirQualityDtoIn object is null");
            }

            var dtoOut = await _airQualityService.InsertAirQualityAsync(model);
            return CreatedAtRoute("GetAirQualityById", new { airQualityId = dtoOut.Id }, dtoOut);
        }

    }
}
