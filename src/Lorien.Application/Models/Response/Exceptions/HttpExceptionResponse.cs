namespace Lorien.Application.Models.Response.Exceptions
{
    public record HttpExceptionResponse
    {
        public required string Message { get; set; }
    }
}
