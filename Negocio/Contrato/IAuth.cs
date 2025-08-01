namespace fileuploadweb.Negocio.Contrato
{
    public interface IAuth
    {
        Task<TReturn> RegisterAsync<TReturn, TParam>(TParam param);
        Task<TReturn> LoginAsync<TReturn, TParam>(TParam param);
    }
}