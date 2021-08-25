using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades;
using Super_Mercado.Data;
using Microsoft.EntityFrameworkCore;

namespace Super_Mercado.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly POSDbContext _context;

        public RolesController(POSDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }


        [HttpGet("GetRol/{id}")]
        public async Task<ActionResult<Role>> GetRol(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Rol == null)
            {
                return NotFound();
            }

            return Rol;
        }


        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateRole")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRole(Role Rol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Rol);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetRole", new { id = Rol.Id }, Rol);
        }

        [HttpPut("UpdateRole/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PutRole(int? id, Role Role)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != Role.Id)
            {
                return BadRequest();
            }


            _context.Entry(Role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                if (!RolExists(id.Value))
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




        // POST: Roles/Delete/5
        [HttpDelete("DeleteRole/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Role>> DeleteRole(int id)
        {
            var Rol = await _context.Roles.FindAsync(id);

            if (Rol == null)
                return NotFound();

            _context.Roles.Remove(Rol);
            await _context.SaveChangesAsync();

            return Rol;
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}

