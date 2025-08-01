namespace fileuploadweb.Services.Contrato.HttpMethods
{
    public interface IHttpGet
    {
        Task<TReturn> PeticionHttpGet<TReturn>(string url, string? token);
    }
}