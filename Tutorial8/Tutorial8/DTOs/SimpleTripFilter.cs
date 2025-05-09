namespace Tutorial8.Models.DTOs;

public class SimpleTripFilter
{
    public SimpleTripFilter(int? tripId)
    {
        TripId = tripId;
    }
    
    public SimpleTripFilter(string? name)
    {
        Name = name;
    }

    public SimpleTripFilter(string? name, int? tripId)
    {
        Name = name;
        TripId = tripId;
    }

    public string? Name { get; set; }
    public int? TripId { get; set; }
}