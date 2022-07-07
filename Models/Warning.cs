using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Warning
    {
        public Warning()
        {
            WarningLists = new List<WarningList>();
        }

        public string WarningType { get; set; }
        public string WarningName { get; set; }

        public virtual ICollection<WarningList> WarningLists { get; set; }
    }
}
