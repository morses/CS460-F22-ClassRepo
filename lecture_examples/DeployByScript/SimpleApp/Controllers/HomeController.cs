using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleApp.Models;
using SimpleApp.DAL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SimpleApp.Areas.Identity.Data;
using System.Reflection.PortableExecutable;

namespace SimpleApp.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserLogRepository _userLogger;
    private readonly UserManager<SimpleUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IUserLogRepository userLogger, UserManager<SimpleUser> userManager)
    {
        _logger = logger;
        _userLogger = userLogger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // Get the current user's id or null
        string id = _userManager.GetUserId(User);

        // Log this visit
        UserLog lg = new UserLog
        {
            AspnetIdentityId = id,
            TimeStamp = DateTime.UtcNow,
            Ipaddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
            UserAgent = Request.Headers.UserAgent
        };
        _userLogger.AddOrUpdate(lg);

        MainPageVM vm = new MainPageVM();

        SimpleUser user = await _userManager.GetUserAsync(HttpContext.User);
        if( user != null)
        {
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.HasUser = true;
            var logs = _userLogger.MostRecentVisit(id,2);
            if( logs.Count() == 2 )
            {
                vm.SetVisitTimes(logs[1].TimeStamp, DateTime.UtcNow);
            }
        }

        return View(vm);
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
