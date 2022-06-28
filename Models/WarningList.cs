using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class WarningList
    {
        public int PunishmentId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string NotificationType { get; set; }
        public string Reason { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Notification NotificationTypeNavigation { get; set; }
    }
}
