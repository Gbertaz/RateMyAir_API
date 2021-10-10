using System;

namespace RateMyAir.Entities.DTO
{
    public class AirQualityDtoOut
    {
        public long Id { get; set; }
        public double TemperatureOutdoor { get; set; }
        public double TemperatureIndoor { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double Pm25 { get; set; }
        public double Pm10 { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
