using System.ComponentModel.DataAnnotations;

namespace Standups.Models.DTO
{
    public class CommentAdmin
    {
        public CommentAdmin() { }
        public CommentAdmin(Supcomment c)
        {
            Id = c.Id;
            QuestionId = c.SupquestionId;
            Question = c.Supquestion.Question;
            SubmissionDate = c.SubmissionDate;
            Comment = c.Comment;
            AdvisorRating = c.AdvisorRating;
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public string Question { get; set; }
        [Required]
        public DateTime SubmissionDate { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public int AdvisorRating { get; set; }

        public bool Matches(Supcomment comment)
        {
            return Id == comment.Id
                   && QuestionId == comment.SupquestionId
                   && SubmissionDate == comment.SubmissionDate
                   && Comment == comment.Comment;
        }
    }
}
