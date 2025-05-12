using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        // Example custom property
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Navigation properties for tasks created by this user
        public virtual ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();

        // Navigation properties for tasks assigned to this user
        public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
