using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace WetCat.Models
{
    public partial class Post
    {
        public Post()
        {
            ReactLists = new List<ReactList>();
        }

        public Post(string PrivacyMode, string PostAuthor, DateTime PostTime, string PostContent, string PostImgSrc){
            this.PrivacyMode = PrivacyMode;
            this.PostAuthor = PostAuthor;
            this.PostTime = PostTime;
            this.PostContent = PostContent;
            this.PostImgSrc = PostImgSrc;
        }

        [Key]
        public int PostId { get; set; }

        [Required]
        public string PrivacyMode { get; set; }

        [Required]
        public string PostAuthor { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        [Required(ErrorMessage = "Content must be filled!")]
        [RegularExpression(@"^\S{1}.{0,1499}$", ErrorMessage = "Content must be filled! The length do not exceed 1500 letters!")]
        public string PostContent { get; set; }

        [RegularExpression(@"(([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg|.gif)$)|(^$)", ErrorMessage = "Only post image file allowed(.jpg/.jpeg/.png.gif)!")]
        public string PostImgSrc { get; set; }

        public virtual User PostAuthorNavigation { get; set; }
        public virtual ICollection<ReactList> ReactLists { get; set; }
    }
}
