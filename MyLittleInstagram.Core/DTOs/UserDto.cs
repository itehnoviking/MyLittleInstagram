namespace MyLittleInstagram.Core.DTOs;

public class UserDto
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime? RegistrationDate { get; set; }
}