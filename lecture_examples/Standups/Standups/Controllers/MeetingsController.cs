using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standups.DAL.Abstract;
using Standups.Data;
using Standups.Models;
using Standups.Services;
using Standups.ViewModels;

namespace Standups.Controllers
{
    public class MeetingsController : Controller
    {
        public readonly IUserService _userService;
        public readonly IRepository<Supmeeting> _meetingRepository;
        public readonly IRepository<Supuser> _userRepository;

        public MeetingsController(IUserService userService, IRepository<Supmeeting> meetingRepository, IRepository<Supuser> userRepository)
        {
            _userService = userService;
            _meetingRepository = meetingRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            _userService.User = User;
            Supuser user = _userService.GetCurrentSupuser();
            
            // Get all meetings for this user
            List<Supmeeting> meetings = user.Supmeetings.OrderByDescending(m => m.SubmissionDate).ToList();

            MeetingsVM vm = new MeetingsVM
            {
                CurrentUser = user,
                Meetings = meetings
            };

            return View(vm);
        }

        public IActionResult Details(int? id)
        {
            _userService.User = User;
            if (id == null)
            {
                return NotFound();
            }
            int mid = (int)id;
            Supuser user = _userService.GetCurrentSupuser();
            Supmeeting meeting = user.Supmeetings.Where(m => m.Id == mid).FirstOrDefault();

            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        public IActionResult Create()
        {
            _userService.User = User;
            Supuser user = _userService.GetCurrentSupuser();

            Supmeeting mtg = new Supmeeting { Supuser = user };
            return View(mtg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Completed,Planning,Obstacles")] Supmeeting supmeeting)
        {
            _userService.User = User;
            Supuser user = _userService.GetCurrentSupuser();
            if (ModelState.IsValid)
            {
                supmeeting.Supuser = user;
                supmeeting.SupuserId = user.Id;
                supmeeting.SubmissionDate = DateTime.UtcNow;
                _meetingRepository.AddOrUpdate(supmeeting);

                return RedirectToAction(nameof(Index));
            }
            Supmeeting mtg = new Supmeeting 
            { 
                Supuser = user, 
                Completed = supmeeting.Completed, 
                Planning = supmeeting.Planning, 
                Obstacles = supmeeting.Obstacles 
            };
            return View(mtg);
        }
    }
}
