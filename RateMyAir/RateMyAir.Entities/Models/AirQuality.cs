using System;

#nullable disable

namespace RateMyAir.Entities.Models
{
    public partial class AirQuality
    {
        public long AirQualityId { get; set; }
        public double? TemperatureOutdoor { get; set; }
        public double? TemperatureIndoor { get; set; }
        public double? Humidity { get; set; }
        public double? Pressure { get; set; }
        public double? Pm25 { get; set; }
        public double? Pm10 { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
