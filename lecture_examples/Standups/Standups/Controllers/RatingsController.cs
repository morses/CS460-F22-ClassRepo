using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Standups.Controllers.ActionFilters;
using Standups.DAL.Abstract;
using Standups.Models;
using Standups.Models.DTO;
using Standups.Services;

namespace Standups.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        public readonly IUserService _userService;
        private readonly ICommentRepository _commentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRepository<SupcommentRating> _commentRatingRepository;
        public RatingsController(ICommentRepository commentRepository, IQuestionRepository questionRepository, IRepository<SupcommentRating> commentRatingRepository)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _commentRatingRepository = commentRatingRepository;   
        }

        // GET api/Ratings/4
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(UserServiceFilter))]
        public ActionResult<List<CommentData>> Comments(int id)
        {
            Supquestion q = _questionRepository.GetById(id);
            if (q != null && q.Active == 1)
            {
                _userService.User = User;
                Supuser user = _userService.GetCurrentSupuser();

                List<CommentData> data = _questionRepository.GetCommentRatingDataForQuestionAndUser(q, user);
                return Ok(data);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/Ratings
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(UserServiceFilter))]
        public ActionResult MakeRating([FromForm]Rating rating)
        {
            string statusValue = "NOT OK";
            int ratingValue = 0;
            if (rating.Action == "UP")
            {
                ratingValue = 1;
            }
            else if (rating.Action == "DOWN")
            {
                ratingValue = -1;
            }

            _userService.User = User;
            Supuser user = _userService.GetCurrentSupuser();
            if (user != null)
            {
                try
                {
                    // Not a great design to save a new one each time.  Someone who clicks the buttons incessantly would
                    // fill up rows in our table.  Better to update the existing one with a new rating and date, unless
                    // we really wanted to track when someone made ratings over time.  Not hard to change over.
                    SupcommentRating newRating = new SupcommentRating
                    {
                        SupraterUserId = user.Id,
                        SupcommentId = rating.CommentId,
                        RatingValue = ratingValue,
                        RatingDate = DateTime.UtcNow
                    };
                    _commentRatingRepository.AddOrUpdate(newRating);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                statusValue = "OK";
            }

            var data = new { status = statusValue };  // legacy code here, JS expects to read a status inside a returned JSON object (should just use the 200 response)
            return Ok(data);
        }
    }
}
