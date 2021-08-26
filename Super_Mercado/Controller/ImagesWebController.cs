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
    public class ImagesWebController : ControllerBase
    {
        private readonly POSDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IHttpContextAccessor _contextAcessor;

        public ImagesWebController(POSDbContext context, IWebHostEnvironment host, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _host = host;
            _contextAcessor = contextAccessor;

        }

        [HttpGet("GetImages")]
        public async Task<ActionResult<IEnumerable<ImagenWebPage>>> GetProductos()
        {
            return await _context.ImagenesWebPages.Select(i => new ImagenWebPage
            {
                Id = i.Id,
                Titulo = i.Titulo,
                FileName = i.FileName,
                Path = i.Path.Replace(_host.WebRootPath, "")
            }
            ).ToListAsync();
            //Imagen = $"{_contextAcessor.HttpContext.Request.Scheme}://{_contextAcessor.HttpContext.Request.Host}/Images/Producto/Producto_{p.Nombre}.png",
        }


        [HttpGet("GetImage/{id}")]
        public async Task<ActionResult<ImagenWebPage>> GetImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenWebPage = await _context.ImagenesWebPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagenWebPage == null)
            {
                return NotFound();
            }

            return imagenWebPage;
        }


        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateImage")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostImages(ImagenWebPage img)
        {
            if (ModelState.IsValid)
            {
                if (img.Path != string.Empty)
                {

                    try
                    {



                        await _context.ImagenesWebPages.AddAsync(img);

                        await _context.SaveChangesAsync();

                    }
                    catch (Exception)
                    {

                        await _context.Database.RollbackTransactionAsync();
                    }

                }
            }
            return CreatedAtAction("GetProducto", new { id = img.Id }, img);
        }

        [HttpPut("UpdateImage/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PutImages(int? id, ImagenWebPage img)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != img.Id)
            {
                return BadRequest();
            }


            _context.Entry(img).State = EntityState.Modified;

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
        [HttpDelete("DeleteImage/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<ImagenWebPage>> DeleteImage(int id)
        {
            var img = await _context.ImagenesWebPages.FindAsync(id);

            if (img == null)
                return NotFound();

            _context.ImagenesWebPages.Remove(img);
            await _context.SaveChangesAsync();

            return img;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
