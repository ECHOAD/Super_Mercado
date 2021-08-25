using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioDireccion
    {

        [Key]
        public int Id { get; set; }

        public int Id_Usuario { get; set; }


        public string Direccion { get; set; }


        [MaxLength(150, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]

        public string Comentario { get; set; }

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }



        [ForeignKey("Id_Usuario")]
        public Usuario Usuarios { get; set; }
    }
}
