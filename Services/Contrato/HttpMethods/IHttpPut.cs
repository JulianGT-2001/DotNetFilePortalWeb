namespace fileuploadweb.Services.Contrato.HttpMethods
{
    public interface IHttpPut
    {
        Task<TReturn> PeticionHttpPut<TParam, TReturn>(TParam param, string url, string token);
    }
}