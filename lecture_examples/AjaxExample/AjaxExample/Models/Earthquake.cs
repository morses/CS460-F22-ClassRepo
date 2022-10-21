namespace AjaxExample.Models
{
    public enum EarthquakeTimeRange
    {
        PastHour = 1,
        PastDay = 2,
        PastWeek = 3,
        PastMonth = 4
    }

    public class Earthquake
    {
        public double Magnitude { get; set; }
        public string Location { get; set; }

        public static readonly Dictionary<EarthquakeTimeRange, string> TimeRanges = new Dictionary<EarthquakeTimeRange, string>() {
            {EarthquakeTimeRange.PastHour, "hour"},
            {EarthquakeTimeRange.PastDay, "day"},
            {EarthquakeTimeRange.PastWeek, "week"},
            {EarthquakeTimeRange.PastMonth, "month"}
        };

        public Earthquake(double magnitude, string location)
        {
            Magnitude = magnitude;
            Location = location;
        }

    }
}
