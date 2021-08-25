using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Data
{
    public class UsuarioModel:Usuario
    {




        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


    }
}
