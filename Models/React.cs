using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class React
    {
        public React()
        {
            ReactLists = new List<ReactList>();
        }

        public string ReactType { get; set; }
        public string ReactName { get; set; }

        public virtual ICollection<ReactList> ReactLists { get; set; }
    }
}
