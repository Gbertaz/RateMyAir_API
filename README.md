# RateMyAir API

This repository contains the implementation of the Rest APIs for the project RateMyAir.  

Developed in .NET Core 5 using the Repository pattern, are designed to be lightweight and efficient paying special attention in complying with Microsoft standards, HTTP and also from a security point of view, in order to be robust, understandable and easily consumable by any client. The goal is to run them on a low-performing hardware such as the Raspberry Pi 4.

#### Some of the solutions adopted in this project might not be the best but it is also developed as a teaching project.

## Specifications

* Database First approach
* Output in Json or Xml format
* Output pagination and filtering
* Global error handling
* ApiKey Authentication method for clients
* SQLite database
* Logging on file system
* Supports API versioning
* Swagger documentation
* Mapping entities to Data Transfer Objects using AutoMapper

## Database Scaffolding

The Database First Approach requires to create the tables in the database first, then creating the models in Visual Studio with the following procedure:

* In Visual Studio open Package Manager Console (Tools => Nuget Package Manager => Package Manager Console)
* In the "Default project" dropdown menu select "RateMyAir.Entities"
* Execute the following command:

```
Scaffold-DbContext "Data Source=<path to your sqlite database>" Microsoft.EntityFrameworkCore.Sqlite -OutputDir Models -force -context DatabaseContext
```

* Open "DatabaseContext.cs" class in "RateMyAir.Entities.Models" and delete the "OnConfiguring" method

## API Output

All the APIs use the following Json output structures depending on whether the response is paged or not:

#### Response

```
{
    "success": true,
    "message": "",
    "errors": [],
    "data": {
        
    }
}
```

#### Paged Response

```
{
    "pageNumber": 1,
    "pageSize": 5,
    "recordsTotal": 0,
    "success": true,
    "message": "",
    "errors": [],
    "data": []
}
```


## Dependencies

* Microsoft.EntityFrameworkCore (5.0.10)
* Microsoft.EntityFrameworkCore.Sqlite (5.0.10)
* Microsoft.EntityFrameworkCore.Design (5.0.10)
* Microsoft.EntityFrameworkCore.Sqlite.Design (1.1.6)
* Microsoft.EntityFrameworkCore.Tools (5.0.10)
* AutoMapper.Extensions.Microsoft.DependencyInjection (8.1.1)
* Microsoft.AspNetCore.Mvc.Versioning (5.0.0)
* Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer (5.0.0)
* Newtonsoft.Json (13.0.1)
* Swashbuckle.AspNetCore (6.2.2)
* AutoMapper (10.1.1)
* NLog.Extensions.Logging (1.7.4)

## Planned features and developments

* The paged output should contain the link to the next and previous page
* Data caching
* Rate limiting and Throttling
