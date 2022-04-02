namespace CloudWeather.Precipitation.DataAccess
{
    public class PrecipitationPostModel
    {
        public Guid Id { get; set; }
        public decimal AmountInches { get; set; }
        public string WeatherType { get; set; }
        public string ZipCode { get; set; }
    }
}