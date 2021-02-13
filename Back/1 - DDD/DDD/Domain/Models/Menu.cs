using System.Collections.Generic;

namespace DDD.Domain.Models
{
    public class Menu
    {
        public Menu()
        {
            PerfilMenu = new HashSet<PerfilMenu>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Rota { get; set; }
        public int? MenuPai { get; set; }
        public bool? Ativo { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<PerfilMenu> PerfilMenu { get; set; }
    }
}
