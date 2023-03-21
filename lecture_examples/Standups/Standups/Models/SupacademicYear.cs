using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPAcademicYears")]
public partial class SupacademicYear
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(9)]
    [Unicode(false)]
    public string Year { get; set; }

    [InverseProperty("SupacademicYear")]
    public virtual ICollection<Supgroup> Supgroups { get; } = new List<Supgroup>();
}
