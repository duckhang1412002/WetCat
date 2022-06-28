using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



//#nullable disable

namespace WetCat.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowUsername1Navigations = new HashSet<Follow>();
            FollowUsername2Navigations = new HashSet<Follow>();
            FriendUsername1Navigations = new HashSet<Friend>();
            FriendUsername2Navigations = new HashSet<Friend>();
            HobbyLists = new HashSet<HobbyList>();
            Posts = new HashSet<Post>();
            ReactLists = new HashSet<ReactList>();
        }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserMail { get; set; }
        [Required]
        public string AvatarSrc { get; set; }
        [Required]
        public string BackgroundSrc { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowUsername1Navigations { get; set; }
        public virtual ICollection<Follow> FollowUsername2Navigations { get; set; }
        public virtual ICollection<Friend> FriendUsername1Navigations { get; set; }
        public virtual ICollection<Friend> FriendUsername2Navigations { get; set; }
        public virtual ICollection<HobbyList> HobbyLists { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ReactList> ReactLists { get; set; }
    }
}
