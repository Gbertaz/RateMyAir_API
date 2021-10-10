using System;
using System.Collections.Generic;

#nullable disable

namespace RateMyAir.Entities.Models
{
    public partial class IndexLevel
    {
        public long IndexLevelId { get; set; }
        public string Pollutant { get; set; }
        public double RangeLow { get; set; }
        public double RangeHigh { get; set; }
        public string AirQualityIndex { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
