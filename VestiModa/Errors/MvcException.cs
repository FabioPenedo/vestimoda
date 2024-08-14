namespace VestiModa.Errors
{
    public class MvcException
    {
        public string StatusCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;

        public MvcException(string statusCode, string message, string details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
