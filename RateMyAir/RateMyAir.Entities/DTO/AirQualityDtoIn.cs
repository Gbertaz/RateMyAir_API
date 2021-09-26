using System;
using System.ComponentModel.DataAnnotations;

namespace RateMyAir.Entities.DTO
{
    public class AirQualityDtoIn
    {
        [Display(Name = "TemperatureOutdoor")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float TemperatureOutdoor { get; set; }

        [Display(Name = "TemperatureIndoor")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float TemperatureIndoor { get; set; }

        [Display(Name = "Humidity")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Humidity { get; set; }

        [Display(Name = "Pressure")]
        [Range(0, float.MaxValue, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pressure { get; set; }

        [Display(Name = "Pm25")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm25 { get; set; }

        [Display(Name = "Pm10")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm10 { get; set; }
    }
}