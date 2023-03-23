using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Standups.Controllers.ActionFilters;
using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Services;
using Standups.ViewModels;
using System.Net;
using System.Runtime.CompilerServices;

namespace Standups.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRepository<Supgroup> _groupRepository;
        private readonly IRepository<Supuser> _userRepository;
        public MyAccountController(IRepository<Supgroup> groupRepository, IRepository<Supuser> userRepository) 
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        [ServiceFilter(typeof(UserServiceFilter))]
        public IActionResult Index()
        {
            _userService.User = User;
            if(_userService.IsAuthenticated() && !_userService.IsAdmin())
            {
                return View(_userService.GetCurrentSupuser());
            }
            else if(_userService.IsAdmin())
            {
                return View(new Supuser());
            }
            else
            {
                return BadRequest();
            }
        }

        [NonAction]
        private List<SelectListItem> CreateGroupsSelectList(IEnumerable<Supgroup> groups)
        {
            return groups.Select(g => new SelectListItem { Value = $"{g.Id}", Text = $"{g.Name} -- \"{g.Motto}\"" }).ToList();
        }

        [ServiceFilter(typeof(UserServiceFilter))]
        // Allow the current user to select which group they are in
        public IActionResult SelectGroup()
        {
            _userService.User = User;
            if (_userService.IsAuthenticated() && !_userService.IsAdmin())
            {
                Supuser user = _userService.GetCurrentSupuser();
                SelectGroupVM vm = new SelectGroupVM
                {
                    GroupId = $"{user.SupgroupId}",
                    Groups = CreateGroupsSelectList(_groupRepository.GetAll())
                };
                return View(vm);
            }
            else if (_userService.IsAdmin())
            {
                return View(null);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(UserServiceFilter))]
        public IActionResult SelectGroup(int? id)
        {
            _userService.User = User;
            int groupId = (int)id;
            Supuser user = _userService.GetCurrentSupuser();
            if (_groupRepository.Exists(groupId))
            {
                user.SupgroupId = groupId;
                _userRepository.AddOrUpdate(user);
                return RedirectToAction("Index", "MyAccount");
            }
            SelectGroupVM vm = new SelectGroupVM
            {
                GroupId = $"{user.SupgroupId}",
                Groups = CreateGroupsSelectList(_groupRepository.GetAll())
            };
            return View(vm);
        }
    }
}
