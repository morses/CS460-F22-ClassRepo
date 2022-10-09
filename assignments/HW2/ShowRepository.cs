using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HW2.DAL.Abstract;
using HW2.Models;

// Put this in folder DAL/Concrete
namespace HW2.DAL.Concrete;

public class ShowRepository : IShowRepository
{
    private DbSet<Show> _shows;

    public ShowRepository(StreamingDbContext context)
    {
        _shows = context.Shows;
    }

    public (int show, int movie, int tv) NumberOfShowsByType()
    {
        // Your implementation here
        return (0,0,0); // this is a value tuple type, new in C# 7.0 (we're now at C# 10)
    }

    public Show HighestTMDBPopularity()
    {
        return null;
    }

    public Show MostIMDBVotes()
    {
        return null;
    }

    public Show Oldest()
    {
        return null;
    }

    public Show Newest()
    {
        return null;
    }

}