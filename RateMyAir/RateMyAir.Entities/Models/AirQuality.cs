using System;
using System.ComponentModel.DataAnnotations;

namespace RateMyAir.Entities.Models
{
    public class AirQuality
    {
        [Key]
        public int AirDataID { get; set; }
        public float Temperature { get; set; }
        public float Temperature0 { get; set; }
        public float Temperature1 { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public float Altitude { get; set; }
        public float DewPoint { get; set; }
        public float Pm25 { get; set; }
        public float Pm10 { get; set; }
        public float Pm25_norm { get; set; }
        public float Pm10_norm { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}