using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPGroups")]
public partial class Supgroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Motto { get; set; }

    [Column("SUPAdvisorID")]
    public int? SupadvisorId { get; set; }

    [Column("SUPAcademicYearID")]
    public int SupacademicYearId { get; set; }

    [ForeignKey("SupacademicYearId")]
    [InverseProperty("Supgroups")]
    public virtual SupacademicYear SupacademicYear { get; set; }

    [ForeignKey("SupadvisorId")]
    [InverseProperty("Supgroups")]
    public virtual Supadvisor Supadvisor { get; set; }

    [InverseProperty("Supgroup")]
    public virtual ICollection<Supuser> Supusers { get; } = new List<Supuser>();
}
