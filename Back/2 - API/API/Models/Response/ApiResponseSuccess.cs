namespace API.Models.Response
{
    public class ApiResponseSuccess<T> : ApiResponseBase
    {
        public T Data { get; set; }
        public int? Total { get; set; }
        
        public ApiResponseSuccess(T data, string mensagem = null, int? total = null)
            : base(mensagem)
        {
            Total = total;
            Data = data;
        }
    }
}
