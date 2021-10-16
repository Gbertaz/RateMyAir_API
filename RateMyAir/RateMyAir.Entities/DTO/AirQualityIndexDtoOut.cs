using System;
using System.Text.Json.Serialization;

namespace RateMyAir.Entities.DTO
{
    public class AirQualityIndexDtoOut
    {
        [JsonIgnore]
        public double Pm25RunningSum { get; set; }
        [JsonIgnore]
        public double Pm10RunningSum { get; set; }
        public double Pm25Concentration { get; set; }
        public double Pm10Concentration { get; set; }
        public int Pm25Samples { get; set; }
        public int Pm10Samples { get; set; }
        public string Pm25AirQualityIndex { get; set; }
        public string Pm10AirQualityIndex { get; set; }
        public string Pm25AirQualityIndexDescription { get; set; }
        public string Pm10AirQualityIndexDescription { get; set; }
        public string Pm25Color { get; set; }
        public string Pm10Color { get; set; }
        public DateTime Date { get; set; }
    }
}
