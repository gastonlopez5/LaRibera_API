using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaRibera_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LaRibera_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Administrador")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;
        private readonly IHostingEnvironment environment;
        private readonly Utilidades utilidades = new Utilidades();

        public UsuariosController(DataContext context, IConfiguration config, IHostingEnvironment environment)
        {
            _context = context;
            this.config = config;
            this.environment = environment;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Msj msj = new Msj();
                    string foto = null;
                    Usuario user = new Usuario();

                    if (_context.Usuarios.Any(x => x.Email == usuario.Email))
                    {
                        return BadRequest();
                    }
                    else
                    {

                        if (usuario.FotoPerfil != null)
                        {
                            foto = usuario.FotoPerfil;
                            usuario.FotoPerfil = "a";
                        }

                        _context.Usuarios.Add(usuario);
                        await _context.SaveChangesAsync();

                        user = _context.Usuarios.FirstOrDefault(x => x.Email == usuario.Email);

                        if (usuario.FotoPerfil != null)
                        {
                            var fileName = "fotoperfil.png";
                            string wwwPath = environment.WebRootPath;
                            string path = wwwPath + "/fotoperfil/" + user.Id;
                            string filePath = "/FotoPerfil/" + user.Id + "/" + fileName;
                            string pathFull = Path.Combine(path, fileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(pathFull, FileMode.Create))
                            {
                                var bytes = Convert.FromBase64String(foto);
                                fileStream.Write(bytes, 0, bytes.Length);
                                fileStream.Flush();
                                user.FotoPerfil = filePath;
                            }

                            _context.Usuarios.Update(user);
                            _context.SaveChanges();
                        }

                        if (usuario.RolId == 4)
                        {
                            string pass = utilidades.GenerarCodigo();

                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: pass,
                                salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));

                            user.Clave = hashed;

                            _context.Usuarios.Update(user);
                            _context.SaveChanges();

                            utilidades.EnciarCorreo(usuario.Email,
                                "Club La Ribera - Alta de usuario",
                                "<h2>Bienvenido " + usuario.Apellido + " " + usuario.Nombre + "!!!</h2>" +
                                "<p>Recuerda modificar la contraseña cuando ingreses.</p>" +
                                "<br />" +
                                "<p>Tu contraseña es: " + pass + "</p>" +
                                "<p>Este es un email generado automáticamente, no lo respondas.");

                            msj.Mensaje = "Usuario registrado exitosamente! Recibirá un email con la contraseña para ingresar";
                        }
                        else
                        {
                            msj.Mensaje = "Usuario registrado exitosamente! Una vez que te aprueben, reciviras un email con tu contraseña";
                        }

                        return Ok(msj);
                    }
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            //return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        // POST: api/Usuarios
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                var p = await _context.Usuarios
                        .Include(a => a.TipoUsuario)
                        .FirstOrDefaultAsync(a => a.Email == login.Email);

                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["Llave"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new []
                    {
                        new Claim(ClaimTypes.Email, p.Email),
                        new Claim(ClaimTypes.Role, p.TipoUsuario.Rol),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var expiration = DateTime.Now.AddMinutes(60);

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: expiration,
                        signingCredentials: credenciales
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("recuperarpass")]
        [AllowAnonymous]
        public async Task<ActionResult> RecuperarPass([FromBody] string email)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == email);
                Msj msj = new Msj();

                if (usuario != null)
                {
                    string pass = utilidades.GenerarCodigo();

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pass,
                    salt: System.Text.Encoding.ASCII.GetBytes("Salt"),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                    usuario.Clave = hashed;

                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    utilidades.EnciarCorreo(usuario.Email,
                        "Club La Ribera - Blanqueo de clave",
                        "<h2>Recuperación de clave para " + usuario.Apellido + " " + usuario.Nombre + "</h2>" +
                        "<p>Recuerda modificar la contraseña cuando ingreses.</p>" +
                        "<br />" +
                        "<p>Tu contraseña es: " + pass + "</p>" +
                        "<p>Este es un email generado automáticamente, no lo respondas.");

                    msj.Mensaje = "Recibirá en su correo la contraseña para ingresar";

                    return Ok(msj);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
