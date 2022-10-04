using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models
{
    [Table("Buyer")]
    public partial class Buyer
    {
        public Buyer()
        {
            Bids = new HashSet<Bid>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [InverseProperty("Buyer")]
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
