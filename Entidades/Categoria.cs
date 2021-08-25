using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
 

        [MaxLength(30, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]

        public string Nombre { get; set; }




    }
}
