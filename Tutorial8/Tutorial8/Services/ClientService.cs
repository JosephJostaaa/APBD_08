using Tutorial8.Mappers;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientService : IClientsService
{
    private readonly ITripRepository _tripRepository;
    private IClientsRepository _clientsRepository;

    public ClientService(ITripRepository tripRepository, IClientsRepository clientsRepository)
    {
        _tripRepository = tripRepository;
        _clientsRepository = clientsRepository;
    }

    public async Task<List<TripRegistrationDto>> GetTripsByClientIdAsync(int id, CancellationToken ct)
    {
        List<Trip> trips = await _tripRepository.FindAllByClientIdAsync(id, ct);

        return trips.Select(TripMapper.MapToTripRegistrationDTO).ToList();
    }

    public async Task<int> AddClientAsync(ClientRequest? clientRequest, CancellationToken ct)
    {
        if (clientRequest == null)
        {
            return -1;
        }
        Client client = ClientMapper.MapToClient(clientRequest);
        return  await _clientsRepository.AddClientAsync(client, ct);
        
    }

    public async Task<ClientRegistrationResponse> RegisterClientOnTripAsync(int clientId, int tripId, CancellationToken ct)
    {
        bool exists = await _clientsRepository.ExistsByIdAsync(clientId, ct);

        if (!exists)
        {
            return new ClientRegistrationResponse(false, "Client not found");
        }
        bool tripExists = await _tripRepository.ExistsByIdAsync(tripId, ct);
        if (!tripExists)
        {
            return new ClientRegistrationResponse(false, "Trip not found");
        }
        var participantcCount = await _tripRepository.GetNumberOfParticipantsById(clientId, ct);
        
        var trips = await _tripRepository.FindTripsAsync(ct);
        
        var tripToRegister = trips.FirstOrDefault(t => t.Id == tripId);

        if (tripToRegister == null)
        {
            return new ClientRegistrationResponse(false, "Trip not found");
        }
        
        if (participantcCount >= tripToRegister.maxPeople)
        {
            return new ClientRegistrationResponse(false, "Trip is full");
        }

        bool isRegistered = await _clientsRepository.RegisterClientForTrip(clientId, tripId, ct);

        if (isRegistered)
        {
            return new ClientRegistrationResponse(true, "Client registered for trip");
        }
        
        return new ClientRegistrationResponse(false, "Could not register client");
    }

    public async Task<ClientRegistrationResponse> CancelRegistrationAsync(int clientId, int tripId, CancellationToken ct)
    {
        var trips = await _tripRepository.FindTripsAsync(ct, new SimpleTripFilter(clientId));
        var trip = trips.FirstOrDefault(t => t.Id == tripId);
        
        if (trip == null)
        {
            return new ClientRegistrationResponse(false, "Registration does not exist");
        }
        bool isCancelled = await _clientsRepository.CancelClientRegistration(clientId, tripId, ct);
        if (isCancelled)
        {
            return new ClientRegistrationResponse(true, "Registration cancelled");
        }
        
        return new ClientRegistrationResponse(false, "Could not cancel registration");
        
    }
}