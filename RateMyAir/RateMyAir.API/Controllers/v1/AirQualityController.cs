using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RateMyAir.API.Attributes;
using RateMyAir.API.Extensions;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Exceptions;
using RateMyAir.Entities.Models;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using System;
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
        private readonly IRepositoryManager _repoManager;
        private readonly IPollutionService _pollutionService;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AirQualityController(
            IRepositoryManager repositoryManager,
            IPollutionService pollutionService,
            IMapper mapper,
            ILoggerService logger)
        {
            _repoManager = repositoryManager;
            _pollutionService = pollutionService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get the AirQuality by Id
        /// </summary>
        /// <param name="id">Air Quality Id</param>
        /// <returns>Response of type AirQualityDtoOut</returns>
        [HttpGet("airquality/{airQualityId}", Name = "GetAirQualityById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQualityById(int airQualityId)
        {
            var airQuality = await _repoManager.AirQuality.GetAirQualityByIdAsync(airQualityId, false);

            if (airQuality == null)
            {
                _logger.LogInfo($"AirQuality with id: {airQualityId} doesn't exist.");
                throw new NotFoundException();
            }

            var dtoOut = _mapper.Map<AirQualityDtoOut>(airQuality);
            return Ok(new Response<AirQualityDtoOut>(dtoOut));
        }

        /// <summary>
        /// Get the most recent AirQuality
        /// </summary>
        /// <returns>Response of type AirQualityDtoOut</returns>
        [HttpGet("airquality/last", Name = "GetLastAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLastAirQuality()
        {
            var last = await _repoManager.AirQuality.GetLastAsync(false);
            return Ok(new Response<AirQualityDtoOut>(_mapper.Map<AirQualityDtoOut>(last)));
        }

        /// <summary>
        /// Get the list of Air Quality indexes
        /// </summary>
        /// <param name="filter">GetAirQualityParameters filter</param>
        /// <returns></returns>
        [HttpGet("airquality/index", Name = "GetAirQualityIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQualityIndex([FromQuery] GetAirQualityIndexParameters filter)
        {
            if(filter.ValidDateRange == false)
            {
                _logger.LogError($"GetAirQualityIndex: FromDate {filter.FromDate} can't be less than ToDate {filter.ToDate}.");
                throw new BadRequestException("FromDate can't be less than ToDate");
            }

            var pollutionData = await _pollutionService.GetAirQualityIndexAsync(filter.FromDate, filter.ToDate);
            return Ok(new Response<List<AirQualityIndexDtoOut>>(pollutionData));
        }

        /// <summary>
        /// Get the paginated list of AirQuality
        /// </summary>
        /// <param name="filter">GetAirQualityParameters filter</param>
        /// <returns>PagedResponse of type AirQualityDtoOut</returns>
        [HttpGet("airquality", Name = "GetAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQuality([FromQuery] GetAirQualityParameters filter)
        {
            if (filter.ValidDateRange == false)
            {
                _logger.LogError($"GetAirQualityIndex: FromDate {filter.FromDate} can't be less than ToDate {filter.ToDate}.");
                throw new BadRequestException("FromDate can't be less than ToDate");
            }

            var airQuality = _repoManager.AirQuality.GetAirQuality(filter.FromDate, filter.ToDate, false);
            return Ok(await this.PaginateSourceData<AirQuality, AirQualityDtoOut, DateTime>(airQuality, filter.PageNumber, filter.PageSize, x => x.CreatedAt, _mapper.ConfigurationProvider));
        }

        /// <summary>
        /// Add the AirQuality record in the database
        /// </summary>
        /// <param name="model">AirDataDtoIn</param>
        /// <returns>Response of type AirQualityDtoOut</returns>
        [HttpPost("airquality", Name = "AddAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddAirQuality([FromBody] AirQualityDtoIn model)
        {
            if (model == null)
            {
                _logger.LogError("AirQualityDtoIn object sent from client is null.");
                throw new BadRequestException("AirQualityDtoIn object is null");
            }

            var entity = _mapper.Map<AirQuality>(model);
            entity.CreatedAt = DateTime.UtcNow;
            _repoManager.AirQuality.CreateAirQuality(entity);
            await _repoManager.SaveAsync();

            var dtoOut = _mapper.Map<AirQualityDtoOut>(entity);
            return CreatedAtRoute("GetAirQualityById", new { airQualityId = dtoOut.Id }, dtoOut);
        }

    }
}
