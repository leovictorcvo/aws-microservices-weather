namespace CloudWeather.Temperature.DataAccess
{
    public class Temperature
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TempHighF { get; set; }
        public decimal TempLowF { get; set; }
        public string ZipCode { get; set; } = string.Empty;

        public Temperature()
        {

        }
        public Temperature(TemperaturePostModel model)
        {
            Id = model.Id;
            CreatedOn = model.CreatedOn.ToUniversalTime();
            TempHighF = model.TempHighF;
            TempLowF = model.TempLowF;
            ZipCode = model.ZipCode;
        }
    }
}
