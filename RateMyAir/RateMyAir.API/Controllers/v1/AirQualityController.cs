using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateMyAir.API.Extensions;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Exceptions;
using RateMyAir.Entities.Models;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces;
using System;
using System.Threading.Tasks;

namespace RateMyAir.API.Controllers.v1
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly IRepositoryManager _repoManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AirQualityController(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
        {
            _repoManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get the AirQuality by Id
        /// </summary>
        /// <param name="id">Air Quality Id</param>
        /// <returns>Response of type AirQualityDtoOut</returns>
        [AllowAnonymous]
        [HttpGet("{airQualityId}", Name = "GetAirQualityById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQualityById(int airQualityId)
        {
            var airQuality = await _repoManager.AirQuality.GetAirQualityByIdAsync(airQualityId, false);

            if (airQuality == null)
            {
                _logger.LogInfo($"AirQUality with id: {airQualityId} doesn't exist.");
                throw new NotFoundException();
            }

            var dtoOut = _mapper.Map<AirQualityDtoOut>(airQuality);
            return Ok(new Response<AirQualityDtoOut>(dtoOut));
        }

        /// <summary>
        /// Get the paginated list of AirQuality
        /// </summary>
        /// <param name="filter">GetAirQualityParameters filter parameters</param>
        /// <returns>PagedResponse of type AirQualityDtoOut</returns>
        [AllowAnonymous]
        [HttpGet(Name = "GetAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAirQuality([FromQuery] GetAirQualityParameters filter)
        {
            var airQuality = _repoManager.AirQuality.GetAirQuality(filter, false);
            return Ok(await this.PaginateSourceData<AirQuality, AirQualityDtoOut, DateTime>(airQuality, filter.PageNumber, filter.PageSize, x => x.CreatedDate, _mapper.ConfigurationProvider));
        }

        /// <summary>
        /// Add the AirQuality record in the database
        /// </summary>
        /// <param name="model">AirDataDtoIn</param>
        /// <returns>Response of type AirQualityDtoOut</returns>
        [AllowAnonymous]
        [HttpPost(Name = "AddAirQuality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddAirQuality([FromBody] AirQualityDtoIn model)
        {
            if (model == null)
            {
                _logger.LogError("AirQualityDtoIn object sent from client is null.");
                throw new BadRequestException("AirQualityDtoIn object is null");
            }

            var entity = _mapper.Map<AirQuality>(model);

            _repoManager.AirQuality.CreateAirData(entity);
            await _repoManager.SaveAsync();

            var dtoOut = _mapper.Map<AirQualityDtoOut>(entity);
            return CreatedAtRoute("GetAirQualityById", new { id = dtoOut.Id }, dtoOut);
        }

    }
}
