using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskItemDTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
        public string? AssigneeId { get; set; }
        public string? AssigneeName { get; set; } // For display purposes
        public string? Status { get; set; }
        public decimal RewardPrice { get; set; }
        public string CreatedById { get; set; } = default!;
        public string CreatedByName { get; set; } // For display purposes
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
