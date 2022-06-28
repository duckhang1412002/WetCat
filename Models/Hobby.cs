using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WetCat.Models
{
    public partial class Hobby
    {
        
        public Hobby()
        {
            HobbyLists = new HashSet<HobbyList>();
        }
        [Required]
        public int HobbyId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string HobbyName { get; set; }

        public virtual ICollection<HobbyList> HobbyLists { get; set; }
    }
}
