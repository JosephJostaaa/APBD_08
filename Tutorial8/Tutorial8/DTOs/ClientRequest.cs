namespace Tutorial8.Models.DTOs;

using System.ComponentModel.DataAnnotations;

public class ClientRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "PESEL is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must be exactly 11 digits.")]
    public string Pesel { get; set; }
}
