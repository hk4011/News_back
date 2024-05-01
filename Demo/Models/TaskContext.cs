using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class TaskContext:IdentityDbContext<ApplicationUser>
    {
     
        public TaskContext() : base() { }
        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
        }
        public DbSet<News> News { get; set; }
        public DbSet<Authors> Authors { get; set; }
    }
}
