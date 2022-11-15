using System;
using System.Collections.Generic;

namespace AuctionHouse.Models
{
    public partial class Seller
    {
        // Utilizing the features of a partial class to add this method into the
        // auto generated Seller model class
        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}