using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace WetCat.Models
{
    public partial class Follow
    {
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Username1 { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Username2 { get; set; }

        public virtual User Username1Navigation { get; set; }
        public virtual User Username2Navigation { get; set; }
    }
}
