using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models
{
    [Table("Item")]
    public partial class Item
    {
        public Item()
        {
            Bids = new HashSet<Bid>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(256)]
        public string Description { get; set; } = null!;
        [StringLength(512)]
        public string Condition { get; set; } = null!;
        [Column("SellerID")]
        public int? SellerId { get; set; }

        [ForeignKey("SellerId")]
        [InverseProperty("Items")]
        public virtual Seller? Seller { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
