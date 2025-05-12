using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskItemDTOs
{
    public class EmployeeTaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
        public TaskItemStatus Status { get; set; }
        public decimal RewardPrice { get; set; }

        [Required]
        public TaskItemStatus NewStatus { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
