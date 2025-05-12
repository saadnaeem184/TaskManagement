using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskItemDTOs
{
    public class TaskListDto
    {
        public List<TaskItemListItemDto> Tasks { get; set; } = new List<TaskItemListItemDto>();
        public bool IsEmployer { get; set; }
    }
}
