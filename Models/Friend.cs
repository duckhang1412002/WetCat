using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Friend
    {
        public string FirstUsername { get; set; }
        public string SecondUsername { get; set; }
        public int? FriendStatus { get; set; }
        public DateTime? StatusTime { get; set; }
        public int? IsDeleted { get; set; }

        public virtual User FirstUsernameNavigation { get; set; }
        public virtual User SecondUsernameNavigation { get; set; }
    }
}
