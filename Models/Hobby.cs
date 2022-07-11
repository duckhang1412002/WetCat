using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Hobby
    {
        public Hobby()
        {
            HobbyLists = new List<HobbyList>();
        }

        public int HobbyId { get; set; }
        public string HobbyName { get; set; }

        public virtual ICollection<HobbyList> HobbyLists { get; set; }
    }
}
