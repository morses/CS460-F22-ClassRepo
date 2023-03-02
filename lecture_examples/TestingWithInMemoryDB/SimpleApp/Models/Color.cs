using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimpleApp.Models;

[Table("Color")]
public partial class Color
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string HexValue { get; set; }

    [InverseProperty("Color")]
    public virtual ICollection<UserLog> UserLogs { get; } = new List<UserLog>();
}
