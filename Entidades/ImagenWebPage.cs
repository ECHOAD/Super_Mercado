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
    public class ImagenWebPage
    {
        [Key]
        public int Id { get; set; }

        public string Titulo { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Path { get; set; }

        public string FileName { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string Comentario { get; set; }



    }
}
