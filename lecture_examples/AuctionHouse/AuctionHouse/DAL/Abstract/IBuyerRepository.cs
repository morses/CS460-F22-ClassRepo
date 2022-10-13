using AuctionHouse.Models;

// Here is a starting point to help you use a good testable design for your application logic

// Put this in a DAL/Abstract folder (DAL stands for Data Access Layer)
namespace AuctionHouse.DAL.Abstract;

public interface IBuyerRepository
{

    List<Buyer> Buyers();
    
    int NumberOfBuyers();

    List<string> EmailList();

}