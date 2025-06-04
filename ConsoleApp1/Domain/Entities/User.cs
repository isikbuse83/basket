using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Domain.Entities 
{
    public class User
    {
        [Key] public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string Password { get; set; }
        
    }
}