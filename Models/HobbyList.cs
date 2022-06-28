using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WetCat.Models
{
    public partial class HobbyList
    {
        [Required]
        public int HobbyId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Username { get; set; }

        public virtual Hobby Hobby { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
