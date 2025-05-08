namespace Tutorial8.Models;

public class Trip
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int maxPeople { get; set; }
    public List<Country> Countries { get; set; } = new List<Country>();
    public int? RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
    
}