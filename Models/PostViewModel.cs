using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace WetCat.Models
{
    public partial class PostViewModel : Post
    {
        public PostViewModel(){
            this.PrivacyMode = "PrivacyMode";
            this.PostAuthor = "PostAuthor";
            this.PostTime = default;
            this.PostContent = "PostContent";
            this.PostImgFile = default;
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

        [DataType(DataType.Upload)]
        public IFormFile PostImgFile { get; set; }
    }
}
