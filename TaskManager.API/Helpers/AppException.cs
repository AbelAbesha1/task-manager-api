namespace TaskManager.API.Helpers
{
    // 404 - Resource not found
    public class NotFoundException : KeyNotFoundException
    {
        public NotFoundException(string resource, int id)
            : base($"{resource} with ID {id} was not found.")
        {
        }
    }

    // 400 - Bad request / business rule violation
    public class BadRequestException : ArgumentException
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }

    // 409 - Conflict (e.g. duplicate email)
    public class ConflictException : Exception
    {
        public int StatusCode { get; } = 409;
        public ConflictException(string message) : base(message)
        {
        }
    }
}