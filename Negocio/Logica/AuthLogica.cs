using fileuploadweb.Models.Dto;
using fileuploadweb.Negocio.Contrato;
using fileuploadweb.Services.Contrato;
using Microsoft.Extensions.Options;

namespace fileuploadweb.Negocio.Logica
{
    public class AuthLogica : IAuth
    {
        #region Atributos
        private readonly GatewayUrls _urlsGateway;
        private readonly IHttp _http;
        #endregion

        #region Constructor
        public AuthLogica(IOptions<GatewayUrls> urlsGateway, IHttp http)
        {
            _urlsGateway = urlsGateway.Value;
            _http = http;
        }
        #endregion

        #region Metodos
        public async Task<TReturn> LoginAsync<TReturn, TParam>(TParam param)
        {
            var response = await _http.PeticionHttpPost<TParam, TReturn>(param, $"{_urlsGateway.Auth}login");
            return response;
        }

        public async Task<TReturn> RegisterAsync<TReturn, TParam>(TParam param)
        {
            var response = await _http.PeticionHttpPost<TParam, TReturn>(param, $"{_urlsGateway.Auth}register");
            return response;
        }
        #endregion
    }
}