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
List<IndexLevel> indexLevels = await _airQualityIndexService.GetAirQualityLevelsAsync();
```

A second query is executed to fetch the PM2.5 and PM10 pollution from *AirData* table filtering out the values between *DateFrom* and *DateTo*

```
//Get the list of PollutionForQueryDtoOut between the specified range of dates
List<PollutionForQueryDtoOut> pollutionData = await _repoManager.AirQuality.GetAirQuality(fromDate, toDate, false)
    .OrderBy(x => x.CreatedAt)
    .ProjectTo<PollutionForQueryDtoOut>(_mapper.ConfigurationProvider).ToListAsync();
```
# Complexity analysis

### STEP 1
For each day, starting from *DateFrom*, the algorithm has to group all the PM2.5 and PM10 values within the same 24 hours, sum them up and calculate the average. This process can be done in O(n) time complexity (where n is the number of records fetched from the database) by using a hash table where the key represents the date (time exluded) and the value is the *AirQualityIndexDtoOut* object which contains the 24 hours average pollution concentration value.  

### STEP 2
The final step is the lookup of the Air Quality Index given the 24 hours average pollution concentration: traverse the hash table, for each key/value pair get the corresponding index level searching the concentration in the array of ranges. 

I have implemented the lookup in two alternative ways: 

* using a **Binary Search** O(log(n)) Time where n is the number of Air Quality Index levels (12 in this case), O(1) Space
* using a **Hash Table** built associating the daily pollution concentration value to the index of the Air Quality range in which the concentration falls into. O(1) Time, O(n * m) Space where n is the number of Air Quality Index levels (12 in this case) and m is the sum of the values of all the 12 ranges

Looking at the *IndexLevels* table, we can see that we have 6 ranges for each pollutant, for a total of 2000 values. Therefore the **Hash Table** solution requires a Space equivalent to 2000 *double* but it is best in terms of Time complexity O(1).  
The **Binary Search** is slower but doesn't require extra space, however it always run on a fixed number of elements (12 in this case) so we can consider O(log(12)) as constant => O(1).

## Improvement 1

The **Hash Table** Space complexity can be improved by excluding the last Pm2.5's range and the last PM10's range because it's where most of the values are, lowering the required Space from 2000 to 225 *double* values. 

## Improvement 2

Develop a **Batch** program that runs once a day just after midnight which processes the data of the day before. This way the API only has to return the data without performing any processing.

# Conclusions

I haven't tested it yet but given that the number of samples is not outrageous, the **Binary Search** is probably the best choice in this particular case. Any suggestion for improvements is very welcome!

To give some numbers: the sensors are collecting data every 15 minutes resulting in 96 samples per day that means 35040 samples in one year. Let's suppose the Client requests an entire year of data, the algorithm processes 35040 in [STEP 1](#step-1) condensing them up in 365 values (the averages for each day). The [STEP 2](#step-2) then executes 365 times a constant time **Binary Search** or a lookup in a **Hash Table**.
