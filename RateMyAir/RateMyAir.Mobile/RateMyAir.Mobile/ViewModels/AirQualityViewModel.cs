using System;
using System.Collections.Generic;
using System.Text;

namespace RateMyAir.Mobile.ViewModels
{
    public class AirQualityViewModel : BaseViewModel
    {
        public int _airQualityIndex;
        public string _airQualityIndexColor;

        public int AirQualityIndex
        {
            get => _airQualityIndex;
            set => SetProperty(ref _airQualityIndex, value);
        }

        public string AirQualityIndexColor
        {
            get => _airQualityIndexColor;
            set => SetProperty(ref _airQualityIndexColor, value);
        }

        public AirQualityViewModel()
        {
            _airQualityIndex = 31;
            _airQualityIndexColor = "#f0e641";
        }

    }
}
