using System.ComponentModel.DataAnnotations.Schema;
namespace FirstApi.Models;

[Table("t_users")]
public class User
{
    [Column("c_userid")]
    public int UserId { get; set; } = 0;
    
    [Column("c_username")]
    public string UserName { get; set; } = null!;
   
    [Column("c_email")]
    public string Email { get; set; } = null!;
   
    [Column("c_password")]
    public string Password { get; set; } = null!;
   
    [Column("c_dateofbirth")]
    public DateTime DateOfBirth { get; set; } = default!; //01/01/0001 00:00:00
  
    [Column("c_gender")]
    public string Gender { get; set; } = null!;
  
    [Column("c_phonenumber")]
    public string PhoneNumber { get; set; } = null!;
  
    [Column("c_address")]
    public string Address { get; set; } = null!;
  
    [Column("c_usertype")]
    public string UserType { get; set; } = null!;
  
    [Column("c_status")]
    public string Status { get; set; } = null!;
  
    [Column("c_createdat")]
    public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
   
    [Column("c_updatedat")]
    public DateTime? UpdateDate { get; set; }=null;
   
    [Column("c_hobby")]
    public string Hobby { get; set; } = null!;
}
