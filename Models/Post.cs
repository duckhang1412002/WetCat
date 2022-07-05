using System;
using System.Collections.Generic;

#nullable disable

namespace WetCat.Models
{
    public partial class Post
    {
        public Post()
        {
            ReactLists = new HashSet<ReactList>();
        }

        public int PostId { get; set; }
        public string PrivacyMode { get; set; }
        public string PostAuthor { get; set; }
        public DateTime PostTime { get; set; }
        public string PostContent { get; set; }
        public string PostImgSrc { get; set; }

        public virtual User PostAuthorNavigation { get; set; }
        public virtual ICollection<ReactList> ReactLists { get; set; }
    }
}
