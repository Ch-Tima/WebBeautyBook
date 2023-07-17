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

        public async Task<ServiceResponse> InsertAsync(Domain.Models.WorkerService workerService)
        {
            try
            {
                var worker = await _workerRepository.GetIncudeAsync(workerService.WorkerId);
                var serviec = await _serviceRepository.GetAsync(workerService.ServiceId);

                if(serviec == null)
                    return new ServiceResponse(false, "Serviec not found.");

                if (worker == null) 
                    return new ServiceResponse(false, "Worker not found.");

                if (worker.WorkerServices.Any(x => x.ServiceId == workerService.ServiceId))
                    return new ServiceResponse(false, "Worker has this service.");

                await _workerServiceRepository.InsertAsync(workerService);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(string workerId, string serviceId)
        {
            try
            {
                var workerService = await _workerServiceRepository.GetFirstAsync(x => x.ServiceId == serviceId &&  x.WorkerId == workerId);

                if(workerService == null) 
                    return new ServiceResponse(false, "WorkerService not found.");

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
