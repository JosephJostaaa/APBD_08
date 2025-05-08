namespace Tutorial8.Models.DTOs;

public class TripRegistrationDto : TripDTO
{
    public RegistrationDetails RegistrationDetails { get; set; }
}

public class RegistrationDetails
{
    public int? RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
}