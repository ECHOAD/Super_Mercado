using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Super_Mercado.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Super_Mercado.Controller
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {



        private readonly POSDbContext _context;
        private readonly JWTSettings _jwtSettings;

        public UsuariosController(POSDbContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtSettings = jwtsettings.Value;

        }


        [HttpGet("GetIsFirstUser")]
        public async Task<ActionResult<bool>> GetIsFirstUser()
        {

            var lst = await _context.Usuarios.ToListAsync();

            if (lst.Count == 0)
            {
                return true;
            }
            return false;

        }




        [HttpGet("GetAllUsuarios")]
        public async Task<ActionResult<List<Usuario>>> GetAllUsers()
        {

            var lst = await _context.Usuarios.Include(x => x.Roles).Select(
                u => new Usuario
                {
                    Id=u.Id,
                    User=u.User,
                    Cedula=u.Cedula,
                    Nombre=u.Nombre,
                    Apellido=u.Apellido,
                    Email=u.Email,
                    Id_Rol=u.Id_Rol,
                    Roles=u.Roles,
                    Telefono=u.Telefono
                    
                }).ToListAsync();
        
            return lst;

        }




        [HttpPost("RegistrarUsuario")]
        public async Task<ActionResult<UsuarioConToken>> RegistrarUsuario([FromBody] Usuario user)
        {

            var Isfirst_user = await GetIsFirstUser();

            if (Isfirst_user.Value)
            {
                await _context.Roles.AddAsync(new Role { Role_Desc = "Admin" });

                await _context.SaveChangesAsync();

                var rol = await _context.Roles.Where(x => x.Role_Desc == "Admin").FirstOrDefaultAsync();

                user.Id_Rol = rol.Id;
                user.Roles = rol;


            }

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            user = await _context.Usuarios.Include(u => u.Roles)
                .Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            UsuarioConToken usuarioConToken = null;

            if (user != null)
            {


                RefreshToken refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                usuarioConToken = new UsuarioConToken(user);
                usuarioConToken.RefreshToken = refreshToken.Token;
            }

            if (usuarioConToken == null)
            {
                return NotFound();
            }

            usuarioConToken.AccessToken = GenerateAccessToken(user.Id);
            return usuarioConToken;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioConToken>> Login([FromBody] Usuario user)
        {
            user = await _context.Usuarios.Include(u => u.Roles)
                                        .Where(u => u.Email == user.Email
                                                && u.Password == user.Password).FirstOrDefaultAsync();

            UsuarioConToken usuarioConToken = null;

            if (user != null)
            {

                RefreshToken refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);

                usuarioConToken = new(user);
                usuarioConToken.RefreshToken = refreshToken.Token;



            }

            if (usuarioConToken == null)
            {
                return NotFound();
            }
            return usuarioConToken;
        }


        private string GenerateAccessToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private static RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.Expiry_Date = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }


        [HttpGet("GetUsuario/{id}")]
        public async Task<ActionResult<Usuario>> GetUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // GET: api/Users/5
        [HttpGet("GetUserDetails/{id}")]
        public async Task<ActionResult<Usuario>> GetUserDetails(int id)
        {
            var user = await _context.Usuarios.Include(u => u.Roles)
                                            .Where(u => u.Id == id)
                                            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        public async Task<ActionResult<Usuario>> GetUsuarios()
        {
            string email = HttpContext.User.Identity.Name;

            var usuario = await _context.Usuarios.Where(usuario => usuario.Email == email).FirstOrDefaultAsync();

            usuario.Password = null;

            return usuario;
        }

        // GET: api/Users
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UsuarioConToken>> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            Usuario user = await GetUserFromAccessToken(refreshRequest.AccessToken);

            if (user != null && ValidateRefreshToken(user, refreshRequest.RefreshToken))
            {
                UsuarioConToken usuarioConToken = new(user);
                usuarioConToken.AccessToken = GenerateAccessToken(user.Id);

                return usuarioConToken;
            }

            return null;
        }


        private bool ValidateRefreshToken(Usuario user, string refreshToken)
        {

            RefreshToken refreshTokenUser = _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                                                .OrderByDescending(rt => rt.Expiry_Date)
                                                .FirstOrDefault();

            if (refreshTokenUser != null && refreshTokenUser.Id_Usuario == user.Id
                && refreshTokenUser.Expiry_Date > DateTime.UtcNow)
            {
                return true;
            }

            return false;
        }

        // GET: api/Users
        [HttpPost("GetUserByAccessToken")]
        public async Task<ActionResult<Usuario>> GetUserByAccessToken([FromBody] string accessToken)
        {
            Usuario user = await GetUserFromAccessToken(accessToken);

            if (user != null)
            {
                return user;
            }

            return null;
        }



        private async Task<Usuario> GetUserFromAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst(ClaimTypes.Name)?.Value;

                    return await _context.Usuarios.Include(u => u.Roles)
                                        .Where(u => u.Id == Convert.ToInt32(userId)).FirstOrDefaultAsync();
                }
            }
            catch (Exception)
            {
                return new Usuario();
            }

            return new Usuario();
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<Usuario>> PostUser(Usuario user)
        {
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        // DELETE: api/Users/5
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<Usuario>> DeleteUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }


        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> PutUser(int id, Usuario user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
    }




}
