using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPAdvisors")]
public partial class Supadvisor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [InverseProperty("Supadvisor")]
    public virtual ICollection<Supgroup> Supgroups { get; } = new List<Supgroup>();
}
