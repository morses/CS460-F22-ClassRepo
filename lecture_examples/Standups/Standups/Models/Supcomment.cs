using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPComments")]
public partial class Supcomment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SUPQuestionID")]
    public int SupquestionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmissionDate { get; set; }

    [Required]
    [StringLength(1000)]
    public string Comment { get; set; }

    public int AdvisorRating { get; set; }

    [InverseProperty("Supcomment")]
    public virtual ICollection<SupcommentRating> SupcommentRatings { get; } = new List<SupcommentRating>();

    [ForeignKey("SupquestionId")]
    [InverseProperty("Supcomments")]
    public virtual Supquestion Supquestion { get; set; }
}
