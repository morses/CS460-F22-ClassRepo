using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standups.Controllers.ActionFilters;
using Standups.DAL.Abstract;
using Standups.DAL.Concrete;
using Standups.Models;
using Standups.Models.DTO;
using System.ComponentModel.Design;
using System.Data;

namespace Standups.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly IRepository<Supcomment> _supcommentRepository;

        public CommentsApiController(IRepository<Supcomment> supcommentRepository) 
        {
            _supcommentRepository = supcommentRepository;
        }

        // GET api/comments?status={all,new,approved,rejected}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<CommentAdmin>> Comments(string status)
        {
            if (status == null)
                return Problem(detail: "status is a required parameter", statusCode: 400, title: "Bad Request");
            IQueryable<Supcomment> result = null;
            switch(status.ToLower())
            {
                case "all":
                    result = _supcommentRepository.GetAll();
                    break;
                case "new":
                    result = _supcommentRepository.GetAll().Where(c => c.AdvisorRating == 0);
                    break;
                case "approved":
                    result = _supcommentRepository.GetAll().Where(c => c.AdvisorRating == 1);
                    break;
                case "rejected":
                    result = _supcommentRepository.GetAll().Where(c => c.AdvisorRating == -1);
                    break;
                default:
                    return Problem(detail: "Allowed values for status parameter are: all, new, approved, rejected", statusCode: 400, title: "Bad Request");
            }
            return result.OrderBy(c => c.SupquestionId)
                         .ThenBy(c => c.SubmissionDate)
                         .Select(c => new CommentAdmin(c))
                         .ToList();
        }


        // PATCH: api/comments/4
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult ChangeAdvisorRating(int id, [FromBody] CommentAdmin comment)
        {
            // We could easily do a general Update operation here, but to practice good security principles
            // we'll limit access to only what is needed.  So we'll only update the advisor rating and nothing else
            // If a later feature needs more then we'll add that later
            if (id == 0 || comment == null || id != comment.Id)
            {
                return BadRequest();
            }

            // Retrieve this comment from the db
            Supcomment actualComment = _supcommentRepository.FindById(id);
            if(actualComment == null)
            {
                return StatusCode(422);
            }
            // Add a layer of security on this.  If we didn't do this then an attacker with 1 point of access
            // (to this endpoint) could change advisor ratings for every comment in the db.  By adding this layer
            // that same attacker would also have to have read access to comments, meaning an additional point of
            // access.  This is also where we could test an anti-forgery token
            if(!comment.Matches(actualComment))
            {
                return BadRequest();
            }

            // Now make the change
            actualComment.AdvisorRating = comment.AdvisorRating;
            // TODO: add error handling to our use of generic repository
            _supcommentRepository.AddOrUpdate(actualComment);
            return Ok();
        }

    }
}
