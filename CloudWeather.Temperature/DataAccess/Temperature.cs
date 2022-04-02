namespace CloudWeather.Temperature.DataAccess
{
    public class Temperature
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TempHighF { get; set; }
        public decimal TempLowF { get; set; }
        public string ZipCode { get; set; }

        public Temperature(TemperaturePostModel model)
        {
            Id = model.Id;
            CreatedOn = DateTime.Now.ToUniversalTime();
            TempHighF = model.TempHighF;
            TempLowF = model.TempLowF;
            ZipCode = model.ZipCode;
        }
    }
}
