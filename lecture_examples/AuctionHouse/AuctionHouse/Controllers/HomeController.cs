using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.Models;
using AuctionHouse.DAL.Abstract;

namespace AuctionHouse.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private readonly AuctionHouseDbContext _context;
    private IBuyerRepository _buyerRepo;

    //public HomeController(ILogger<HomeController> logger, AuctionHouseDbContext ctx)
    public HomeController(ILogger<HomeController> logger, IBuyerRepository buyerRepo)
    {
        _logger = logger;
        //_context = ctx;
        _buyerRepo = buyerRepo;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Buyers()
    {
        //IEnumerable<Buyer> buyers = _context.Buyers.ToList();
        int count = _buyerRepo.NumberOfBuyers();
        IEnumerable<Buyer> buyers = _buyerRepo.Buyers();
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
