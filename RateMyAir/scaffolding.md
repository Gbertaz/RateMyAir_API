## Database First model scaffolding

1) In Visual Studio open Package Manager Console (Tools => Nuget Package Manager => Package Manager Console)

2) Execute the following command:

Scaffold-DbContext "Data Source=D:\\Projects\\RateMyAir_API\\Database\\airquality.sqlite" Microsoft.EntityFrameworkCore.Sqlite -OutputDir Models -force -context DatabaseContext

3) Open DatabaseContext.cs class in RateMyAir.Entities.Models and delete the OnConfiguring method