using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Standups.Models;

[Table("SUPUsers")]
public partial class Supuser
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

    [Column("SUPGroupID")]
    public int? SupgroupId { get; set; }

    [Required]
    [Column("ASPNetIdentityID")]
    [StringLength(128)]
    public string AspnetIdentityId { get; set; }

    [InverseProperty("SupraterUser")]
    public virtual ICollection<SupcommentRating> SupcommentRatings { get; } = new List<SupcommentRating>();

    [ForeignKey("SupgroupId")]
    [InverseProperty("Supusers")]
    public virtual Supgroup Supgroup { get; set; }

    [InverseProperty("Supuser")]
    public virtual ICollection<Supmeeting> Supmeetings { get; } = new List<Supmeeting>();
}
