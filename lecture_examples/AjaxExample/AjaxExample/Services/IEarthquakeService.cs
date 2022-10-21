using AjaxExample.Models;

namespace AjaxExample.Services
{
    public interface IEarthquakeService
    {
        IEnumerable<Earthquake> GetRecentEarthquakes(EarthquakeTimeRange range);
    }
}
