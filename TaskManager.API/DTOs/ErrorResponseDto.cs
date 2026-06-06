namespace TaskManager.API.DTOs
{
    public class ErrorResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}