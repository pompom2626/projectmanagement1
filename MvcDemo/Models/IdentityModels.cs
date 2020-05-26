using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
//using ADKZProject.Models; //for dolphin's namespace
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MvcDemo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            // this.Projects = new HashSet<Project>();
        }
        [Display(Name = "Full"), Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public Guid Project_Id { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public Guid TaskHelper_Id { get; set; }
        public virtual ICollection<TaskHelper> TaskHelpers { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        { }
        public ApplicationRole(string roleName) : base(roleName)
        {

        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //    public DbSet<Manager> Managers { get; set; }
        // public DbSet<ManagerModel> magerModels { get; set; }
        //model name should be changed (mvc already same model name)
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskHelper> TaskHelpers { get; set; }

        //  public DbSet<User> Users { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //To use fluent API here~~
            modelBuilder.Entity<ApplicationUser>()
            .HasMany(c => c.Projects).WithMany(i => i.ApplicationUsers)
            .Map(t => t.MapLeftKey("ApplicationUser_Id")
            .MapRightKey("Project_Id")
            .ToTable("UserProjects"));

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(c => c.TaskHelpers).WithMany(i => i.ApplicationUsers)
           .Map(t => t.MapLeftKey("ApplicationUser_Id")
           .MapRightKey("TaskHelper_Id")
           .ToTable("UserTasks"));

            modelBuilder.Entity<Project>()
            .HasMany<TaskHelper>(g => g.TaskHelpers)
            .WithRequired(s => s.Project)
            .HasForeignKey<Guid>(s => s.Project_Id);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //    public System.Data.Entity.DbSet<MvcDemo.Models.RoleViewModel> RoleViewModels { get; set; }
    }
}