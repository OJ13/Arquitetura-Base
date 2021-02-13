using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Models
{
    public class Perfil : IdentityRole<Guid>
    {
        public bool Ativo { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    }
}
