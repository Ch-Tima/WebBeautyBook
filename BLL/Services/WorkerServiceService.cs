using DAL.Repository;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class WorkerServiceService
    {

        private readonly WorkerRepository _workerRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly WorkerServiceRepository _workerServiceRepository;

        public WorkerServiceService(WorkerRepository workerRepository, ServiceRepository serviceRepository, 
            WorkerServiceRepository workerServiceRepository)
        {
            _workerRepository = workerRepository;
            _serviceRepository = serviceRepository;
            _workerServiceRepository = workerServiceRepository;
        }

        public async Task<Domain.Models.WorkerService> GetAsync(string id)
        {
            return await _workerServiceRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Domain.Models.WorkerService>> GetAllAsync()
        {
            return await _workerServiceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Domain.Models.WorkerService>> GetAllFindAsync(Expression<Func<Domain.Models.WorkerService, bool>> func)
        {
            return await _workerServiceRepository.GetAllFindAsync(func);
        }

        public async Task<ServiceResponse> InsertAsync(string companyId, Domain.Models.WorkerService workerService)
        {
            try
            {
                var worker = await _workerRepository.GetFirstAsync(x => x.Id == workerService.WorkerId && x.CompanyId == companyId);
                if (worker == null)//Is this "Worker" owned by a company?
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                var serviec = await _serviceRepository.GetFirstAsync(x => x.Id == workerService.ServiceId && x.CompanyId == companyId);
                if(serviec == null)//Is this "Serviec" owned by a company?
                    return new ServiceResponse(false, $"This Serviec does not belong to this company.");

                //find Duplicate 
                var workerServiceDuplicate = await _workerServiceRepository.GetFirstAsync(x => x.ServiceId == workerService.ServiceId && x.WorkerId == workerService.WorkerId);
                if (workerServiceDuplicate != null)
                    return new ServiceResponse(false, "Worker has this service.");

                await _workerServiceRepository.InsertAsync(workerService);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(string companyId, string workerId, string serviceId)
        {
            try
            {
                //find WorkerService in database
                var workerService = await _workerServiceRepository.GetFirstIncludeAsync(x => x.ServiceId == serviceId &&  x.WorkerId == workerId);

                if (workerService == null) 
                    return new ServiceResponse(false, "This worker does not own this service.");

                //If this service and worker belong to the company then delete

                if (workerService.Worker.CompanyId != companyId)//Is this "Worker" owned by a company?
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                if (workerService.Service.CompanyId != companyId)//Is this "Service" owned by a company?
                    return new ServiceResponse(false, $"This Service does not belong to this company.");

                //TODO: check for open Records

                await _workerServiceRepository.DeleteAsync(workerService.Id);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task UpdataAsync(Domain.Models.WorkerService newWorker)
        {
            if (newWorker == null) return;

            await _workerServiceRepository.UpdateAsync(newWorker.Id, newWorker);
        }

    }
}
