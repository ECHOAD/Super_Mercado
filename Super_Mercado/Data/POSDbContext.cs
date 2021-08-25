using Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Mercado.Data
{
    public class POSDbContext : DbContext
    {


        public POSDbContext(DbContextOptions<POSDbContext> options) : base(options)
        {

        }





        public virtual DbSet<Usuario> Usuarios { get; set; }

        public virtual DbSet<UsuarioDireccion> UsuariosDirecciones { get; set; }


        public virtual DbSet<Producto> Productos { get; set; }

        public virtual DbSet<Categoria> Categorias { get; set; }
       
        public virtual DbSet<Ordenes> Ordenes { get; set; }

        public virtual DbSet<Orden_Detalle> Ordenes_Detalles { get; set; }


        public virtual DbSet<Role> Roles{ get; set; }

        public virtual DbSet<ImagenWebPage> ImagenesWebPages { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }








    }
}
