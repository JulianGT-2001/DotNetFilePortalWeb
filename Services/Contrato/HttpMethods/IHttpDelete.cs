namespace fileuploadweb.Services.Contrato.HttpMethods
{
    public interface IHttpDelete
    {
        Task<TReturn> PeticionHttpDelete<TReturn>(string url, string token);
    }
}