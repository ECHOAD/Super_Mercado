using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }


        [MaxLength(30, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es requerido.")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El {0} es requerido.")]
        public int Stock { get; set; }

        public int Id_Categoria { get; set; }

        [ForeignKey("Id_Categoria")]
        public Categoria Categorias { get; set; }

        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Detalle { get; set; }


        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Imagen { get; set; }

   

    }
}
