using System.ComponentModel.DataAnnotations;
using TaskManager.API.Helpers;

namespace TaskManager.API.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [RegularExpression("^(Low|Medium|High)$",
            ErrorMessage = "Priority must be Low, Medium, or High")]
        public string Priority { get; set; } = "Medium";

        [FutureDate(ErrorMessage = "Due date must be in the future")]
        public DateTime DueDate { get; set; }
    }
}