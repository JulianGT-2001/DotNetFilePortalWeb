using fileuploadweb.Models.Dto;
using fileuploadweb.Negocio.Contrato;
using fileuploadweb.Services.Contrato;
using Microsoft.Extensions.Options;

namespace fileuploadweb.Negocio.Logica
{
    public class FileLogica : IFile
    {
        #region Atributos
        private readonly GatewayUrls _urlsGateway;
        private readonly IHttp _http;
        #endregion

        #region Constructor
        public FileLogica(IOptions<GatewayUrls> urlsGateway, IHttp http)
        {
            _urlsGateway = urlsGateway.Value;
            _http = http;
        }
        #endregion

        #region Metodos
        public async Task<TReturn> AddFileAsync<TReturn>(FormFileCollection param, string token)
        {
            var respuesta = await _http.PeticionHttpPostArchivos<TReturn>(param, _urlsGateway.Files, token);
            return respuesta;
        }

        public async Task<TReturn> DeleteFilesAsync<TParam, TReturn>(TParam param, string token)
        {
            var respuesta = await _http.PeticionHttpDelete<TReturn>($"{_urlsGateway.Files}/{param}", token);
            return respuesta;
        }

        public async Task<TReturn> GetFileAsync<TParam, TReturn>(TParam param, string token)
        {
            var respuesta = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Files}/{param}", token);
            return respuesta;
        }

        public async Task<TReturn> GetFileContentAsync<TParam, TReturn>(TParam param, string token)
        {
            var respuesta = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Files}/{param}/download", token);
            return respuesta;
        }

        public async Task<TReturn> GetFilesAsync<TReturn>(string token)
        {
            var respuesta = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Files}", token);
            return respuesta;
        }
        #endregion
    }
}