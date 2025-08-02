namespace fileuploadweb.Negocio.Contrato
{
    public interface IAuth
    {
        Task<TReturn> RegisterAsync<TReturn, TParam>(TParam param);
        Task<TReturn> LoginAsync<TReturn, TParam>(TParam param);
        Task<TReturn> ObtenerUsuarioAsync<TReturn>(string token);
        Task<TReturn> ReiniciarClaveDeAutenticacionAsync<TReturn, TParam>(TParam param, string token);
        Task<TReturn> ObtenerClaveDeAutenticacionAsync<TReturn, TParam>(TParam param, string token);
    }
}