namespace BLL.Response
{
    public class OkResponse : IServiceResponse
    {

        public string Message => "Ok";

        public bool IsSuccess => true;
    }
}
