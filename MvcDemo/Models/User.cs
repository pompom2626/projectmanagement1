using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class User
    {
        public User()
        {
            TaskHelpers = new HashSet<TaskHelper>();
            Notifications = new HashSet<Notification>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6), MaxLength(20)]
        public string Password { get; set; }
        public Nullable<int> Phone { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TaskHelper> TaskHelpers { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}