using AjaxExample.Models;
using AjaxExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AjaxExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarthquakesController : ControllerBase
    {
        private readonly IEarthquakeService _earthquakeService;

        public EarthquakesController(IEarthquakeService earthquakeService)
        {
            _earthquakeService = earthquakeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Earthquake>> Earthquakes(EarthquakeTimeRange timeRange = EarthquakeTimeRange.PastHour)
        {
            if (!(timeRange >= EarthquakeTimeRange.PastHour && timeRange <= EarthquakeTimeRange.PastMonth))
            {
                timeRange = EarthquakeTimeRange.PastHour;
            }

            return Ok(_earthquakeService.GetRecentEarthquakes(timeRange));
        }
    }
}
