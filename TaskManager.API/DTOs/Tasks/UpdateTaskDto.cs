using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [RegularExpression("^(Low|Medium|High)$",
            ErrorMessage = "Priority must be Low, Medium, or High")]
        public string Priority { get; set; } = "Medium";

        public DateTime DueDate { get; set; }
    }
}