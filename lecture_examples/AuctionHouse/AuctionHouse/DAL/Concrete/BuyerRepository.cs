using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.Models;

// Put this in folder DAL/Concrete
namespace AuctionHouse.DAL.Concrete;

public class BuyerRepository : IBuyerRepository
{
    private DbSet<Buyer> _buyers;

    public BuyerRepository(AuctionHouseDbContext context)
    {
        _buyers = context.Buyers;
    }

    public List<Buyer> Buyers()
    {
        return _buyers.ToList();
    }

    public int NumberOfBuyers()
    {
        return _buyers.Count();
    }

    public List<string> EmailList()
    {
        return _buyers.Select(b => b.Email).ToList();
    }

}