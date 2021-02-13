using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Models
{
    public class Usuario : IdentityUser<Guid>
    {
        public bool Ativo { get; set; }
        public bool Logado { get; set; }
        public bool FirstLogin { get; set; }
        public DateTime? DataUltimoLogin { get; set; }
        public DateTime? DataUltimaTrocaSenha { get; set; }
        public bool ForcaTrocaSenha { get; set; }
        public Guid? PerfilId { get; set; }
        public DateTime DataCriacao { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    }
}
