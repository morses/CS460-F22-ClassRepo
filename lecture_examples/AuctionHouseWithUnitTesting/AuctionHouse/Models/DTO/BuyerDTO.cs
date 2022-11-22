namespace AuctionHouse.Models.DTO
{
    public class BuyerDTO
    {
        // It's easiest to just put Buyer here, and it's generally fine, but to have more fine grained control
        // we'd probably want to put Id, Name, etc. right here in this class rather than having a Buyer.  See below
        public Buyer Buyer { get; set; }
        public int NumberOfBids { get; set; }

        public BuyerDTO(Buyer buyer, int numberOfBids)
        {
            Buyer = buyer;
            NumberOfBids = numberOfBids;
        }   
    }
}

/*
 If we don't need or want the bids, or don't want to have to worry about handling cycles, we could simply do the following.
This does suffer from violating DRY (Don't Repeat Yourself) but we'd be doing much of that already in a view model.  With API's we 
want to control precisely the data that goes out, and it frequently shouldn't be everything in that database table.
public class BuyerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int NumberOfBids { get; set; }

    public BuyerDTO(Buyer buyer, int numberOfBids)
    {
        Id = buyer.Id;
        FirstName = buyer.FirstName;
        LastName = buyer.LastName;
        Email = buyer.Email;
        NumberOfBids = numberOfBids;
    }   
    }
 */
