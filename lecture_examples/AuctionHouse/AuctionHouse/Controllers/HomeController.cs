using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.Models;

namespace AuctionHouse.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuctionHouseDbContext _context;

    public HomeController(ILogger<HomeController> logger, AuctionHouseDbContext ctx)
    {
        _logger = logger;
        _context = ctx;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Buyers()
    {
        IEnumerable<Buyer> buyers = _context.Buyers.ToList();
        return View(buyers);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
