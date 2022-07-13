using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class ReactList
    {
        public string ReactType { get; set; }
        public int PostId { get; set; }
        public string Username { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Post Post { get; set; }
        public virtual React ReactTypeNavigation { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
