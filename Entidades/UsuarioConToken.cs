using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioConToken:Usuario
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UsuarioConToken(Usuario user)
        {
            this.Id = user.Id;
            this.Cedula = user.Cedula;
            this.Email = user.Email;
            this.User = user.User;
            this.Nombre = user.Nombre;
            this.Apellido=user.Apellido;
            this.Roles=user.Roles;
            this.Telefono=user.Telefono;

        }
    }
}
