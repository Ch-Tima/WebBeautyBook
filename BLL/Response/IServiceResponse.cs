namespace BLL.Response
{
    public interface IServiceResponse
    {
        string Message { get; }
        bool IsSuccess { get; }

    }
}
