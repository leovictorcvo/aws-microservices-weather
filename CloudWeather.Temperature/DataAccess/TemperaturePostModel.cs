namespace CloudWeather.Temperature.DataAccess
{
    public class TemperaturePostModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        public decimal TempHighF { get; set; }
        public decimal TempLowF { get; set; }
        public string ZipCode { get; set; } = string.Empty;
    }
}