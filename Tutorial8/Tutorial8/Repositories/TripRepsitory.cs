using Microsoft.Data.SqlClient;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public class TripRepository : ITripRepository
{
    private readonly string _connectionString;

    public TripRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    

    public async Task<List<Trip>> FindTripsAsync(CancellationToken ct, SimpleTripFilter? filter = null)
    {
        var trips = new List<Trip>();

        string command = @"SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, c.IdCountry, c.Name AS CountryName
                           FROM Trip t
                           JOIN Country_Trip tc ON t.IdTrip = tc.IdTrip
                           JOIN Country c ON tc.IdCountry = c.IdCountry
                           WHERE 1 = 1";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand())
        {
            if (filter?.TripId != null)
            {
                command += " AND t.IdTrip = @IdTrip";
                cmd.Parameters.AddWithValue("@IdTrip", filter.TripId);
            }
            
            if (filter?.Name != null)
            {
                command += " AND t.Name = @Name";
                cmd.Parameters.AddWithValue("@Name", filter.Name);
            }
            cmd.CommandText = command;
            cmd.Connection = conn;
            
            await conn.OpenAsync(ct);

            using (var reader = await cmd.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    var tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));
                    var trip = trips.FirstOrDefault(t => t.Id == tripId);

                    if (trip == null)
                    {
                        trip = new Trip
                        {
                            Id = tripId,
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            DateFrom = (DateTime)reader["DateFrom"],
                            DateTo = (DateTime)reader["DateTo"],
                            maxPeople = reader["maxPeople"] != DBNull.Value ? (int)reader["maxPeople"] : 0,
                            Countries = new List<Country>()
                        };
                        
                        trips.Add(trip);
                    }

                    trip.Countries.Add(new Country
                    {
                        Id = (int)reader["IdCountry"],
                        CountryName = (string)reader["CountryName"]
                    });
                }
            }
        }
        return trips;
    }

    public async Task<int> GetNumberOfParticipantsById(int tripId, CancellationToken cancellationToken)
    {
        string query = @"SELECT Count(*) as ParticipantsCount FROM Client_Trip WHERE IdTrip = @IdTrip";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@IdTrip", tripId);
                
                await conn.OpenAsync(cancellationToken);
                var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                using (reader)
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return reader.GetInt32(reader.GetOrdinal("ParticipantsCount"));
                    }
                }
            }
        }
        return 0;
    }

    public async Task<List<Trip>> FindAllByClientIdAsync(int id, CancellationToken ct)
    {
        
        var trips = new List<Trip>();

        string command  = @"SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, c.IdCountry, c.Name AS CountryName, ct.RegisteredAt, ct.PaymentDate
                           FROM Trip t
                            JOIN Country_Trip tc ON t.IdTrip = tc.IdTrip
                            JOIN Country c ON tc.IdCountry = c.IdCountry 
                            JOIN Client_Trip ct ON ct.IdTrip = t.IdTrip WHERE ct.IdClient = @ClientId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync(ct);
            cmd.Parameters.AddWithValue("@ClientId", id);

            using (var reader = await cmd.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    var tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));
                    var trip = trips.FirstOrDefault(t => t.Id == tripId);

                    if (trip == null)
                    {
                        trip = new Trip
                        {
                            Id = tripId,
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            DateFrom = (DateTime)reader["DateFrom"],
                            DateTo = (DateTime)reader["DateTo"],
                            maxPeople = reader["maxPeople"] != DBNull.Value ? (int)reader["maxPeople"] : 0,
                            Countries = new List<Country>(),
                            RegisteredAt = reader["RegisteredAt"] != DBNull.Value ? (int)reader["RegisteredAt"] : 0,
                            PaymentDate = reader["PaymentDate"] != DBNull.Value ? (int)reader["PaymentDate"] : 0
                        };
                        
                        trips.Add(trip);
                    }

                    trip.Countries.Add(new Country
                    {
                        Id = (int)reader["IdCountry"],
                        CountryName = (string)reader["CountryName"]
                    });
                }
            }
        }
        return trips;
        
    }

    public Task<Trip> FindByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Boolean> ExistsByIdAsync(int id, CancellationToken cancellationToken)
    {
        string query = @"SELECT COUNT(*) FROM Trip WHERE IdTrip = @IdTrip";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = query;
                
                command.Parameters.AddWithValue("@IdTrip", id);
                await conn.OpenAsync(cancellationToken);
                var result = await command.ExecuteScalarAsync(cancellationToken);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result) > 0;
                }
                
                return false;
                
            }
        }
    }
}