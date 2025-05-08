namespace Tutorial8.Models.DTOs;

public class ClientRegistrationResponse
{
    public bool IsSuccess {get; set;}
    public string Message {get; set;}
    
    public ClientRegistrationResponse(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}