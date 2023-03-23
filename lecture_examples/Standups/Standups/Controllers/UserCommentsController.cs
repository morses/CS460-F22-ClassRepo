using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Standups.DAL.Abstract;
using Standups.DAL.Concrete;
using Standups.Models;

namespace Standups.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICommentRepository _commentRepository;

        public UserCommentsController(IQuestionRepository questionRepository, ICommentRepository commentRepository)
        {
            _questionRepository = questionRepository;
            _commentRepository = commentRepository; 
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Supquestion q = _questionRepository.GetById((int)id);

            if (q == null || q.Active < 1)
            {
                // Show them a page that says this question isn't open any more
                return RedirectToAction("QuestionUnavailable");
            }
            Supcomment comment = new Supcomment
            {
                SupquestionId = id.Value,
                Supquestion = q
            };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int? id, [Bind("SupquestionId,Comment")] Supcomment cmt)
        {
            if (ModelState.IsValid)
            {
                cmt.SubmissionDate = DateTime.UtcNow;
                // new comments have an advisor rating of 0 to indicate they have not yet been reviewed for suitability to post
                cmt.AdvisorRating = 0;
                _commentRepository.AddOrUpdate(cmt);
                return RedirectToAction("Thanks", new { questionID = cmt.SupquestionId });
            }
            if (id == null)
            {
                return NotFound();
            }
            Supquestion q = _questionRepository.GetById((int)id);

            if (q == null || q.Active < 1)
            {
                // Show them a page that says this question isn't open any more
                return RedirectToAction("QuestionUnavailable");
            }
            Supcomment c = new Supcomment
            {
                SupquestionId = id.Value,
                Supquestion = q,
                Comment = cmt.Comment
            };
            return View(c);
        }

        public IActionResult QuestionUnavailable()
        {
            return View();
        }

        public IActionResult Thanks(int? questionID)
        {
            if (questionID == null)
            {
                return NotFound();
            }
            Supquestion q = _questionRepository.GetById(questionID.Value);
            if (q == null || q.Active < 1)
            {
                // Show them a page that says this question isn't open any more
                return RedirectToAction("QuestionUnavailable");
            }
            return View(q);
        }

        public IActionResult Rate(int? id)
        {
            List<Supquestion> questions = _questionRepository.GetOpenQuestions();
            return View(questions);
        }

        
    }
}
