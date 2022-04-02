﻿namespace CloudWeather.Precipitation.DataAccess
{
    public class Precipitation
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal AmountInches { get; set; }
        public string WeatherType { get; set; }
        public string ZipCode { get; set; }

        public Precipitation(PrecipitationPostModel model)
        {
            Id = model.Id;
            CreatedOn = DateTime.Now.ToUniversalTime();
            AmountInches = model.AmountInches;
            WeatherType = model.WeatherType;
            ZipCode = model.ZipCode;
        }
    }
}
