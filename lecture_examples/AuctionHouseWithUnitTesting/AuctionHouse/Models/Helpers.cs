namespace AuctionHouse.Models
{
    public class Helpers
    {
        public static string ExtractFirstName(string fullname)
        {
            return fullname.Trim().Split(' ').First();
        }

    }
}
