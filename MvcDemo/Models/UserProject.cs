using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class UserProject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("ApplicationUser")]
        public string ApplicationUser_Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Project")]
        public Guid Project_Id { get; set; }
        
        public virtual Project Project { get; set; }
    }
}