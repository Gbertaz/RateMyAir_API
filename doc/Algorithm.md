# Details of the Air Quality Index algorithm

The API reponsible of calculating and returning the Air Quality index is available at the following endpoint

```
api/airquality/index
```

Returns the Air Quality Index of each day between the *DateFrom* and *DateTo* parameters based on the average pollution of the 24 hours. If no dates are provided, returns the last 24 hours Air Quality Index.  

The implementation can be found in *RateMyAir.Services.PollutionService.cs* class.

The first part of the algorithm hits the database to fetch the necessary data by reading the Air Quality index levels stored in the *IndexLevels* table. To make things even faster and reduce unnecessary database access, this query is executed only once a day, the list of *IndexLevels* are cached in memory for a fast access.

```
//Get the Air Quality index levels either from database or memory cache
List<IndexLevel> pm25IndexLevels = await _airQualityIndexService.GetAirQualityLevelsAsync(Enums.Pollutants.Pm25);
List<IndexLevel> pm10IndexLevels = await _airQualityIndexService.GetAirQualityLevelsAsync(Enums.Pollutants.Pm10);
```

A second query is executed to fetch the PM2.5 and PM10 pollution from *AirQuality* table filtering out the values between *DateFrom* and *DateTo*

```
List<PollutionQueryDto> pollutionData = await FilterAirQuality(filter, false)
    .ProjectTo<PollutionQueryDto>(_mapper.ConfigurationProvider).ToListAsync();
```
# Complexity analysis

### STEP 1
For each day, starting from *DateFrom*, the algorithm groups all the PM2.5 and PM10 values within the same 24 hours, sum them up and calculate the average. This process can be done in O(n) time complexity (where n is the number of records fetched from the database) by using a hash table where the key represents the date (time exluded) and the value is the *AirQualityIndexDtoOut* object which contains the 24 hours average pollution concentration value.  

### STEP 2
The final step is the lookup of the Air Quality Index given the 24 hours average pollution concentration: traverse the hash table, for each key/value pair get the corresponding index level searching the concentration in the array of ranges. 

I have implemented the lookup using a **Binary Search** O(log(n)) Time where n is the number of Air Quality Index levels (12 in this case), O(1) Space
However it always run on a fixed number of elements (12 in this case) so we can consider O(log(12)) as constant => O(1).

## Possible improvement

Develop a **Batch** program that runs once a day just after midnight which processes the data of the day before. This way the API only has to return the data without performing any processing.

# Conclusions

I haven't tested it yet but given that the number of samples is not outrageous, the **Binary Search** is probably the best choice in this particular case. Any suggestion for improvements is very welcome!

To give some numbers: the sensors are collecting data every 15 minutes resulting in 96 samples per day that means 35040 samples in one year. Let's suppose the Client requests an entire year of data, the algorithm processes 35040 in [STEP 1](#step-1) condensing them up in 365 values (the averages for each day). The [STEP 2](#step-2) then executes 365 times a constant time **Binary Search** or a lookup in a **Hash Table**.
