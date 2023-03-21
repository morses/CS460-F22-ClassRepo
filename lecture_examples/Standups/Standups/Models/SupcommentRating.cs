using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPCommentRatings")]
public partial class SupcommentRating
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SUPCommentID")]
    public int SupcommentId { get; set; }

    [Column("SUPRaterUserID")]
    public int SupraterUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RatingDate { get; set; }

    public int RatingValue { get; set; }

    [ForeignKey("SupcommentId")]
    [InverseProperty("SupcommentRatings")]
    public virtual Supcomment Supcomment { get; set; }

    [ForeignKey("SupraterUserId")]
    [InverseProperty("SupcommentRatings")]
    public virtual Supuser SupraterUser { get; set; }
}
