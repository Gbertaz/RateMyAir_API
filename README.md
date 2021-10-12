# RateMyAir API

This repository contains the implementation of the **Rest APIs** for the project **RateMyAir**.  

It exposes the services to manipulate and access the Air data (temperature, humidity, pressure and particulate matter pollution) collected by different sensors (more on that soon) to understand more about Air Quality where I live.  

Developed in **.NET Core 5** using the **Repository pattern**, it is designed to be lightweight and efficient paying special attention in complying with Microsoft and HTTP standards in order to be robust, understandable and easily consumable by any client. The goal is to run it on a low-performing hardware such as the Raspberry Pi 4.

# Air Quality Index

The Air Quality Index level is based on the concentration values of the following pollutants in µg/m3:

* Particulate Matter: PM 10 micrograms per cubic meter
* Fine Particulate Matter: PM 2.5 micrograms per cubic meter

| Pollutant  | Index level | Index level | Index level | Index level | Index level | Index level |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
|        | Good  | Fair  | Moderate | Poor | Very poor | Extremely poor |
| PM 2.5 | 0-10  | 10-20  | 20-25 | 25-50 | 50-75 | 75-800 |
| PM 10  | 0-20 | 20-40 | 40-50 | 50-100 | 100-150 | 150-1200 |

Source: https://www.eea.europa.eu

# Table of Contents

* [API Specifications](doc/ApiSpecs.md)
* [Solution Specifications](doc/SolutionSpecs.md)
* [Details of the Algorithm](doc/Algorithm.md)

# Planned features and developments

* The database structure and the APIs only support one sensor which collects temperature (indoor and outdoor), humidity, pressure and particulate matter pollution. A future development is to make everything more generic to support *n* sensors
* The paged output should contain the link to the next and previous page
* Data caching
* Rate limiting and Throttling
