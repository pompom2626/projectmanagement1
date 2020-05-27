using MvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo.ViewModels
{
    public class UPTNotification
    {
        public ApplicationUser ApplicationUsers { get; set; }
        public Project Projects { get; set; }
        public TaskHelper TaskHelper { get; set; }
        public Notification Notifications { get; set; }
    }
}