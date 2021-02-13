namespace DDD.Domain.Models
{
    public class PerfilMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int PerfilId { get; set; }
        public bool? Ativo { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
