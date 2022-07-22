using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseP = new HashSet<Comment>();
            NotificationLists = new HashSet<NotificationList>();
            WarningLists = new HashSet<WarningList>();
        }

        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int? ParentId { get; set; }
        public string CommentAuthor { get; set; }
        public DateTime CommentTime { get; set; }
        public string CommentContent { get; set; }
        public int? IsDeleted { get; set; }
        public virtual User CommentAuthorNavigation { get; set; }
        public virtual Comment P { get; set; }
        public virtual ICollection<Comment> InverseP { get; set; }
        public virtual ICollection<NotificationList> NotificationLists { get; set; }
        public virtual ICollection<WarningList> WarningLists { get; set; }
    }
}
