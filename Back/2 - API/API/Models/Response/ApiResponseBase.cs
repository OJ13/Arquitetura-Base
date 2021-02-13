namespace API.Models.Response
{
    public class ApiResponseBase
    {
        public ApiResponseBase(string mensagem, bool sucesso = true)
        {
            Message = mensagem;
            Sucesso = sucesso;
        }

        public string Message { get; set; }
        public bool Sucesso { get; private set; }
    }
}
