using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Report.DataAccess
{
    public class WeatherReportDbContext : DbContext
    {
        public WeatherReportDbContext(){ }
        public WeatherReportDbContext(DbContextOptions opts) : base(opts) { }

        public DbSet<WeatherReport> WeatherReport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SnakeCaseIdentityTableNames(modelBuilder);
        }

        private static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherReport>(b => b.ToTable("weather_report"));
        }

    }
}
