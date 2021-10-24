## Project Specifications

* Database First approach
* Global error handling
* SQLite database
* Logging on file system
* Supports API versioning
* Mapping entities to Data Transfer Objects using AutoMapper
* In memory data caching

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

## Database Scaffolding

The *Database First Approach* requires to create the tables in the database first, then creating the models in Visual Studio with the following procedure:

* In Visual Studio open Package Manager Console (Tools => Nuget Package Manager => Package Manager Console)
* In the "Default project" dropdown menu select "RateMyAir.Entities"
* Execute the following command:

```
Scaffold-DbContext "Data Source=<path to your sqlite database>" Microsoft.EntityFrameworkCore.Sqlite -OutputDir Models -force -context DatabaseContext
```

* Open "DatabaseContext.cs" class in "RateMyAir.Entities.Models" and delete the "OnConfiguring" method

## Compilation 

Compile the project for release on linux based system with this command:

```
dotnet publish -c Release -r linux-arm
```
