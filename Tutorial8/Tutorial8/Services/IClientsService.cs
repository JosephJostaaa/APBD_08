using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    public Task<List<TripRegistrationDto>> GetTripsByClientIdAsync(int id, CancellationToken ct);
    public Task<int> AddClientAsync(ClientRequest clientRequest, CancellationToken ct);
    public Task<ClientRegistrationResponse> RegisterClientOnTripAsync(int clientId, int tripId, CancellationToken ct);
}