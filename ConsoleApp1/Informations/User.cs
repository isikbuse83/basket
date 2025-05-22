using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Informations;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
}