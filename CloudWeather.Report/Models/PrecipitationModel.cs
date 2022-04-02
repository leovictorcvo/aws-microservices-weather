namespace CloudWeather.Report.Models
{
    public class PrecipitationModel
    {
        public decimal AmountInches { get; set; }
        public string WeatherType { get; set; } = string.Empty;
    }
}