namespace CloudWeather.Report.Config
{
    public class WeatherDataConfig
    {
        public string PrecipitationDataProtocol { get; set; } = string.Empty;
        public string PrecipitationDataHost { get; set; } = string.Empty;
        public string PrecipitationDataPort { get; set; } = string.Empty;
        public string TemperatureDataProtocol { get; set; } = string.Empty;
        public string TemperatureDataHost { get; set; } = string.Empty;
        public string TemperatureDataPort { get; set; } = string.Empty;
    }
}