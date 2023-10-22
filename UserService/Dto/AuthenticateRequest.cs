namespace UserService.Dto;

public class AuthenticateRequest
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}