namespace Tutorial8.Models.DTOs;

public class SimpleTripFilter
{
    public SimpleTripFilter(int? clientId)
    {
        ClientId = clientId;
    }

    public int? ClientId { get; set; }
}