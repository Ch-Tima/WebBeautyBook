using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class WorkerController : ControllerBase
    {
        private readonly BLL.Services.WorkerService _workerService;

        public WorkerController(BLL.Services.WorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet("getWorkersByServiceId/{serviceId}")]
        public async Task<IEnumerable<Worker>> GetWorkersByServiceId(string serviceId)
        {
            var workers = await _workerService.GetAllIncludeFindAsync(x => x.Assignments.Any(r => r.ServiceId == serviceId));
            var result = await DeleteSensitiveData(workers);
            return result;
        }

        [HttpGet("getWorkersByCompanyId/{companyId}")]
        public async Task<IEnumerable<Worker>> GetWorkersByCompanyId(string companyId)
        {
            var workers = await _workerService.GetAllIncludeFindAsync(x => x.CompanyId == companyId);
            var result = await DeleteSensitiveData(workers);
            return result;
        }

        private async Task<IEnumerable<Worker>> DeleteSensitiveData(IEnumerable<Worker> workers)
        {
            workers.ToList().ForEach(worker =>
            {
                if(worker.BaseUser != null)
                {
                    worker.BaseUser.Email = String.Empty;
                    worker.BaseUser.PhoneNumber = String.Empty;
                    worker.BaseUser.NormalizedEmail = String.Empty;
                    worker.BaseUser.PasswordHash = String.Empty;
                    worker.BaseUser.ConcurrencyStamp = String.Empty;
                    worker.BaseUser.SecurityStamp = String.Empty;
                }
            });

            return workers;
        }

    }
}
