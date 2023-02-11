using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimpleApp.Models;

[Table("UserLog")]
public partial class UserLog
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeStamp { get; set; }

    [Required]
    [Column("IPAddress")]
    [StringLength(45)]
    [Unicode(false)]
    public string Ipaddress { get; set; }

    [StringLength(150)]
    public string UserAgent { get; set; }

    [Column("ASPNetIdentityId")]
    [StringLength(450)]
    public string AspnetIdentityId { get; set; }
}
