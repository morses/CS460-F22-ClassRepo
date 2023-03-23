using Standups.Models;
using Standups.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups.DAL.Abstract
{
    public interface IQuestionRepository : IRepository<Supquestion>
    {
        List<Supquestion> GetOpenQuestions();
        IQueryable<Supcomment> GetCommentsFor(int questionID);

        List<CommentData> GetCommentRatingDataForQuestionAndUser(Supquestion supquestion, Supuser user);
        Supquestion GetById(int id);

    }
}
