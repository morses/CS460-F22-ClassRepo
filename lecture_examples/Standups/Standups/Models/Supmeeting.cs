using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPMeetings")]
public partial class Supmeeting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmissionDate { get; set; }

    [Column("SUPUserID")]
    public int SupuserId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Completed { get; set; }

    [Required]
    [StringLength(1000)]
    public string Planning { get; set; }

    [Required]
    [StringLength(1000)]
    public string Obstacles { get; set; }

    [ForeignKey("SupuserId")]
    [InverseProperty("Supmeetings")]
    public virtual Supuser Supuser { get; set; }
}
