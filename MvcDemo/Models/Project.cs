using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo.Models
{
    public class Project
    {
        public Project()
        {
            this.TaskHelpers = new HashSet<TaskHelper>();
        }
      

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
      //  public Guid ManagerId { get; set; }
    //    public virtual Manager Manager { get; set; }
        [Required]
        public decimal Budget { get; set; }
        public Nullable<decimal> RealBudget { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Required]
        public DateTime Deadline { get; set; }
        public Nullable<DateTime> FinishedTime { get; set; }
        [Required]
        public bool IsFinished { get; set; } = false;
        [Required]
        [MaxLength(100)]
        public string ProjectTitle { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ProjectContent { get; set; }
       // public Guid TaskHelper_Id { get; set; }
        public virtual ICollection<TaskHelper> TaskHelpers { get; set; }
        [Required]
        public int Priority { get; set; } = 1;

        public Guid CreatorId { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
       // public virtual Notification Notification { get; set; }
        

        //public string ApplicationUser_Id { get; set; }
        //[ForeignKey("ApplicationUser_Id")]

        //public virtual ApplicationUser ApplicationUser { get; set; }


    }
}