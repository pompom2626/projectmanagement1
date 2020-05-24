using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required,MaxLength(500)]
        public string Content { get; set; }
        public Guid UserId { get; set; }//Sender
        public User User { get; set; }
        //public bool IsChecked { get; set; }
    }
}