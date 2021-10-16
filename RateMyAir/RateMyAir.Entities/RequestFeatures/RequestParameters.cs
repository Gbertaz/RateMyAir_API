using System;

namespace RateMyAir.Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        public int PageNumber { get; set; } = 1;
        private const int maxPageSize = 50;
        private int _pageSize = 5;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }

    public class GetAirQualityIndexParameters
    {
        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(-1).AddSeconds(1);
        public DateTime ToDate { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);
        public bool ValidDateRange => ToDate >= FromDate;
    }

    public class GetAirQualityParameters : RequestParameters
    {
        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(-1).AddSeconds(1);
        public DateTime ToDate { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);
        public bool ValidDateRange => ToDate >= FromDate;
    }
}
