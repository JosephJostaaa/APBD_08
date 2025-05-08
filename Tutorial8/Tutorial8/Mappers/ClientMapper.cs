using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Mappers;

public class ClientMapper
{
    public static Client MapToClient(ClientRequest clientRequest)
    {
        return new Client
        {
            FirstName = clientRequest.FirstName,
            LastName = clientRequest.LastName,
            Email = clientRequest.Email,
            Telephone = clientRequest.Phone,
            Pesel = clientRequest.Pesel
        };
    }
}