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

        public async Task<TReturn> ObtenerUsuarioAsync<TReturn>(string token)
        {
            var response = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Auth}", token);
            return response;
        }

        public async Task<TReturn> ReiniciarClaveDeAutenticacionAsync<TReturn, TParam>(TParam param, string token)
        {
            var response = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Auth}{param}/reiniciar_clave_de_autenticacion", token);
            return response;
        }

        public async Task<TReturn> ObtenerClaveDeAutenticacionAsync<TReturn, TParam>(TParam param, string token)
        {
            var response = await _http.PeticionHttpGet<TReturn>($"{_urlsGateway.Auth}{param}/obtener_clave_de_autenticacion", token);
            return response;
        }
        #endregion
    }
}