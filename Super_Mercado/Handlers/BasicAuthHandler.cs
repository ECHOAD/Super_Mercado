using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Super_Mercado.Data;
using Entidades;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Super_Mercado.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly POSDbContext _context;

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            POSDbContext context)
            : base(options, logger, encoder, clock)
        {

            _context = context;
        }



        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authroization header was not found");

            try
            {
                var _authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var bytes = Convert.FromBase64String(_authenticationHeaderValue.Parameter);

                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string emailAdress = credentials[0];
                string password = credentials[1];

                Usuario user = await _context.Usuarios.Where(user => user.Email == emailAdress && user.Password == password).FirstOrDefaultAsync();

                if (user == null)
                    return AuthenticateResult.Fail("Usuario o contraseña invalida");
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);

                }


            }
            catch (Exception)
            {

                return AuthenticateResult.Fail("Ocurrio un error");
            }



            return AuthenticateResult.Fail("");
        }
    }
}
