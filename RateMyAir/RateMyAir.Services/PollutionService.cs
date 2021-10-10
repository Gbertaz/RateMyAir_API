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

            //Hash table that contains the pollution concentration for each 24 hours
            Dictionary<DateTime, AirQualityIndexDtoOut> pollutionConcentration = new Dictionary<DateTime, AirQualityIndexDtoOut>();

            foreach(PollutionForQueryDtoOut item in pollutionData)
            {
                if (pollutionConcentration.ContainsKey(item.CreatedAt.Date))
                {
                    AirQualityIndexDtoOut dtoOut = pollutionConcentration[item.CreatedAt.Date];
                    
                    if(item.Pm25 > 0)
                    {
                        dtoOut.Pm25Concentration += item.Pm25;
                        dtoOut.Pm25Samples += 1;
                    }

                    if (item.Pm10 > 0)
                    {
                        dtoOut.Pm10Concentration += item.Pm10;
                        dtoOut.Pm10Samples += 1;
                    }
                }
                else
                {
                    AirQualityIndexDtoOut dtoOut = new AirQualityIndexDtoOut();
                    dtoOut.Pm25Concentration = (item.Pm25 > 0) ? item.Pm25 : 0;
                    dtoOut.Pm10Concentration = (item.Pm10 > 0) ? item.Pm10 : 0;
                    dtoOut.Pm25Samples = (item.Pm25 > 0) ? 1 : 0;
                    dtoOut.Pm10Samples = (item.Pm10 > 0) ? 1 : 0;
                    dtoOut.Pm25AirQualityIndex = "";
                    dtoOut.Pm10AirQualityIndex = "";
                    dtoOut.Pm25Color = "";
                    dtoOut.Pm10Color = "";
                    dtoOut.Date = item.CreatedAt.Date;
                    pollutionConcentration.Add(item.CreatedAt.Date, dtoOut);
                }
            }

            //Output list
            List<AirQualityIndexDtoOut> airQualityIndexes = new List<AirQualityIndexDtoOut>();

            //Traverse the hash table again to calculate the concentration average of the 24 hours and associate the air quality index
            foreach (KeyValuePair<DateTime, AirQualityIndexDtoOut> item in pollutionConcentration)
            {
                AirQualityIndexDtoOut dailyConcentration = item.Value;
                dailyConcentration.Pm25Concentration /= dailyConcentration.Pm25Samples;
                dailyConcentration.Pm10Concentration /= dailyConcentration.Pm10Samples;

                IndexLevel pm25Level = GetAirQualityIndexLevel(indexLevels, dailyConcentration.Pm25Concentration, Enums.Pollutants.Pm25, true);
                IndexLevel pm10Level = GetAirQualityIndexLevel(indexLevels, dailyConcentration.Pm10Concentration, Enums.Pollutants.Pm10, true);

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
        /// Lookup table uses too much memory: O(n * m) but it is faster: it is a real O(1) Time complexity if the look up table is computed only once  with a singleton
        /// but if the IndexLevels are changed in the database, the application must be restarted!
        /// So probably the Binary Search is the best choice
        /// </summary>
        /// <param name="indexLevels">Air quality indexes</param>
        /// <param name="dailyConcentration">Value of the 24 hours average pollution concentration of the given <paramref name="pollutant"/></param>
        /// <param name="pollutant">Pollutant</param>
        /// <param name="useBinarySearch">Wether to use Binary search to look up for the index level or not</param>
        /// <returns>IndexLevel</returns>
        private IndexLevel GetAirQualityIndexLevel(List<IndexLevel> indexLevels, double dailyConcentration, Enums.Pollutants pollutant, bool useBinarySearch)
        {
            //Constant time => negligible as regards of Time and Space complexity
            List<IndexLevel> pollutantIndexLevels = indexLevels.Where(x => x.Pollutant == pollutant.ToString()).ToList();

            if (useBinarySearch)
            {
                //Binary search is O(log(n)) Time complexity but in this case can be treated as O(1) because n is constant (6 elements)
                return IndexLevelBinarySearch(pollutantIndexLevels, dailyConcentration);
            }
            else
            {
                //Creates the lookup table in O(n * m) where n is the number of index levels (6 elements) and m is the sum of the values of all the 6 ranges
                Dictionary<int, int> pm10IndexLevelLookUp = ComputeIndexLevelLookUpTable(pollutantIndexLevels);

                //O(1) Time complexity because it is a lookup in a hash table
                return pollutantIndexLevels[pm10IndexLevelLookUp[(int)dailyConcentration]];
            }
        }

        /// <summary>
        /// Computes the Lookup table by associating the daily pollution concentration value to the Air quality index's index (no pun intended)
        /// </summary>
        /// <param name="indexLevels">Air quality indexes</param>
        /// <returns>Hash table where the key is the pollution concentration value and the value is the Air quality index's index</returns>
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
