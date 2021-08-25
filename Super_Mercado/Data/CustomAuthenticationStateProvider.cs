using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Blazored.LocalStorage;
using Super_Mercado.Service;
using System.Net.Http;
using Entidades;

namespace Super_Mercado.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorageService { get; }
        public IUsuarioServicio _usuarioServicio { get; set; }
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
            IUsuarioServicio usuarioServicio,
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
        }


        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");

            ClaimsIdentity identity;

            if (accessToken != null && accessToken != string.Empty)
            {
               
                Usuario user = await _usuarioServicio.GetUserByAccessTokenAsync(accessToken);

              
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(UsuarioConToken user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public void MarkUserAsLogout()
        {
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);



            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }



        private ClaimsIdentity GetClaimsIdentity(Usuario user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Email != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, user.Email),
                                    new Claim(ClaimTypes.Role, user.Roles.Role_Desc),
                                    new Claim("Cedula", user.Cedula),
                                    new Claim("User",user.User),
                                }, "apiauth_type");
            }

            return claimsIdentity;
        }

    }
}
