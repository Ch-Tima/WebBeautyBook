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

        public async Task<ServiceResponse> InsertAsync(string companyId, Assignment assignment)
        {
            try
            {
                var worker = await _workerRepository.GetFirstAsync(x => x.Id == assignment.WorkerId && x.CompanyId == companyId);
                if (worker == null)//Is this "Worker" owned by a company?
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                var serviec = await _serviceRepository.GetFirstAsync(x => x.Id == assignment.ServiceId && x.CompanyId == companyId);
                if(serviec == null)//Is this "Serviec" owned by a company?
                    return new ServiceResponse(false, $"This Serviec does not belong to this company.");

                //find Duplicate 
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

        public async Task<ServiceResponse> DeleteAsync(string companyId, string workerId, string serviceId)
        {
            try
            {
                //find assignment in database
                var assignment = await _assignmentRepository.FirstIncludeAsync(x => x.ServiceId == serviceId &&  x.WorkerId == workerId);

                if (assignment == null) 
                    return new ServiceResponse(false, "This worker does not own this service.");

                //If this service and worker belong to the company then delete

                if (assignment.Worker.CompanyId != companyId)//Is this "Worker" owned by a company?
                    return new ServiceResponse(false, $"This Worker does not belong to this company.");

                if (assignment.Service.CompanyId != companyId)//Is this "Service" owned by a company?
                    return new ServiceResponse(false, $"This Service does not belong to this company.");

                //TODO: check for open Records

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
