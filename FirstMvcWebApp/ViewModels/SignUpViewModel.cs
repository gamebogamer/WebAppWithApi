using System.ComponentModel.DataAnnotations;

namespace FirstMvcWebApp.ViewModel;
public class SignUpViewModel
{

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public string UserName { get; set;}=null!;
 
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }=null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }=null!;

    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }=null!;

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [StringLength(10, ErrorMessage = "Username cannot exceed 10 characters.")]
    public string Gender { get; set; }=null!;

    [Required(ErrorMessage = "Phone Number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }=null!;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
    public string Address { get; set; }=null!;
    
    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Hobby { get; set; }=null!;

}
