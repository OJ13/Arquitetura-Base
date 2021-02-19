namespace API.Models.DTO
{
    public class UserRedefinicaoSenhaDTO
    {
        public string UserName { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string RepeatNovaSenha { get; set; }
    }
}
