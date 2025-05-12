using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskItemDTOs
{
    public class TaskItemListItemDto
    {
        public int Id { get; set; }
        public string? PictureUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal RewardPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool CanEditDelete { get; set; }
    }
}
