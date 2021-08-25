
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entidades
{


    public class Usuario
    {

        public Usuario()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }


        [Key]
        public int Id { get; set; }




        [MinLength(13, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        [MaxLength(13, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        //[Index("TitleIndex", IsUnique = true)]
        public string Cedula { get; set; }

        [MaxLength(16, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string User { get; set; }


        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        public string Email { get; set; }




        [MaxLength(255)]
        public string Password { get; set; }




        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string Apellido { get; set; }

        [MaxLength(10, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres)")]
        public string Telefono { get; set; }

       
        public int? Id_Rol { get; set; }

        [ForeignKey("Id_Rol")]
        public Role Roles{ get; set; }



 



        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }





    }

}