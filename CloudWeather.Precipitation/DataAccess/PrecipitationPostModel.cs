namespace CloudWeather.Precipitation.DataAccess
{
    public class PrecipitationPostModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        public decimal AmountInches { get; set; }
        public string WeatherType { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}