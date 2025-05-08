using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface IClientsRepository
{
    public Task<int> AddClientAsync(Client client, CancellationToken cancellationToken);
    public Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
    public Task<bool> RegisterClientForTrip(int clientId, int tripId, CancellationToken cancellationToken);
    public Task<bool> CancelClientRegistration(int clientId, int tripId, CancellationToken cancellationToken);

}