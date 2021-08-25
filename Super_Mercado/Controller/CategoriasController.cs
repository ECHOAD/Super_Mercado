using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades;
using Super_Mercado.Data;

namespace Super_Mercado.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly POSDbContext _context;

        public CategoriasController(POSDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetCategorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }


        [HttpGet("GetCategoria/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }


        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateCategoria")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("UpdateCategoria/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PutCategoria(int? id, Categoria categoria)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != categoria.Id)
            {
                return BadRequest();
            }


            _context.Entry(categoria).State = EntityState.Modified;

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
