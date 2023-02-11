using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Data;
using SimpleApp.DAL.Abstract;
using SimpleApp.Models;

// Put this in folder DAL/Concrete
namespace SimpleApp.DAL.Concrete;

public class UserLogRepository : Repository<UserLog>, IUserLogRepository
{ 
    public UserLogRepository(SimpleDbContext ctx) : base(ctx)
    {

    }

    public List<UserLog> MostRecentVisit(string aspNetUserId, int count)
    {
        var lastVisits = GetAll(ul => ul.AspnetIdentityId== aspNetUserId)
                            .OrderByDescending(ul => ul.TimeStamp)
                            .Take(count)
                            .ToList();
        return lastVisits;
    }

}