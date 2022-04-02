using CloudWeather.Report.DataAccess;

namespace CloudWeather.Report.BusinessLogic
{
    /// <summary>
    /// Aggregates data from multiple external sources to build a weather report
    /// </summary>
    public interface IWeatherReportAggregator
    {
        /// <summary>
        /// Builds and returns a weekly Weather Report
        /// Persists WeeklyWeatherReport data
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public Task<WeatherReport> BuildReport(string zip, int days);
    }
}