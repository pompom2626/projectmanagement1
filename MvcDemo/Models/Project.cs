using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
        [Required]
        public decimal Budget { get; set; }
        public Nullable<decimal> RealBudget { get; set; }
        public DateTime CreateTime { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public Nullable<DateTime> FinishedTime { get; set; }
        [Required]
        public bool IsFinished { get; set; } = false;
        [Required]
        [DataType (DataType.Text)]
        public string ProjectContent { get; set; }
        public virtual ICollection<TaskHelper> TaskHelpers { get; set; }
        [Required]
        public int Priority { get; set; } = 1;
        
    }
}