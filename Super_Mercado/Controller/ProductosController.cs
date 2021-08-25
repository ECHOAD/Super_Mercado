using Microsoft.AspNetCore.Mvc;
using Super_Mercado.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Super_Mercado.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly POSDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IHttpContextAccessor _contextAcessor;

        public ProductosController(POSDbContext context, IWebHostEnvironment host, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _host = host;
            _contextAcessor = contextAccessor;

        }

        [HttpGet("GetProductos")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.Select(p => new Producto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Id_Categoria = p.Id_Categoria,
                Categorias = p.Categorias,
                Stock = p.Stock,
                Imagen = p.Imagen.Replace(_host.WebRootPath,""),
                Detalle = p.Detalle

            }
            ).ToListAsync();
            //Imagen = $"{_contextAcessor.HttpContext.Request.Scheme}://{_contextAcessor.HttpContext.Request.Host}/Images/Producto/Producto_{p.Nombre}.png",

        }


        [HttpGet("GetProducto/{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Producto == null)
            {
                return NotFound();
            }

            return Producto;
        }


        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateProducto")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostProducto(Producto Producto)
        {
            if (ModelState.IsValid)
            {
                if (Producto.Imagen != string.Empty)
                {

                    try
                    {
                        var categoria = await _context.Categorias.Where(x => x.Id == Producto.Id_Categoria).FirstOrDefaultAsync();

                        Producto.Categorias = categoria;
                        Producto.Id_Categoria=categoria.Id;

                        await _context.Productos.AddAsync(Producto);

                       await _context.SaveChangesAsync();

                    }
                    catch (Exception)
                    {

                        await _context.Database.RollbackTransactionAsync();
                    }

                }


            }
            return CreatedAtAction("GetProducto", new { id = Producto.Id }, Producto);
        }

        [HttpPut("UpdateProducto/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PutProducto(int? id, Producto Producto)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != Producto.Id)
            {
                return BadRequest();
            }


            _context.Entry(Producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                if (!ProductoExists(id.Value))
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




        // POST: Productos/Delete/5
        [HttpDelete("DeleteProducto/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            var Producto = await _context.Productos.FindAsync(id);

            if (Producto == null)
                return NotFound();

            _context.Productos.Remove(Producto);
            await _context.SaveChangesAsync();

            return Producto;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
