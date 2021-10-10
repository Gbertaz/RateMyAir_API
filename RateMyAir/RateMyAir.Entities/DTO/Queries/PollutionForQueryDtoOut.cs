using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMyAir.Entities.DTO.Queries
{
    public class PollutionForQueryDtoOut
    {
        public double Pm25 { get; set; }
        public double Pm10 { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
