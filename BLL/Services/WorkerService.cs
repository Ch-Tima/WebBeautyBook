using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    /// <summary>
    /// Service class responsible for managing worker-related operations.
    /// </summary>
    public class WorkerService
    {
        private readonly WorkerRepository _repositoryWorker;
        private readonly BaseUserRepository _baseUserRepository;
        private readonly CompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerService"/> class.
        /// </summary>
        /// <param name="repositoryWorker">The repository for workers.</param>
        /// <param name="baseUserRepository">The repository for base users.</param>
        /// <param name="companyRepository">The repository for companies.</param>
        public WorkerService(WorkerRepository repositoryWorker, BaseUserRepository baseUserRepository, CompanyRepository companyRepository)
        {
            _repositoryWorker = repositoryWorker;
            _baseUserRepository = baseUserRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Gets a worker by its ID.
        /// </summary>
        /// <param name="id">The ID of the worker to retrieve.</param>
        /// <returns>The worker entity or null if not found.</returns>
        public async Task<Worker?> GetAsync(string id)
        {
            if(id == null) return null;
            return await _repositoryWorker.GetAsync(id);
        }

        /// <summary>
        /// Gets a worker by its ID, including related entities.
        /// </summary>
        /// <param name="id">The ID of the worker to retrieve.</param>
        /// <returns>The worker entity with included related data or null if not found.</returns>
        public async Task<Worker?> GetIncudeAsync(string id)
        {
            if (id == null) return null;
            return await _repositoryWorker.GetIncudeAsync(id);
        }

        /// <summary>
        /// Gets a list of workers based on a filter expression, including related entities.
        /// </summary>
        /// <param name="func">The filter expression to apply.</param>
        /// <returns>A list of workers with included related data.</returns>
        public async Task<IEnumerable<Worker>> GetAllIncludeFindAsync(Expression<Func<Worker, bool>> func)
        {
            return await _repositoryWorker.GetAllFindIncludeAsync(func);
        }

        /// <summary>
        /// Gets a list of workers based on a filter expression.
        /// </summary>
        /// <param name="func">The filter expression to apply.</param>
        /// <returns>A list of workers.</returns>

        public async Task<IEnumerable<Worker>> GetAllFindAsync(Expression<Func<Worker, bool>> func)
        {
            return await _repositoryWorker.GetAllFindAsync(func);
        }

        /// <summary>
        /// Inserts a new worker and associates it with a company and a base user.
        /// </summary>
        /// <param name="companyId">The id of the company to associate the worker with.</param>
        /// <param name="baseUser">The base user entity associated with the worker.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<ServiceResponse> InsertAsync(string companyId, BaseUser baseUser)
        {
            try
            {
               if ((await _companyRepository.GetAsync(companyId)) == null)
                    return new ServiceResponse(false, $"Company with id: {companyId} was not found.");

                if (baseUser == null || (await _baseUserRepository.GetAsync(baseUser.Id)) == null)
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
                await _baseUserRepository.UpdateAsync(baseUser.Id, baseUser);

                return new ServiceResponse(true, "Completed.");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Removes a worker from a company.
        /// </summary>
        /// <param name="workerId">The id of the worker to remove.</param>
        public async Task ExitFromCompanyAsync(string workerId)
        {
            //find worker profile
            var workerProfile = await _repositoryWorker.GetAsync(workerId);
            
            if (workerProfile == null) return;

            //user is not null, Worker cannot exist without BaseUser
            var user = await _baseUserRepository.GetAsync(workerProfile.BaseUserId);//find BaseUser

            //remove Worker
            await _repositoryWorker.DeleteAsync(workerId);

            //clear workerId and update user
            user.WorkerId = null; 
            await _baseUserRepository.UpdateAsync(user.Id, user);

        }

        /// <summary>
        /// Updates a worker's information.
        /// </summary>
        /// <param name="newWorker">The updated worker entity.</param>
        public async Task UpdataAsync(Worker newWorker)
        {
            if (newWorker == null) return;

            await _repositoryWorker.UpdateAsync(newWorker.Id, newWorker);
        }

    }
}