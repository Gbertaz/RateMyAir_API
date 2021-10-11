# RateMyAir API

This repository contains the implementation of the **Rest APIs** for the project **RateMyAir**.  

It exposes the services to manipulate and provide the Air data (temperature, humidity, pressure and particulate matter pollution) collected by different sensors (more on that soon) to understand more about Air Quality where I live.  

Developed in **.NET Core 5** using the **Repository pattern**, it is designed to be lightweight and efficient paying special attention in complying with Microsoft and HTTP standards in order to be robust, understandable and easily consumable by any client. The goal is to run it on a low-performing hardware such as the Raspberry Pi 4.

## Air Quality Index

The Air Quality Index level is based on the concentration values of the following pollutants in µg/m3:

* Particulate Matter: PM 10 micrograms per cubic meter
* Fine Particulate Matter: PM 2.5 micrograms per cubic meter

| Pollutant  | Index level | Index level | Index level | Index level | Index level | Index level |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
|        | Good  | Fair  | Moderate | Poor | Very poor | Extremely poor |
| PM 2.5 | 0-10  | 10-20  | 20-25 | 25-50 | 50-75 | 75-800 |
| PM 10  | 0-20 | 20-40 | 40-50 | 50-100 | 100-150 | 150-1200 |

### Details of the algorithm

The API reponsible of calculating and returning the Air Quality index is available at the following endpoint

```
api/airquality/index
```

Returns the Air Quality Index of each day between the *DateFrom* and *DateTo* parameters based on the average pollution of the 24 hours. If no dates are provided, returns the last 24 hours Air Quality Index.  

The implementation can be found in *RateMyAir.Services.PollutionService.cs* class.

The first part of the algorithm hits the database to fetch the necessary data by reading the Air Quality index levels stored in the *IndexLevels* table

```
//Air quality index map table. The data set is already sorted
List<IndexLevel> indexLevels = await _repoManager.IndexLevels.GetLevels();
```

A second query is executed to fetch the PM2.5 and PM10 pollution from *AirData* table filtering out the values between *DateFrom* and *DateTo*

```
//Get the list of PollutionForQueryDtoOut between the specified range of dates
List<PollutionForQueryDtoOut> pollutionData = await _repoManager.AirQuality.GetAirQuality(fromDate, toDate, false)
    .OrderBy(x => x.CreatedAt)
    .ProjectTo<PollutionForQueryDtoOut>(_mapper.ConfigurationProvider).ToListAsync();
```

At this point - for each day - starting from *DateFrom*, the algorithm has to group all the PM2.5 and PM10 values within the same 24 hours, sum them up and calculate the average. This process can be done in O(n) time complexity (where n is the number of records fetched from the database) by using a hash table where the key represents the date and the value is the sum of the pollution concentration value. At the same time a counter keeps track of the number of the values in order to compute the average.  


# Contents

* [API Specifications](doc/ApiSpecs.md)
* [Solution Specifications](doc/SolutionSpecs.md)

## Planned features and developments

* The paged output should contain the link to the next and previous page
* Data caching
* Rate limiting and Throttling
