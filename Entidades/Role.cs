using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Descripcion del Rol es requerido.")]
        public string Role_Desc { get; set; }



    }
}
