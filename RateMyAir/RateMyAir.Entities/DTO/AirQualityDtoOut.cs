using System;

namespace RateMyAir.Entities.DTO
{
    public class AirQualityDtoOut
    {
        public int Id { get; set; }
        public float OutdoorTemp { get; set; }
        public float IndoorTemp { get; set; }
        public float IndoorHumidity { get; set; }
        public float IndoorPressure { get; set; }
        public float IndoorDewPoint { get; set; }
        public float IndoorPm25 { get; set; }
        public float IndoorPm10 { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
