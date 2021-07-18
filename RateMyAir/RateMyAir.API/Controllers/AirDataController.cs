using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Exceptions;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirDataController : ControllerBase
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AirDataController(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _repo = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get the AirData by Id
        /// </summary>
        /// <param name="id">Air Data Id</param>
        /// <returns>AirDataDtoOut</returns>
        [AllowAnonymous]
        [HttpGet("{id}", Name = "AirDataById")]
        public async Task<IActionResult> AirDataById(int id)
        {
            var data = await _repo.AirData.GetByIdAsync(id, false);

            if (data == null)
            {
                _logger.LogInfo($"AirData with id: {id} doesn't exist.");
                throw new NotFoundException();
            }

            var result = _mapper.Map<AirDataDtoOut>(data);
            return Ok(new Response<AirDataDtoOut>(result));
        }

        /// <summary>
        /// Get the last Air Data
        /// </summary>
        /// <returns>AirDataDtoOut</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("last")]
        public async Task<IActionResult> Get()
        {
            var lastData = await _repo.AirData.GetLastAsync();

            if (lastData == null)
            {
                _logger.LogInfo($"Last AirData doesn't exist in the database.");
                throw new NotFoundException();
            }

            var result = _mapper.Map<AirDataDtoOut>(lastData);
            return Ok(new Response<AirDataDtoOut>(result));
        }

        /// <summary>
        /// Get the Air Data list between two dates range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of AirDataDtoOut</returns>
        [AllowAnonymous]
        [HttpGet("{startDate}/{endDate}")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate)
        {
            var data = await _repo.AirData.GetRangeAsync(startDate, endDate, false);
            var result = _mapper.Map<IEnumerable<AirDataDtoOut>>(data);
            return Ok(new Response<IEnumerable<AirDataDtoOut>>(result));
        }

        /// <summary>
        /// Add the Air data into database
        /// </summary>
        /// <param name="model">AirDataDtoIn</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AirDataDtoIn model)
        {
            if (model == null)
            {
                _logger.LogError("AirDataDTOIn object sent from client is null.");
                throw new BadRequestException("AirDataDTOIn object is null");
            }

            var entity = _mapper.Map<AirData>(model);

            _repo.AirData.CreateAirData(entity);
            await _repo.SaveAsync();

            var returnObj = _mapper.Map<AirDataDtoOut>(entity);
            return CreatedAtRoute("AirDataById", new { id = returnObj.Id }, returnObj);
        }

    }
}
