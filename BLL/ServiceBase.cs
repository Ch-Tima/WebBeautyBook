using BLL.Response;

namespace BLL
{
    public abstract class ServiceBase
    {

        protected virtual IServiceResponse OkResult() => new OkResponse();
        protected virtual IServiceResponse BadResult(string msg) => new BadResponse(msg);

    }
}