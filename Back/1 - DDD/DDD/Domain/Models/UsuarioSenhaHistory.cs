using System;

namespace DDD.Domain.Models
{
    public partial class UsuarioSenhaHistory
    {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string PasswordHash { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
