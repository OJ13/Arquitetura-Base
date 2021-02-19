using System.ComponentModel.DataAnnotations;
namespace API.Models.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
