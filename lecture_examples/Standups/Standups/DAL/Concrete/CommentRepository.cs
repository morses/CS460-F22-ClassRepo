using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Data;

namespace Standups.DAL.Concrete
{
    public class CommentRepository : Repository<Supcomment>, ICommentRepository
    {
        public CommentRepository(StandupsDbContext ctx) : base(ctx) { }

        

    }
}