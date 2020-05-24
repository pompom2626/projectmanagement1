using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class NotificationModel
    {
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Max length must less than {0}")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required, MaxLength(500,ErrorMessage ="Max length must less than {0}")]
        [DataType(DataType.Text)]
        public string Content { get; set; }
    }
}