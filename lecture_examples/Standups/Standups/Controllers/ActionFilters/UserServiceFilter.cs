using Microsoft.AspNetCore.Mvc.Filters;
using Standups.Services;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;

namespace Standups.Controllers.ActionFilters
{
    /// <summary>
    /// Here we use a custom Action Filter to (sort of) solve a problem we had with the UserService.  Previously
    /// we injected the UserService into any controller that needed to ask questions about the current user (other than 
    /// whether someone was logged in or not).  But it wasn't fully initialized, because the User property isn't set until 
    /// the action method is invoked.  So the injected service isn't ready to go until you set the User property in it.  
    /// 
    /// This filter solves this problem slightly better than forcing the developer to put _userService.User = User; at the 
    /// beginning of every action method.  Now, you only have to add this filter to any action method that needs it and in here
    /// we set that property.  Convoluted yes but a little slightly more organized.
    /// </summary>
    public class UserServiceFilter : IActionFilter
    {
        private IUserService _userService;

        // We can use DI here as long as we use this as a ServiceFilter 
        public UserServiceFilter(IUserService userService)
        {
            _userService = userService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Debug.WriteLine("-----------In action filter, before action method has begun");
            ClaimsPrincipal user = context.HttpContext.User;
            if (user != null)
            {
                // Fully initialize our user service object
                _userService.User = user;
                // now set it in the controller via reflection
                var userServiceField = context.Controller.GetType().GetField("_userService", BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.GetField);
                userServiceField?.SetValue(context.Controller,_userService, BindingFlags.SetField,null,null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Debug.WriteLine("   after execution");
        }
    }
}
