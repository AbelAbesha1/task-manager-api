namespace TaskManager.API.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string Priority { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}