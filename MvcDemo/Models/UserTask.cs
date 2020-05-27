using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class UserTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("ApplicationUser")]
        public string ApplicationUser_Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("TaskHelper")]
        public Guid   TaskHelper_Id { get; set; }

        public virtual TaskHelper TaskHelper { get; set; }
    }
}