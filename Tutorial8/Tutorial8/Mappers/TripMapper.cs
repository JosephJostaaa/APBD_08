namespace Tutorial8.Mappers;

using Tutorial8.Models;
using Tutorial8.Models.DTOs;

public static class TripMapper
{
    public static TripDTO MapToTripDTO(Trip trip)
    {
        return new TripDTO
        {
            Id = trip.Id,
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            maxPeople = trip.maxPeople,
            Countries = trip.Countries.Select(country => new CountryDTO
            {
                Name = country.CountryName
            }).ToList()
        };
    }
    
    public static TripRegistrationDto MapToTripRegistrationDTO(Trip trip)
    {
        return new TripRegistrationDto()
        {
            Id = trip.Id,
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            maxPeople = trip.maxPeople,
            Countries = trip.Countries.Select(country => new CountryDTO
            {
                Name = country.CountryName
            }).ToList(),
            RegistrationDetails = new RegistrationDetails(){
                RegisteredAt = trip.RegisteredAt,
                PaymentDate = trip.PaymentDate
            }
        };
    }
    
    
}