using System;

namespace RateMyAir.Entities.DTO
{
    public class AirQualityDtoOut
    {
        public int Id { get; set; }
        public float TemperatureOutdoor { get; set; }
        public float TemperatureIndoor { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public float Pm25 { get; set; }
        public float Pm10 { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
