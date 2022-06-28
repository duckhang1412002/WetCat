using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationLists = new HashSet<NotificationList>();
            WarningLists = new HashSet<WarningList>();
        }

        public string NotificationType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NotificationList> NotificationLists { get; set; }
        public virtual ICollection<WarningList> WarningLists { get; set; }
    }
}
