namespace FirstMvcWebApp.DTOs;
public class UserDto
{
    public int UserId { get; set; } = 0;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = default!; // Default value: 01/01/0001 00:00:00
    public string Gender { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string UserType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; } = null;
    public string Hobby { get; set; } = null!;
}