namespace CloudWeather.Temperature.DataAccess
{
    public class TemperaturePostModel
    {
        public Guid Id { get; set; }
        public decimal TempHighF { get; set; }
        public decimal TempLowF { get; set; }
        public string ZipCode { get; set; } = string.Empty;
    }
}