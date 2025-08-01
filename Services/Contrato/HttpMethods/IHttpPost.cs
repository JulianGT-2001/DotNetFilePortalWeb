namespace fileuploadweb.Services.Contrato.HttpMethods
{
    public interface IHttpPost
    {
        Task<TReturn> PeticionHttpPost<TParam, TReturn>(TParam param, string url, string? token = "");
        Task<TReturn> PeticionHttpPostArchivos<TReturn>(FormFileCollection files, string url, string token);
    }
}