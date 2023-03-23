using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Data;

namespace Standups.DAL.Concrete
{
    public class UserRepository : Repository<Supuser>, IUserRepository
    {
        public UserRepository(StandupsDbContext ctx) : base(ctx) { }

    }
}