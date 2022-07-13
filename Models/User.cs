using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowFollowedUsernameNavigations = new HashSet<Follow>();
            FollowFollowerUsernameNavigations = new HashSet<Follow>();
            FriendFirstUsernameNavigations = new HashSet<Friend>();
            FriendSecondUsernameNavigations = new HashSet<Friend>();
            HobbyLists = new HashSet<HobbyList>();
            Posts = new HashSet<Post>();
            ReactLists = new HashSet<ReactList>();
            WarningLists = new HashSet<WarningList>();
        }

        public string Username { get; set; }
        public string UserMail { get; set; }
        public string AvatarSrc { get; set; }
        public string BackgroundSrc { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Nickname { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public string Quote { get; set; }
        public int? IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowFollowedUsernameNavigations { get; set; }
        public virtual ICollection<Follow> FollowFollowerUsernameNavigations { get; set; }
        public virtual ICollection<Friend> FriendFirstUsernameNavigations { get; set; }
        public virtual ICollection<Friend> FriendSecondUsernameNavigations { get; set; }
        public virtual ICollection<HobbyList> HobbyLists { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ReactList> ReactLists { get; set; }
        public virtual ICollection<WarningList> WarningLists { get; set; }
    }
}
