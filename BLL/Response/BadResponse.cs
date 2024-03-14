namespace BLL.Response
{
    public class BadResponse : IServiceResponse
    {
        public BadResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public bool IsSuccess => false;
    }
}
