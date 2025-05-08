using Microsoft.Data.SqlClient;
using Tutorial8.Mappers;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private ITripRepository _tripRepository;

    public TripsService(ITripRepository tripRepository)
    {
        this._tripRepository = tripRepository;
    }

    public async Task<List<TripDTO>> GetTrips(CancellationToken ct)
    {
        List<Trip> trips = await _tripRepository.FindTripsAsync(ct);

        return trips.Select(TripMapper.MapToTripDTO).ToList();
    }
}