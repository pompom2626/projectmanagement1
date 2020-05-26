using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class TaskHelper
    {
        //public TaskHelper()
        //{
        //    this.Project = new HashSet<Project>();
        //}
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        public Guid StaffId { get; set; }
      //  public Staff Staff { get; set; }
        public Guid Project_Id { get; set; }
        public Project Project { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public DateTime CreateTime { get; set; }
        public Nullable<DateTime> FinishTime { get; set; }
        [Required]
        public bool IsFinished { get; set; } = false;
        public int Priority { get; set; } = 1;
        public int Status { get; set; }
        public string ApplicationUser_Id { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}