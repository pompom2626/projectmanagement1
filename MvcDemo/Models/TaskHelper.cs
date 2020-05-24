using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class TaskHelper
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string Discribtion { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public DateTime CreateTime { get; set; }
        public Nullable< DateTime> FinishTime { get; set; }
        [Required]
        public bool IsFinished { get; set; } = false;
    }
}