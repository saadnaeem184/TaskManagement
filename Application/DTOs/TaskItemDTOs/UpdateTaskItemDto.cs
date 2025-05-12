using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TaskItemDTOs
{
    public class UpdateTaskItemDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string? ExistingPictureUrl { get; set; }
        public string? PictureUrl { get; set; }
        public IFormFile? NewPicture { get; set; }

        public string? AssigneeId { get; set; }
        public List<string> EmployeeOptions { get; set; } = new List<string>(); // For dropdown

        [Required]
        public TaskItemStatus Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RewardPrice { get; set; }
    }
}
