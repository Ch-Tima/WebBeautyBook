namespace BLL
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ServiceResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
