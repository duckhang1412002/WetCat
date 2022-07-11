using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationLists = new List<NotificationList>();
        }

        public string NotificationType { get; set; }
        public string NotificationName { get; set; }

        public virtual ICollection<NotificationList> NotificationLists { get; set; }
    }
}
