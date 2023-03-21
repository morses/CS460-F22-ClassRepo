using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPQuestions")]
public partial class Supquestion
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmissionDate { get; set; }

    [Required]
    [StringLength(1000)]
    public string Question { get; set; }

    public int Active { get; set; }

    [InverseProperty("Supquestion")]
    public virtual ICollection<Supcomment> Supcomments { get; } = new List<Supcomment>();
}
