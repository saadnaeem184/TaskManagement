using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.TaskStatusUpdate
{
    public class TaskStatusUpdateDto
    {
        //public int Id { get; set; }
        //public int TaskItemId { get; set; }
        [JsonPropertyName("status")]
        public TaskItemStatus Status { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; } = string.Empty;
        //public DateTime UpdatedAt { get; set; }
        //public string? UpdatedById { get; set; }
        //public string? UpdatedByName { get; set; }
    }
}
