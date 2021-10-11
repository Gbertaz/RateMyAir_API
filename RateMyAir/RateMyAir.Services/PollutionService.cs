using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.DTO.Queries;
using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Services
{
    public class PollutionService : IPollutionService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IMapper _mapper;

        public PollutionService(IRepositoryManager repoManager, IMapper mapper)
        {
            _repoManager = repoManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the air quality index of each day between <paramref name="fromDate"/> to <paramref name="toDate"/>
        /// based on the average pollution of the 24 hours.
        /// O(n) Time complexity where n is the number of pollutionData samples
        /// O(n) Space complexity
        /// </summary>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <returns>List of AirQualityIndexDtoOut</returns>
        public async Task<List<AirQualityIndexDtoOut>> GetAirQualityIndexAsync(DateTime fromDate, DateTime toDate)
        {
            //Air quality index map table. The data set is already sorted
            List<IndexLevel> indexLevels = await _repoManager.IndexLevels.GetLevels();

            //Get the list of PollutionForQueryDtoOut between the specified range of dates
            List<PollutionForQueryDtoOut> pollutionData = await _repoManager.AirQuality.GetAirQuality(fromDate, toDate, false)
                .OrderBy(x => x.CreatedAt)
                .ProjectTo<PollutionForQueryDtoOut>(_mapper.ConfigurationProvider).ToListAsync();

            Dictionary<DateTime, AirQualityIndexDtoOut> pollutionConcentration = ProcessDailyConcentrationAverage(pollutionData);

            return LookUpAirQualityIndex(pollutionConcentration, indexLevels, true);
        }

        /// <summary>
        /// Processes the 24 hours daily pollution concentration. O(n) Time and Space where n is the number of pullution samples
        /// </summary>
        /// <param name="pollutionData">Pollution samples</param>
        /// <returns>Dictionary<DateTime, AirQualityIndexDtoOut></returns>
        private Dictionary<DateTime, AirQualityIndexDtoOut> ProcessDailyConcentrationAverage(List<PollutionForQueryDtoOut> pollutionData)
        {
            //Hash table that contains the pollution concentration for each 24 hours
            Dictionary<DateTime, AirQualityIndexDtoOut> pollutionConcentration = new Dictionary<DateTime, AirQualityIndexDtoOut>();

            foreach (PollutionForQueryDtoOut item in pollutionData)
            {
                if (pollutionConcentration.ContainsKey(item.CreatedAt.Date))
                {
                    AirQualityIndexDtoOut dtoOut = pollutionConcentration[item.CreatedAt.Date];
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
                    pollutionConcentration.Add(item.CreatedAt.Date, dtoOut);
                }
            }

            return pollutionConcentration;
        }

        private List<AirQualityIndexDtoOut> LookUpAirQualityIndex(Dictionary<DateTime, AirQualityIndexDtoOut> pollutionConcentration, List<IndexLevel> indexLevels, bool useBinarySearch)
        {
            //Output list
            List<AirQualityIndexDtoOut> airQualityIndexes = new List<AirQualityIndexDtoOut>();

            //Constant Time
            List<IndexLevel> pm25IndexLevels = indexLevels.Where(x => x.Pollutant == Enums.Pollutants.Pm25.ToString()).ToList();
            List<IndexLevel> pm10IndexLevels = indexLevels.Where(x => x.Pollutant == Enums.Pollutants.Pm10.ToString()).ToList();

            Dictionary<int, int> indexLevelsLookup = null;

            if (useBinarySearch == false)
            {
                //Creates the lookup table
                indexLevelsLookup = ComputeIndexLevelLookUpTable(indexLevels);
            }

            //Traverse the hash table again to look up the air quality index based on the daily concentration and prepare the output list
            foreach (KeyValuePair<DateTime, AirQualityIndexDtoOut> item in pollutionConcentration)
            {
                AirQualityIndexDtoOut dailyConcentration = item.Value;

                IndexLevel pm25Level = GetAirQualityIndexLevel(indexLevels, indexLevelsLookup, dailyConcentration.Pm25Concentration, useBinarySearch);
                IndexLevel pm10Level = GetAirQualityIndexLevel(indexLevels, indexLevelsLookup, dailyConcentration.Pm10Concentration, useBinarySearch);

                dailyConcentration.Pm25AirQualityIndex = pm25Level.AirQualityIndex;
                dailyConcentration.Pm25AirQualityIndexDescription = pm25Level.Description;
                dailyConcentration.Pm25Color = pm25Level.Color;
                dailyConcentration.Pm10AirQualityIndex = pm10Level.AirQualityIndex;
                dailyConcentration.Pm10AirQualityIndexDescription = pm10Level.Description;
                dailyConcentration.Pm10Color = pm10Level.Color;
                airQualityIndexes.Add(dailyConcentration);
            }

            return airQualityIndexes;
        }

        /// <summary>
        /// Get the Air Quality Index level given the 24 hours average pollution concentration
        /// Both algorithms (Binary Search and Lookup table Search) can be considered as O(1) Time complexity because the number of elements on which they operate is constant
        /// Binary Search uses less memory: O(1) Space complexity
        /// Lookup table uses too much memory: O(n * m) where n is the number of Air Quality index levels (6 elements) and m is the sum of the values of all the 6 ranges
        /// but it is faster: it is a real O(1) Time complexity
        /// So probably the Binary Search is the best choice
        /// </summary>
        /// <param name="indexLevels">Air quality indexes</param>
        /// <param name="indexLevelsLookup">Lookup table</param>
        /// <param name="dailyConcentration">Value of the 24 hours average pollution concentration of the given <paramref name="pollutant"/></param>
        /// <param name="useBinarySearch">Wether to use Binary search to look up for the index level or not</param>
        /// <returns>IndexLevel</returns>
        private IndexLevel GetAirQualityIndexLevel(List<IndexLevel> indexLevels, Dictionary<int, int> indexLevelsLookup, double dailyConcentration, bool useBinarySearch)
        {
            if (useBinarySearch)
            {
                //Binary search is O(log(n)) Time complexity but in this case can be treated as O(1) because n is constant (6 elements)
                return IndexLevelBinarySearch(indexLevels, dailyConcentration);
            }
            else
            {
                //O(1) Time complexity because it is a lookup in a hash table
                return indexLevels[indexLevelsLookup[(int)dailyConcentration]];
            }
        }

        /// <summary>
        /// Computes the Lookup table by associating the daily pollution concentration value to the index of the Air quality range in which the concentration falls into
        /// </summary>
        /// <param name="indexLevels">Air quality indexes</param>
        /// <returns>Hash table where the key is the 24 hours average pollution concentration and the value is the index of the Air quality range in which the concentration falls into</returns>
        private Dictionary<int, int> ComputeIndexLevelLookUpTable(List<IndexLevel> indexLevels)
        {
            Dictionary<int, int> indexLevelLookUp = new Dictionary<int, int>();

            for(int i = 0; i < indexLevels.Count; i++)
            {
                IndexLevel level = indexLevels[i];

                for (int j = (int)level.RangeLow; j < level.RangeHigh; j++)
                {
                    indexLevelLookUp.Add(j, i);
                }
            }

            return indexLevelLookUp;
        }

        /// <summary>
        /// Pollution concentration range lookup using a binary search.
        /// O(log n) time complexity
        /// O(1) Space complexity
        /// </summary>
        /// <param name="indexLevels">List of air quality index level ranges</param>
        /// <param name="concentration">Pollution concentration</param>
        /// <returns>IndexLevel</returns>
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
