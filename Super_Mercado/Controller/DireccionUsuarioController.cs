using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Super_Mercado.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super_Mercado.Controller
{


    [Route("api/[controller]")]
    [ApiController]
    public class DireccionUsuarioController : ControllerBase
    {
        private readonly POSDbContext _context;

        public DireccionUsuarioController(POSDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetDirecciones/{username}")]

        public async Task<ActionResult<IEnumerable<UsuarioDireccion>>> GetDirecciones(string username)
        {

            var oUser = await _context.Usuarios.FirstOrDefaultAsync(x => x.User == username);


            if (oUser == null)
            {
                return NotFound();
            }


            var lst_direcciones = await _context.UsuariosDirecciones.
                Where(m => m.Usuarios == oUser).
                Include(x => x.Usuarios).ToListAsync();


            if (lst_direcciones == null)
            {
                return NotFound();
            }

            return lst_direcciones;
        }


        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateUsuarioDireccion")]

        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCategoria(UsuarioDireccion direccion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oUser = await _context.Usuarios.FirstOrDefaultAsync(x => x.User == direccion.Usuarios.User);

                    if (oUser == null)
                    {
                        return BadRequest();
                    }

                    direccion.Usuarios = oUser;

                    direccion.Id_Usuario = oUser.Id;

                    _context.Add(direccion);
                    await _context.SaveChangesAsync();

                }
                catch (Exception)
                {

                    _context.Database.RollbackTransaction();
                }

            }
            return CreatedAtAction("GetCategoria", new { id = direccion.Id }, direccion);
        }

        [HttpPut("UpdateDireccionUsuario/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PutDireccionUsuario(int? id, UsuarioDireccion usuarioDireccion)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != usuarioDireccion.Id)
            {
                return BadRequest();
            }


            _context.Entry(usuarioDireccion).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                if (!CategoriaExists(id.Value))
                {
                    return NotFound();

                }
                else
                {
                    throw;

                }

            }
            return NoContent();

        }




        // POST: Categorias/Delete/5
        [HttpDelete("DeleteCategoria/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Categoria>> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }


    }
}