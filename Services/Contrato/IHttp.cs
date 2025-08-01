using fileuploadweb.Services.Contrato.HttpMethods;

namespace fileuploadweb.Services.Contrato
{
    public interface IHttp : IHttpGet, IHttpPost, IHttpPut, IHttpDelete
    {
    }
}