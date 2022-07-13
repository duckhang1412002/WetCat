using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Follow
    {
        public string FollowerUsername { get; set; }
        public string FollowedUsername { get; set; }
        public int? IsDeleted { get; set; }

        public virtual User FollowedUsernameNavigation { get; set; }
        public virtual User FollowerUsernameNavigation { get; set; }
    }
}
