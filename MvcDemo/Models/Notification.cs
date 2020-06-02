using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class Notification
    {
       public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required,MaxLength(500)]
        public string Content { get; set; }
        public string ApplicationUserId { get; set; }//Sender
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public bool IsChecked { get; set; } = false;
        public bool IsBeyondDeadline { get; set; } = false;
      //  [ForeignKey("Project")]
        public Guid DeadlineProjectId { get; set; }
        public  virtual Project Project { get; set; }
      //  [ForeignKey("TaskHelper")]
        public Guid DeadlineTaskHelperId { get; set; }
        public virtual TaskHelper TaskHelper { get; set; }
  //       public Nullable<Guid> Task { get; set; }
    }
}