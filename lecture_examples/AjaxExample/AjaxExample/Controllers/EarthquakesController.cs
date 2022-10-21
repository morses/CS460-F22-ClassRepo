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

        [HttpGet("{timeRange}")]
        public ActionResult<IEnumerable<Earthquake>> Earthquakes(EarthquakeTimeRange timeRange = EarthquakeTimeRange.PastHour)
        {
            if (!(timeRange >= EarthquakeTimeRange.PastHour && timeRange <= EarthquakeTimeRange.PastMonth))
            {
                timeRange = EarthquakeTimeRange.PastHour;
            }

            return Ok(_earthquakeService.GetRecentEarthquakes(timeRange));
        }

        [HttpGet("timeranges")]
        // Don't have a type for this one since we use an anonymous type.  See how there is less documentation in Swagger as a result
        public ActionResult AvailableTimeRanges()
        {
            // Build items for select list
            var selItems = Enum.GetValues<EarthquakeTimeRange>()
                               .Select(tr => new {
                                   Value = ((int)tr).ToString(),
                                   Text = Earthquake.TimeRanges[tr]
                               })
                               .ToList();
            return Ok(selItems);
        }
    }
}
