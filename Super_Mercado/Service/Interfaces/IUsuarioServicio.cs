using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Service
{
    public interface IUsuarioServicio
    {
        public Task<UsuarioConToken> LoginAsync(Usuario user);
        public Task<Usuario> RegisterUserAsync(Usuario user);
        public Task<Usuario> GetUserByAccessTokenAsync(string accessToken);
        public Task<Usuario> RefreshTokenAsync(RefreshRequest refreshRequest);
        public Task<bool> IsFirstUser();
       

    }
}
