using WebApplication1.Constants.Enums;

namespace WebApplication1.Utils.HttpExceptionResponse
{
    public class HttpExceptionResponse : Exception
    {
        public HttpExceptionResponse() { }
        public HttpExceptionResponse(
            HttpExceptionTypes statusCode,
            string message,
            object? errors = null
        ) => (StatusCode, Message, Errors) = (statusCode, message, errors);

        public HttpExceptionTypes StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Errors { get; set; }
    }
}
