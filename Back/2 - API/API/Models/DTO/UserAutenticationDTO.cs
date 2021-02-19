using System;

namespace API.Models.DTO
{
    public class UserAutenticationDTO
    {
        public string UserName { get; set; }
        public DateTime? DataLogin { get; set; }
        public string Token { get; set; }
        public bool Autenticated { get; set; }
        public bool ForcaTrocaSenha { get; set; }
    }
}
