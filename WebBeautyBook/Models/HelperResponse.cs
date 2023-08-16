namespace WebBeautyBook.Models
{
    public class HelperResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public HelperResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
