using System.Text;
using System.Text.Json;
using fileuploadweb.Services.Contrato;

namespace fileuploadweb.Services.Logica
{
    public class HttpLogica : IHttp
    {
        #region Atributos
        private readonly HttpClient _http;
        #endregion

        #region Constructor
        public HttpLogica(HttpClient http)
        {
            _http = http;
        }
        #endregion

        #region Metodos
        public async Task<TReturn> PeticionHttpDelete<TReturn>(string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (TReturn)(object)true;
            }

            return (TReturn)(object)false;
        }

        public async Task<TReturn> PeticionHttpGet<TReturn>(string url, string? token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            using HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return default!;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (typeof(TReturn) == typeof(string) && !jsonResponse.TrimStart().StartsWith("{") && !jsonResponse.TrimStart().StartsWith("["))
            {
                // Convierte el contenido RAW a base64
                var rawBytes = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(rawBytes);
                return (TReturn)(object)base64;
            }

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return default!;
            }

            try
            {
                TReturn? objectDeserialized = JsonSerializer.Deserialize<TReturn>(jsonResponse);

                return objectDeserialized!;
            }
            catch (JsonException)
            {
                return default!;
            }
        }

        public async Task<TReturn> PeticionHttpPost<TParam, TReturn>(TParam param, string url, string? token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            request.Content = new StringContent(
                JsonSerializer.Serialize(param),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _http.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Si la respuesta es exitosa y vacía, retorna true si TReturn es bool, sino default
            if (response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(jsonResponse))
            {
                if (typeof(TReturn) == typeof(bool))
                    return (TReturn)(object)true;
                return default!;
            }

            // Si la respuesta no es exitosa, retorna default
            if (!response.IsSuccessStatusCode)
            {
                return default!;
            }

            // Si la respuesta es vacía, retorna default
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return default!;
            }

            try
            {
                TReturn? objectDeserialized = JsonSerializer.Deserialize<TReturn>(jsonResponse);
                return objectDeserialized!;
            }
            catch (JsonException)
            {
                return default!;
            }
        }

        public async Task<TReturn> PeticionHttpPostArchivos<TReturn>(FormFileCollection files, string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    content.Add(streamContent, "files", file.FileName);
                }
            }

            request.Content = content;

            using HttpResponseMessage response = await _http.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Si la respuesta es exitosa y vacía, retorna true si TReturn es bool, sino default
            if (response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(jsonResponse))
            {
                if (typeof(TReturn) == typeof(bool))
                    return (TReturn)(object)true;
                return default!;
            }

            // Si la respuesta no es exitosa, retorna default
            if (!response.IsSuccessStatusCode)
            {
                return default!;
            }

            // Si la respuesta es vacía, retorna default
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return default!;
            }

            try
            {
                TReturn? objectDeserialized = JsonSerializer.Deserialize<TReturn>(jsonResponse);
                return objectDeserialized!;
            }
            catch (JsonException)
            {
                return default!;
            }
        }

        public async Task<TReturn> PeticionHttpPut<TParam, TReturn>(TParam param, string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(param),
                Encoding.UTF8,
                "application/json"
            );

            request.Content = jsonContent;

            using HttpResponseMessage response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return default!;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return default!;
            }

            try
            {
                TReturn? objectDeserialized = JsonSerializer.Deserialize<TReturn>(jsonResponse);
                return objectDeserialized!;
            }
            catch (JsonException)
            {

                throw;
            }
        }
        #endregion
    }
}