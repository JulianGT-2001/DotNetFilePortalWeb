namespace fileuploadweb.Negocio.Contrato
{
    public interface IFile
    {
        Task<TReturn> AddFileAsync<TReturn>(FormFileCollection param, string token);
        Task<TReturn> GetFilesAsync<TReturn>(string token);
        Task<TReturn> DeleteFilesAsync<TParam, TReturn>(TParam param, string token);
        Task<TReturn> GetFileAsync<TParam, TReturn>(TParam param, string token);
        Task<TReturn> GetFileContentAsync<TParam, TReturn>(TParam param, string token);
    }
}