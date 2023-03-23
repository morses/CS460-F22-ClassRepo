using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Standups.DAL.Abstract;
using Standups.Models;
using System.Security.Claims;

namespace Standups.Services
{
    // We want shared behavior that uses these resources that are provided by dependency injection.
    // But we don't want to add item after item to our constructors to request resources.  To simplify we use
    // an aggregate pattern, or a "Dependency Aggregate".  This gives us the benefit of placing all this
    // common functionality in one easy reusable place and improves testability

    // This one isn't perfect as we need the User object from a controller, which must be set manually.
    public interface IUserService
    {
        UserManager<IdentityUser> UserManager { get; }
        IRepository<Supuser> UserRepository { get; }

        ClaimsPrincipal User { set; }

        Supuser GetCurrentSupuser();

        string GetCurrentIdentityID();

        bool IsAuthenticated();

        bool IsAdmin();

        bool CurrentUserNeedsGroupSet();

    }
    public class UserService : IUserService
    {
        /// <summary>
        /// After creation you must manually set the User object (ClaimsPrincipal) using the User
        /// from the BaseController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userRepository"></param>
        public UserService(UserManager<IdentityUser> userManager, IRepository<Supuser> userRepository)
        {
            UserManager = userManager;
            UserRepository = userRepository;
        }

        public UserManager<IdentityUser> UserManager { get; set; }

        public IRepository<Supuser> UserRepository { get; set; }

        // You must manually set this
        public ClaimsPrincipal User { set; private get; } = null;

        /* Source code for Identity, useful for looking things up: https://github.com/dotnet/aspnetcore/tree/main/src/Identity/Extensions.Core/src */

        /// <summary>
        /// Get the SUPUser object corresponding to the currently logged in user
        /// </summary>
        /// <returns>The currently logged in user as a Supuser or null if not logged 
        /// in or no user exists in the database that has that Identity ID</returns>
        public Supuser GetCurrentSupuser()
        {
            string id = GetCurrentIdentityID();
            if (id == null)
            {
                return null;
            }
            return UserRepository.GetAll(u => u.Supgroup,u => u.Supmeetings).FirstOrDefault(u => u.AspnetIdentityId == id);
        }

        /// <summary>
        /// Get the ASPNetIdentityId for the current user, if available.  This only pulls it out of the Request (ClaimTypes.NameIdentifier)
        /// and does not hit the DB
        /// </summary>
        /// <returns>The id value or null if not present</returns>
        public string GetCurrentIdentityID()
        {
            return UserManager.GetUserId(User);
        }

        /// <summary>
        /// Helper method to access Identity to see if the current request comes from
        /// a logged in user.
        /// </summary>
        /// <returns>true if the user is authenticated, false otherwise</returns>
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        public bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        public bool CurrentUserNeedsGroupSet()
        {
            Supuser user = GetCurrentSupuser();
            if (user == null || !user.HasGroup())
            {
                return true;
            }
            return false;
        }

        // Anything else that's shared amongst Controllers ...

    }
}
