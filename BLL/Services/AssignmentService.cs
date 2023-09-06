using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class AssignmentService
    {

        private readonly WorkerRepository _workerRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly AssignmentRepository _assignmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentService"/> class.
        /// </summary>
        /// <param name="workerRepository">The repository for worker data.</param>
        /// <param name="serviceRepository">The repository for service data.</param>
        /// <param name="assignmentRepository">The repository for assignment data.</param>
        public AssignmentService(WorkerRepository workerRepository, ServiceRepository serviceRepository, 
            AssignmentRepository assignmentRepository)
        {
            _workerRepository = workerRepository;
            _serviceRepository = serviceRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Assignment> GetAsync(string id)
        {
            return await _assignmentRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _assignmentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllFindAsync(Expression<Func<Assignment, bool>> func)
        {
            return await _assignmentRepository.GetAllFindAsync(func);
        }

        public async Task<Assignment?> FirstIncludeAsync(Expression<Func<Assignment, bool>> func)
        {
            return await _assignmentRepository.FirstIncludeAsync(func);
        }

        /// <summary>
        /// Inserts a new assignment into the repository.
        /// </summary>
        /// <param name="companyId">The ID of the company to which the assignment should belong.</param>
        /// <param name="assignment">The assignment to insert.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the insertion operation.</returns>
        public async Task<ServiceResponse> InsertAsync(string companyId, Assignment assignment)
        {
            try
            {
                var worker = await _workerRepository.GetFirstAsync(x => x.Id == assignment.WorkerId && x.CompanyId == companyId);
                if (worker == null)//Check if the worker is owned by the specified company
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                var serviec = await _serviceRepository.GetFirstAsync(x => x.Id == assignment.ServiceId && x.CompanyId == companyId);
                if(serviec == null)//Check if the service is owned by the specified company
                    return new ServiceResponse(false, $"This Serviec does not belong to this company.");

                //Check for duplicate assignments (worker having the same service).
                var assignmentDuplicate = await _assignmentRepository.GetFirstAsync(x => x.ServiceId == assignment.ServiceId && x.WorkerId == assignment.WorkerId);
                if (assignmentDuplicate != null)
                    return new ServiceResponse(false, "Worker has this service.");

                await _assignmentRepository.InsertAsync(assignment);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an assignment if it meets specific criteria.
        /// </summary>
        /// <param name="companyId">The ID of the company for which the assignment is being deleted.</param>
        /// <param name="workerId">The ID of the worker associated with the assignment.</param>
        /// <param name="serviceId">The ID of the service associated with the assignment.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the deletion operation.</returns>
        public async Task<ServiceResponse> DeleteAsync(string companyId, string workerId, string serviceId)
        {
            try
            {
                //find assignment in database
                var assignment = await _assignmentRepository.FirstIncludeAsync(x => x.ServiceId == serviceId &&  x.WorkerId == workerId);
                if (assignment == null) 
                    return new ServiceResponse(false, "This worker does not own this service.");

                //Check if this service and worker belong to the same company
                if (assignment.Worker.CompanyId != companyId)//Check if the "Worker" is owned by the specified company
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                if (assignment.Service.CompanyId != companyId)//Check if the "Service" is owned by the specified company
                    return new ServiceResponse(false, $"This Service does not belong to this company.");

                // TODO: Check for open records (you may want to implement this check).

                //Delete the assignment from the repository
                await _assignmentRepository.DeleteAsync(assignment);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}
