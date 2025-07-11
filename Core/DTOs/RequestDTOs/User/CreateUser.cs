namespace Core.DTOs.RequestDTOs;

public class CreateUser
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}
