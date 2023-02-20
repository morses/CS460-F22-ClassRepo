using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AsyncDemo.Models;
using System.Collections.Generic;

namespace AsyncDemo.Controllers;

/*
    Example used to show the difference between synchronous and async execution when
    performing I/O.  For asynchronous, we do 2 versions: 1) the straightforward conversion
    from sync to async, then 2) the "proper" way when more than 1 async call in the same method
    can actually be performed at the same time.

    Remember, there are various purposes to employing asynchronous programming, which include:
        -- efficient use of resources (threads), e.g. web server with high request load
        -- faster execution via parallel execution, e.g. making a single request faster

    It's important to know what your goals are and how this works.  Simply labeling it Async
    may not actually improve anything.

    DISCLAIMER: I've taken the liberty here of not using dependency injection and the service pattern to 
    simplify the demo.  Same goes for the lack of exception handling and placing everything in the Models 
    folder.  So, this is not "proper" or "good" and certainly isn't amenable to testing.
*/

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CoinDeskAPI _coinDesk;
    private readonly CryptoWatchAPI _cryptoWatch;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _coinDesk = new CoinDeskAPI();
        _cryptoWatch = new CryptoWatchAPI();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult GetDataSynchronous()
    {
        _logger.LogInformation("GetDataSynchronous");
        EarthquakeAPI quakeSource = new EarthquakeAPI("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/2.5_day.geojson");
        IEnumerable<Earthquake> quakes  = quakeSource.GetRecentEarthquakes().OrderByDescending(e => e.Magnitude);
        List<ExchangeCandlestick> bitcoin = _cryptoWatch.GetHistoricalBitcoinData();
        return Json(new { earthquakes = quakes, bitcoinPrices = bitcoin });
    }

    public async Task<IActionResult> GetDataAsynchronous()
    {
        _logger.LogInformation("GetDataAsynchronous");
        EarthquakeAPI quakeSource = new EarthquakeAPI("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/2.5_day.geojson");
        IEnumerable<Earthquake> quakes  = await quakeSource.GetRecentEarthquakesAsync();
        List<ExchangeCandlestick> bitcoin = await _cryptoWatch.GetHistoricalBitcoinDataAsync();
        return Json(new { earthquakes = quakes.OrderByDescending(e => e.Magnitude), bitcoinPrices = bitcoin });
    }

    public async Task<IActionResult> GetDataAsynchronousParallel()
    {
        _logger.LogInformation("GetDataAsynchronousParallel");
        EarthquakeAPI quakeSource = new EarthquakeAPI("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/2.5_day.geojson");
        // Start both asynchronous requests, each one returns a Task object immediately
        var task1 = quakeSource.GetRecentEarthquakesAsync();
        var task2 = _cryptoWatch.GetHistoricalBitcoinDataAsync();
        // await both of them
        await Task.WhenAll(task1, task2);
        // "Lift" results out of each task
        IEnumerable<Earthquake> quakes  = task1.Result;
        List<ExchangeCandlestick> bitcoin = task2.Result;
        return Json(new { earthquakes = quakes.OrderByDescending(e => e.Magnitude), bitcoinPrices = bitcoin });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
