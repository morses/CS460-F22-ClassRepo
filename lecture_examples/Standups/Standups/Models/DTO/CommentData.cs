namespace Standups.Models.DTO
{
    public class CommentData
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UserRating { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Comment { get; set; }
    }
}
