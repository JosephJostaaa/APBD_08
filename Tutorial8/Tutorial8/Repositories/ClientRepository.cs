using Microsoft.Data.SqlClient;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public class ClientRepository : IClientsRepository
{
    private readonly string _connectionString;

    public ClientRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<int> AddClientAsync(Client client, CancellationToken cancellationToken)
    {
        string command = @"INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                           OUTPUT INSERTED.IdClient
                           VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel);
            await conn.OpenAsync(cancellationToken);
            
            var res =  await cmd.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(res);
        }
    }

    public async Task<Boolean> ExistsByIdAsync(int id, CancellationToken cancellationToken)
    {
        string query = @"SELECT COUNT(*) FROM Client WHERE IdClient = @IdClient";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = query;
                
                command.Parameters.AddWithValue("@IdClient", id);
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

    public async Task<bool> RegisterClientForTrip(int clientId, int tripId, CancellationToken cancellationToken)
    {
        string query = @"INSERT INTO CLIENT_TRIP(IdClient, IdTrip, RegisteredAt) VALUES (@IdClient, @IdTrip, @RegisteredAt)";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = query;
                
                command.Parameters.AddWithValue("@IdClient", clientId);
                command.Parameters.AddWithValue("@IdTrip", tripId);
                command.Parameters.AddWithValue("@RegisteredAt", int.Parse(DateTime.Now.ToString("yyyyMMdd")));
                

                await conn.OpenAsync(cancellationToken);
                var result = await command.ExecuteNonQueryAsync(cancellationToken);

                return result == 1;

            }
        }
    }
    
    

    public async Task<bool> CancelClientRegistration(int clientId, int tripId, CancellationToken cancellationToken)
    {
        string query = "DELETE FROM CLIENT_TRIP WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = query;
                
                command.Parameters.AddWithValue("@IdClient", clientId);
                command.Parameters.AddWithValue("@IdTrip", tripId);
                await conn.OpenAsync(cancellationToken);
                var result = await command.ExecuteNonQueryAsync(cancellationToken);

                return result == 1;

            }
        }
    }
}