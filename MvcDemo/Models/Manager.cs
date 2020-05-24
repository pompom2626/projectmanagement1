using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class Manager
    {
        public Manager()
        {
            this.Users = new HashSet<User>();
            this.Projects =new HashSet<Project>();
            this.Notifications = new HashSet<Notification>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        public Nullable<int> Phone { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}