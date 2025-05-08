using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface ITripRepository 
{
    public Task<List<Trip>> FindTripsAsync(CancellationToken ct, SimpleTripFilter? filter = null);
    public Task<List<Trip>> FindAllByClientIdAsync(int id, CancellationToken ct);
    public Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
    public Task<int> GetNumberOfParticipantsById(int tripId, CancellationToken cancellationToken);
    
}