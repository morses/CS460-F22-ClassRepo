using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Services;
using Standups.ViewModels;
using System.Diagnostics;

namespace Standups.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;         // use this for all user related functionality
        private readonly IQuestionRepository _questionRepository;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IQuestionRepository questionRepository)
        {
            _logger = logger;
            _userService = userService;
            _questionRepository = questionRepository;
        }

        public IActionResult Index()
        {
            // And it isn't set by the time the Controller is instantiated, only when this method is called
            _userService.User = User;
            HomeVM vm = new HomeVM();

            // Registered users (that aren't admin) must select which group they are in before they can
            // create a meeting report
            vm.NeedsGroup = false;
            if (_userService.IsAuthenticated() && !_userService.IsAdmin() && _userService.CurrentUserNeedsGroupSet())
            {
                vm.NeedsGroup = true;
            }
            // Current open questions for the students to comment on
            vm.Questions = _questionRepository.GetOpenQuestions();
            vm.IsAuthenticated = _userService.IsAuthenticated();

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
}