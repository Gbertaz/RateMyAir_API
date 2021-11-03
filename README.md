# RateMyAir API

This repository contains the implementation of the **Rest APIs** for the project **RateMyAir**.  

It exposes the services to manipulate and access the Air data (temperature, humidity, pressure and particulate matter pollution) measured by [RateMyAir_FW](https://github.com/Gbertaz/RateMyAir_FW) to understand more about Air Quality where I live.  

Developed in **.NET 6** using the **Repository pattern**, it is designed to be lightweight and efficient paying special attention in complying with Microsoft and HTTP standards in order to be robust, understandable and easily consumable by any client. The goal is to run it on a low-performing hardware such as the Raspberry Pi 4.

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

# Hosting a .NET application on a Raspberry Pi

This is a step by step guide on how to host a .NET REST API on a Raspberry Pi and make it securely accessible outside your Local Area Network from anywhere in the world.

I am deploying the application on a Raspberry Pi 4 Model B which is powerful enough to host the APIs as long as you don't expect to handle thousands of requests.
In any case to run a .NET app you need at least a Raspberry Pi 2 with an ARMv7 processor.

I am assuming that we start from a new fresh installation of Raspbian, we are going to deploy a self-contained app so we don't need to install .NET Core but we still need to do quite a few steps:
Some of these steps are optional, if you don't want to make the API accessible from outside your LAN you can skip the steps 7 and 8.

1. [Install Raspberry Pi OS](doc/hosting/InstallDebian.md)
2. [Setup a static IP Address](doc/hosting/StaticIp.md)
3. [Install VNC server (Optional)](doc/hosting/VncServer.md)
4. [Install SQLite browser](doc/hosting/SqliteBrowser.md)
5. [Install nginx web server](doc/hosting/Nginx.md)
6. [Deploy and run RateMyAir_API](doc/hosting/DeployRateMyAirAPI.md)
7. [Install a dynamic DNS (Optional)](doc/hosting/DynamicDns.md)
8. [Install Letsencrypt SSL certificate (Optional)](doc/hosting/CertificateSsl.md)

## Test the APIs

If everything has been set up following all the steps with the same naming conventions, the APIs should be available at the following URL:

```
https://yourdomain.duckdns.org/ratemyair/api
```

All the endpoints are authenticated using the API Key method thus make sure to pass the API KEY specified in [appsettings.Production.json](https://github.com/Gbertaz/RateMyAir_API/blob/master/RateMyAir/RateMyAir.API/appsettings.Production.json) in the header of the calls. The header's key to be added is **ApiKey**.

# Table of Contents

* [API Specifications](doc/ApiSpecs.md)
* [Solution Specifications](doc/SolutionSpecs.md)
* [Details of the Air Quality Index algorithm](doc/Algorithm.md)

# Planned features and developments

* The database structure and the APIs only support one sensor which collects temperature (indoor and outdoor), humidity, pressure and particulate matter pollution. A future development is to make everything more generic to support *n* sensors
* The paged output should contain the link to the next and previous page
* Data caching
* Rate limiting and Throttling
