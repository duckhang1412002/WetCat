using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class NotificationList
    {
        public string Target { get; set; }
        public string Causer { get; set; }
        public int NotificationId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string NotificationType { get; set; }
        public DateTime NotifyTime { get; set; }
        public int? IsDeleted { get; set; }

        public virtual User CauserNavigation { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Notification NotificationTypeNavigation { get; set; }
        public virtual User TargetNavigation { get; set; }
    }
}
