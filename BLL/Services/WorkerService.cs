using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class WorkerService
    {
        private readonly WorkerRepository _repositoryWorker;
        private readonly BaseUserService _baseUserService;
        private readonly CompanyRepository _companyRepository;

        public WorkerService(WorkerRepository repositoryWorker, BaseUserService baseUserService, CompanyRepository companyRepository)
        {
            _repositoryWorker = repositoryWorker;
            _baseUserService = baseUserService;
            _companyRepository = companyRepository;
        }

        public async Task<Worker?> GetAsync(string id)
        {
            if(id == null) return null;
            return await _repositoryWorker.GetAsync(id);
        }

        public async Task<Worker?> GetIncudeAsync(string id)
        {
            if (id == null) return null;
            return await _repositoryWorker.GetIncudeAsync(id);
        }

        public async Task<IEnumerable<Worker>> GetAllIncludeFindAsync(Expression<Func<Worker, bool>> func)
        {
            return await _repositoryWorker.GetAllFindIncludeAsync(func);
        }

        public async Task<IEnumerable<Worker>> GetAllFindAsync(Expression<Func<Worker, bool>> func)
        {
            return await _repositoryWorker.GetAllFindAsync(func);
        }

        public async Task<ServiceResponse> InsertAsync(string companyId, BaseUser baseUser)
        {
            try
            {
               if ((await _companyRepository.GetAsync(companyId)) == null)
                    return new ServiceResponse(false, $"Company with id: {companyId} was not found.");

                if (baseUser == null || (await _baseUserService.GetAsync(baseUser.Id)) == null)
                    return new ServiceResponse(false, "User not found.");

                //create new Worker
                var worker = new Worker()
                {
                    CompanyId = companyId,
                    BaseUserId = baseUser.Id
                };

                await _repositoryWorker.InsertAsync(worker);

                //linking a worker profile to baseUser
                baseUser.WorkerId = worker.Id;
                await _baseUserService.UpdateAsync(baseUser);

                return new ServiceResponse(true, "Completed.");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task ExitFromCompanyAsync(string workerId)
        {
            //find worker profile
            var workerProfile = await _repositoryWorker.GetAsync(workerId);
            
            if (workerProfile == null) return;

            //user is not null, Worker cannot exist without BaseUser
            var user = await _baseUserService.GetAsync(workerProfile.BaseUserId);//find BaseUser

            //remove Worker
            await _repositoryWorker.DeleteAsync(workerId);

            //clear workerId and update user
            user.WorkerId = null; 
            await _baseUserService.UpdateAsync(user);

        }

        public async Task UpdataAsync(Worker newWorker)
        {
            if (newWorker == null) return;

            await _repositoryWorker.UpdateAsync(newWorker.Id, newWorker);
        }

    }
}
