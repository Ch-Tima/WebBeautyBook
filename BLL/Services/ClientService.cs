using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class ClientService
    {

        private readonly ClientRepository _clientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetAsync(string id)
        {
            return await _clientRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Client>> GetAllFindAsync(Expression<Func<Client, bool>> func)
        {
            return await _clientRepository.GetAllFindAsync(func);
        }

        public async Task InsertAsync(Client client)
        {
            await _clientRepository.InsertAsync(client);
        }

        public async Task DeleteAsync(string id)
        {
            await _clientRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Client newClient)
        {
            if (newClient == null) return;

            await _clientRepository.UpdateAsync(newClient.Id, newClient);
        }


    }
}
