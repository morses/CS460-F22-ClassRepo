using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimpleAjaxExample.Models
{
    [Table("Person")]
    public partial class Person
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int Age { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime Anniversary { get; set; }
    }
}
