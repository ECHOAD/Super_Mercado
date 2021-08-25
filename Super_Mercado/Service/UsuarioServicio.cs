using Entidades;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Super_Mercado.Data;
using Newtonsoft.Json;

namespace Super_Mercado.Service
{
    public class UsuarioServicio : IUsuarioServicio
    {

        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }

        public UsuarioServicio(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            httpClient.BaseAddress = new Uri(_appSettings.SuperMercadoBaseAdress);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");

            _httpClient = httpClient;
        }

        public async Task<bool> IsFirstUser()
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Usuarios/GetIsFirstUser");


            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();
            var returnedExistence = JsonConvert.DeserializeObject<bool>(responseBody);

            return await Task.FromResult(returnedExistence);

        }

        public async Task<Usuario> GetUserByAccessTokenAsync(string accessToken)
        {
            string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Usuarios/GetUserByAccessToken");
            requestMessage.Content = new StringContent(serializedRefreshRequest);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<Usuario>(responseBody);

            return await Task.FromResult(returnedUser);
        }

        public async Task<UsuarioConToken> LoginAsync(Usuario user)
        {

            user.Password = Utility.Encrypt(user.Password);
            string serializedUser = JsonConvert.SerializeObject(user);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Usuarios/Login");
            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<Usuario>(responseBody);

            var Tokens = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(responseBody);


            var usuarioConToken = new UsuarioConToken(returnedUser);
            
            usuarioConToken.AccessToken= Tokens["accessToken"];
            usuarioConToken.RefreshToken = Tokens["refreshToken"];





            return await Task.FromResult(usuarioConToken);
        }

        public async Task<Usuario> RefreshTokenAsync(RefreshRequest refreshRequest)
        {
            string serializedUser = JsonConvert.SerializeObject(refreshRequest);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Usuarios/RefreshToken");
            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<Usuario>(responseBody);

            return await Task.FromResult(returnedUser);
        }

        public async Task<Usuario> RegisterUserAsync(Usuario user)
        {
            user.Password = Utility.Encrypt(user.Password);


            string serializedUser = JsonConvert.SerializeObject(user);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Usuarios/RegistrarUsuario");
            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<Usuario>(responseBody);

            return await Task.FromResult(returnedUser);
        }

        
    }
}
