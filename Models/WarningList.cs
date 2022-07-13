using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WetCat.Models
{
    public partial class WarningList
    {
        [Key]
        [Required(ErrorMessage = "Warning ID can not be blank!")]
        [Display(Name = "Warning ID")]
        public int WarningId { get; set; }

        [Required(ErrorMessage = "User name can not be blank!")]
        [Display(Name = "Username")]
        [StringLength(20)]
        public string Username { get; set; }

        [Display(Name = "Post ID")]
        public int? PostId { get; set; }

        [Display(Name = "Comment ID")]
        public int? CommentId { get; set; }

        [Required(ErrorMessage = "Warning Type can not be blank")]
        [Display(Name = "Warning Type")]
        [StringLength(20)]
        public string WarningType { get; set; }

        [Display(Name = "Reason")]
        [DataType(DataType.Text)]
        public string Reason { get; set; }

        [Display(Name = "Time Start")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeStart { get; set; }

        [Display(Name = "Time End")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeEnd { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User UsernameNavigation { get; set; }
        public virtual Warning WarningTypeNavigation { get; set; }
    }
}
