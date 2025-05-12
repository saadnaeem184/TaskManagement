using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string? PictureUrl { get; set; }

        // Foreign key for the AppUser who is assigned this task
        public string? AssigneeId { get; set; } // Changed to string? to match AppUser.Id
        [ForeignKey("AssigneeId")]
        public virtual AppUser? Assignee { get; set; }

        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RewardPrice { get; set; }

        // Foreign key for the AppUser who created this task
        [Required]
        public string CreatedById { get; set; } = default!; // Changed to string to match AppUser.Id
        [ForeignKey("CreatedById")]
        public virtual AppUser CreatedBy { get; set; } = default!;

        public virtual ICollection<TaskStatusUpdate> StatusUpdates { get; set; } = new List<TaskStatusUpdate>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
