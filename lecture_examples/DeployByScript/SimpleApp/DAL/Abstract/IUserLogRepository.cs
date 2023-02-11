using SimpleApp.Models;

namespace SimpleApp.DAL.Abstract;

public interface IUserLogRepository : IRepository<UserLog>
{
    List<UserLog> MostRecentVisit(string aspNetUserId, int count);
}