using System.ComponentModel.DataAnnotations;
namespace FirstApi.DTOs;
public class CreateUserRequestDTO
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; } = default!; // Default value: 01/01/0001 00:00:00

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public string UserType { get; set; } = null!;

    [Required]
    public string Status { get; set; } = null!;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdateDate { get; set; } = null;

    [Required]
    public string Hobby { get; set; } = null!;
}