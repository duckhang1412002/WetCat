using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class WarningList
    {
        public int WarningId { get; set; }
        public string Username { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string WarningType { get; set; }
        public string Reason { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User UsernameNavigation { get; set; }
        public virtual Warning WarningTypeNavigation { get; set; }
    }
}
