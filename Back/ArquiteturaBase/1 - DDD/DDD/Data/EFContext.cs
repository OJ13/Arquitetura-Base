using DDD.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DDD.Data
{
    public partial class EFContext : IdentityDbContext<Usuario, Perfil, Guid>
    {
        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<PerfilMenu> PerfilMenu { get; set; }
    }
}
