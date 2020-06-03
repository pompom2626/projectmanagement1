using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MvcDemo.Models
{
    public class Budget
    {
        
        public Budget() 
        {
          //  Id = Guid.NewGuid();
        }
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; } 
        public Guid ProjectId { get; set; }
        public string CreatorId { get; set; }
        public double days { get; set; }
        public double? FinishDays { get; set; }


    }
}