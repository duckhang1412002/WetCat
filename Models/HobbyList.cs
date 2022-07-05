using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class HobbyList
    {
        public int HobbyId { get; set; }
        public string Username { get; set; }

        public virtual Hobby Hobby { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
