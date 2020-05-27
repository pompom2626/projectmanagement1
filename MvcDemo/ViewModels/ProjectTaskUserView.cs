using MvcDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.ViewModels
{
    public class ProjectTaskUserView
    {

        public decimal Budget { get; set; }
        public Nullable<decimal> RealBudget { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public DateTime Deadline { get; set; }
        public Nullable<DateTime> FinishedTime { get; set; }
 
        public bool IsFinished { get; set; } = false;

        [MaxLength(100)]
        public string ProjectTitle { get; set; }
 
        [DataType(DataType.Text)]
        public string ProjectContent { get; set; }
        // public Guid TaskHelper_Id { get; set; }
        public virtual ICollection<TaskHelper> TaskHelpers { get; set; }

        public int Priority { get; set; } = 1;

        public Guid CreatorId { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }
        public Guid StaffId { get; set; }
        //  public Staff Staff { get; set; }

        public Guid ProjectTask_Id { get; set; }
        public Project Project { get; set; }

        public DateTime TaskDeadline { get; set; }
        public DateTime TaskCreateTime { get; set; }
        public Nullable<DateTime> FinishTime { get; set; }
   
        public bool TaskIsFinished { get; set; } = false;
        public int TaskPriority { get; set; } = 1;
        public int Status { get; set; }
        public Guid TaskCreatorId { get; set; }
        public string ApplicationUser_Id { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }

    }
}