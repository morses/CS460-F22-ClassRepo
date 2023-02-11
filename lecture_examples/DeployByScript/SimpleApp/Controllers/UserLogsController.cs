using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleApp.DAL.Abstract;
using SimpleApp.Data;
using SimpleApp.Models;

namespace SimpleApp.Controllers
{
    public class UserLogsController : Controller
    {
        private readonly IRepository<UserLog> _userLoggerRepository;

        public UserLogsController(IRepository<UserLog> userLoggerRepository)
        {
            _userLoggerRepository = userLoggerRepository;
        }

        // GET: UserLogs
        public IActionResult Index()
        {
              return View(_userLoggerRepository.GetAll().ToList());
        }

    }
}
