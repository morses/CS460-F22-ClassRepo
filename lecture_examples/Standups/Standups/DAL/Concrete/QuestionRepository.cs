using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Data;
using Standups.Models.DTO;

namespace Standups.DAL.Concrete
{
    public class QuestionRepository : Repository<Supquestion>, IQuestionRepository
    {
        public QuestionRepository(StandupsDbContext ctx) : base(ctx) { }

        public List<Supquestion> GetOpenQuestions()
        {
            return GetAll(q => q.Supcomments).Where(q => q.Active > 0).ToList();
        }

        public IQueryable<Supcomment> GetCommentsFor(int questionID)
        {
            /*
             return db.SUPQuestions
                     .Where(q => q.ID == questionID)
                     .FirstOrDefault()
                     .SUPComments
                     .AsQueryable();
             */
            throw new NotImplementedException();
        }

        public List<CommentData> GetCommentRatingDataForQuestionAndUser(Supquestion supquestion, Supuser user)
        {
            if(!Exists(supquestion.Id) || user == null)
            {
                return new List<CommentData>();
            }
            else
            {
                List<CommentData> data = supquestion.Supcomments
                    .Where(c => c.AdvisorRating > 0)
                    .Select(c => new CommentData
                    {
                        Id = c.Id,
                        TimeStamp = c.SubmissionDate,
                        UserRating = c.SupcommentRatings.OrderByDescending(cr => cr.RatingDate)
                                                        .FirstOrDefault(cr => cr.SupraterUserId == (user?.Id ?? -1))
                                                        ?.RatingValue ?? 0,
                        Likes = c.SupcommentRatings.Where(cr => cr.RatingValue > 0).Count(),
                        Dislikes = c.SupcommentRatings.Where(cr => cr.RatingValue < 0).Count(),
                        Comment = c.Comment
                    })
                    .ToList();
                return data;
            }
            
        }

        public Supquestion GetById(int id)
        {
            return GetAll(q => q.Supcomments).Where(q => q.Id == id).FirstOrDefault();
        }
    }
}