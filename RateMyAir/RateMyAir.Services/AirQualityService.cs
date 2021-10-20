using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.DTO.Queries;
using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Services
{
    public class AirQualityService : IAirQualityService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IAirQualityLevelsService _airQualityIndexService;
        private readonly IMapper _mapper;

        public AirQualityService(IRepositoryManager repoManager, IAirQualityLevelsService airQualityIndexService, IMapper mapper)
        {
            _repoManager = repoManager;
            _airQualityIndexService = airQualityIndexService;
            _mapper = mapper;
        }

        private IQueryable<AirQuality> FilterAirQuality(GetAirQualityParameters filter, bool trackChanges)
        {
            return _repoManager.AirQuality.GetAllAirQuality(trackChanges).OrderBy(x => x.CreatedAt)
                .Where(x => x.CreatedAt >= filter.FromDate && x.CreatedAt <= filter.ToDate);
        }

        public async Task<AirQualityDtoOut> GetAirQualityByIdAsync(int airQualityId, bool trackChanges)
        {
            var airQuality = await _repoManager.AirQuality.GetAirQualityByIdAsync(airQualityId, trackChanges);
            return _mapper.Map<AirQualityDtoOut>(airQuality);
        }

        public async Task<AirQualityDtoOut> GetLastAirQualityAsync(bool trackChanges)
        {
            var lastAirQuality = await _repoManager.AirQuality.GetLastAsync(trackChanges);
            return _mapper.Map<AirQualityDtoOut>(lastAirQuality);
        }

        public async Task<AirQualityDtoOut> InsertAirQualityAsync(AirQualityDtoIn model)
        {
            AirQuality entity = _mapper.Map<AirQuality>(model);
            entity.CreatedAt = DateTime.UtcNow;
            _repoManager.AirQuality.InsertAirQuality(entity);
            await _repoManager.SaveAsync();
            return _mapper.Map<AirQualityDtoOut>(entity);
        }

        public async Task<List<AirQualityDtoOut>> GetPagedAirQualityAsync(GetAirQualityParameters filter)
        {
            return await FilterAirQuality(filter, false)
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ProjectTo<AirQualityDtoOut>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<int> CountAirQualityAsync(GetAirQualityParameters filter)
        {
            return await FilterAirQuality(filter, false).CountAsync();
        }

        /// <summary>
        /// O(n) Time complexity where n is the number of pollutionData samples
        /// O(n) Space complexity
        /// </summary>
        /// <param name="GetAirQualityParameters">GetAirQualityParameters filter</param>
        /// <returns>List of AirQualityIndexDtoOut</returns>
        public async Task<List<AirQualityIndexDtoOut>> GetDailyAirQualityIndexAsync(GetAirQualityParameters filter)
        {
            List<PollutionQueryDto> pollutionData = await FilterAirQuality(filter, false)
                .ProjectTo<PollutionQueryDto>(_mapper.ConfigurationProvider).ToListAsync();

            List<AirQualityIndexDtoOut> airQualityIndexes = new List<AirQualityIndexDtoOut>();
            if (pollutionData.Count == 0) return airQualityIndexes;

            List<IndexLevel> pm25IndexLevels = await _airQualityIndexService.GetAirQualityLevelsAsync(Enums.Pollutants.Pm25);
            List<IndexLevel> pm10IndexLevels = await _airQualityIndexService.GetAirQualityLevelsAsync(Enums.Pollutants.Pm10);

            Dictionary<DateTime, AirQualityIndexDtoOut> dailyPollutionAverage = ComputeDailyPollutionAverage(pollutionData);

            foreach (KeyValuePair<DateTime, AirQualityIndexDtoOut> item in dailyPollutionAverage)
            {
                AirQualityIndexDtoOut dailyPollution = item.Value;
                IndexLevel pm25Level = IndexLevelBinarySearch(pm25IndexLevels, dailyPollution.Pm25Concentration);
                IndexLevel pm10Level = IndexLevelBinarySearch(pm10IndexLevels, dailyPollution.Pm10Concentration);
                dailyPollution.Pm25AirQualityIndex = pm25Level.AirQualityIndex;
                dailyPollution.Pm25AirQualityIndexDescription = pm25Level.Description;
                dailyPollution.Pm25Color = pm25Level.Color;
                dailyPollution.Pm10AirQualityIndex = pm10Level.AirQualityIndex;
                dailyPollution.Pm10AirQualityIndexDescription = pm10Level.Description;
                dailyPollution.Pm10Color = pm10Level.Color;
                airQualityIndexes.Add(dailyPollution);
            }
            return airQualityIndexes;
        }

        private Dictionary<DateTime, AirQualityIndexDtoOut> ComputeDailyPollutionAverage(List<PollutionQueryDto> pollutionData)
        {
            Dictionary<DateTime, AirQualityIndexDtoOut> dailyPollutionAverage = new Dictionary<DateTime, AirQualityIndexDtoOut>();

            foreach (PollutionQueryDto item in pollutionData)
            {
                if (dailyPollutionAverage.ContainsKey(item.CreatedAt.Date))
                {
                    AirQualityIndexDtoOut dtoOut = dailyPollutionAverage[item.CreatedAt.Date];
                    dtoOut.Pm25RunningSum += item.Pm25;
                    dtoOut.Pm10RunningSum += item.Pm10;
                    dtoOut.Pm25Samples += 1;
                    dtoOut.Pm10Samples += 1;
                    dtoOut.Pm25Concentration = dtoOut.Pm25RunningSum / dtoOut.Pm25Samples;
                    dtoOut.Pm10Concentration = dtoOut.Pm10RunningSum / dtoOut.Pm10Samples;
                }
                else
                {
                    AirQualityIndexDtoOut dtoOut = new AirQualityIndexDtoOut();
                    dtoOut.Pm25Concentration = 0;
                    dtoOut.Pm10Concentration = 0;
                    dtoOut.Pm25RunningSum = item.Pm25;
                    dtoOut.Pm10RunningSum = item.Pm10;
                    dtoOut.Pm25Samples = 1;
                    dtoOut.Pm10Samples = 1;
                    dtoOut.Pm25AirQualityIndex = "";
                    dtoOut.Pm10AirQualityIndex = "";
                    dtoOut.Pm25Color = "";
                    dtoOut.Pm10Color = "";
                    dtoOut.Date = item.CreatedAt.Date;
                    dailyPollutionAverage.Add(item.CreatedAt.Date, dtoOut);
                }
            }

            return dailyPollutionAverage;
        }

        private IndexLevel IndexLevelBinarySearch(List<IndexLevel> indexLevels, double concentration)
        {
            int leftPointer = 0;
            int rightPointer = indexLevels.Count - 1;
            while(leftPointer <= rightPointer)
            {
                int middlePoint = rightPointer - leftPointer / 2;
                IndexLevel item = indexLevels[middlePoint];
                if (item.RangeLow <= concentration && item.RangeHigh >= concentration) return item;
                if (concentration < item.RangeLow) rightPointer = middlePoint - 1;
                else if (concentration > item.RangeHigh) leftPointer = middlePoint + 1;
            }

            return null;
        }
    }

}
