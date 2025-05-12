using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TaskStatusUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskItemId { get; set; }
        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; } = default!;

        public TaskItemStatus Status { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Optionally, who made the update
        public string? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        public virtual AppUser? UpdatedBy { get; set; }
    }
}
