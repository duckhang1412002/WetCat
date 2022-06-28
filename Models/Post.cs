using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WetCat.Models
{
    public partial class Post
    {
        public Post()
        {
            ReactLists = new List<ReactList>();
        }

        [Key]
        [Required]
        public int PostId { get; set; }

        [Required]
        public string PrivacyMode { get; set; }

        [Required]
        public string PostAuthor { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        [Required(ErrorMessage = "Content must be filled!")]
        [RegularExpression(@"^\S{1}.{0,1499}$", ErrorMessage = "Content must be filled! The length do not exceed 1500 letters!")]
        public string Content { get; set; }
        
        [RegularExpression(@"(([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg|.gif)$)|(^$)", ErrorMessage = "Only post image file allowed(.jpg/.jpeg/.png.gif)!")]
        public string MediaSrc { get; set; }

        public virtual User PostAuthorNavigation { get; set; }
        public virtual ICollection<ReactList> ReactLists { get; set; }
    }
}