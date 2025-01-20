namespace FirstMvcWebApp.Models;
public class UserModel
{
    public int UserId { get; set;}=0;
    public string UserName { get; set;}=null!;
    public string Email { get; set; }=null!;
    public string Password { get; set; }=null!;
    public DateTime DateOfBirth { get; set; }=default;
    public string Gender { get; set; }=null!;
    public string PhoneNumber { get; set; }=null!;
    public string Address { get; set; }=null!;
    public string UserType {get; set;}=null!;
    public string Status { get; set;}=null!;
    public DateTime CreatedDate { get; set;}=default;
    public DateTime UpdateDate{ get; set;}=default;
    public string Hobby { get; set;}=null!;

}
