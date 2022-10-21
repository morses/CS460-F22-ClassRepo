using AjaxExample.Models;

namespace AjaxExample.Services
{
    public interface IEarthquakeService
    {
        IEnumerable<Earthquake> GetRecentEarthquakes(EarthquakeTimeRange range);
        IEnumerable<Earthquake> GetRecentEarthquakes2(EarthquakeTimeRange range);
    }
}
