using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskStatusUpdate
{
    public class CreateTaskStatusUpdateDto
    {
        [Required]
        public int TaskItemId { get; set; }

        [Required]
        public TaskItemStatus Status { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;
    }
}
