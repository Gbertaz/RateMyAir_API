using System;
using System.ComponentModel.DataAnnotations;

namespace RateMyAir.Entities.DTO
{
    public class AirDataDtoIn
    {
        [Display(Name = "Temperature")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float Temperature { get; set; }

        [Display(Name = "Temperature0")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float Temperature0 { get; set; }

        [Display(Name = "Temperature1")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float Temperature1 { get; set; }

        [Display(Name = "Humidity")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Humidity { get; set; }

        [Display(Name = "Pressure")]
        [Range(0, float.MaxValue, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pressure { get; set; }

        [Display(Name = "Altitude")]
        [Range(-1000, 9000, ErrorMessage = "{0} is required and it can't be lower than -1000")]
        public float Altitude { get; set; }

        [Display(Name = "Dew point")]
        [Range(-100, 100, ErrorMessage = "{0} is required and it can't be lower than -100")]
        public float DewPoint { get; set; }

        [Display(Name = "Pm25")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm25 { get; set; }

        [Display(Name = "Pm10")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm10 { get; set; }

        [Display(Name = "Pm25 normalized")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm25_norm { get; set; }

        [Display(Name = "Pm10 normalized")]
        [Range(0, 100, ErrorMessage = "{0} is required and it can't be lower than 0")]
        public float Pm10_norm { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }
    }
}
