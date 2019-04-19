using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fox.Models;
using Input = Fox.Models.Transfer.Input;
//using Output = Fox.Models.Transfer.Output;

namespace Fox.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/usuario")]
    public class UsuarioController : Controller
    {
        private IUsuarioService Usuarios { get; }

        public UsuarioController(IUsuarioService users)
        {
            Usuarios = users;
        }

        [HttpPost("autenticacion")]
        public IActionResult Autenticacion([FromBody]Input.Usuario.Autenticacion data)
        {
            try
            {
                var user = Usuarios.Authenticate(data.Rut, data.Password);

                if (user == null) { return BadRequest(new RequestError("Credenciales inválidas")); }

                return Ok(user);
            } catch (NullReferenceException)
            {
                return BadRequest(new RequestError("Solicitud incompleta o inválida"));
            } catch (Exception e)
            {
                return BadRequest(new RequestError("Error no controlado: " + e.Message));
            }
        }

        [Authorize(Roles = "Farmaceutico, Administrador")]
        [HttpGet]
        public IActionResult Get([FromBody]Input.Usuario.Get data) {
            try
            {
                var result = Usuarios.GetAll();
                if (data.Roles != null) { result = result.Where(x => x.Roles == data.Roles); }
                if (data.Cantidad != null) { result = result.Take(data.Cantidad.Value); }

                return Ok(result.Select(x => new
                {
                    rut = x.Rut,
                    email = x.Email,
                    fecha_nacimiento = x.FechaNacimiento,
                    roles = x.Roles
                }));
            } catch (NullReferenceException)
            {
                return BadRequest(new RequestError("Solicitud incompleta o inválida"));
            } catch(Exception e)
            {
                return BadRequest(new RequestError("Error no controlado: " + e.Message));
            }
        }

        [Authorize(Roles = "Farmaceutico, Administrador")]
        [HttpPost]
        public IActionResult Post([FromBody]Input.Usuario.Post data)
        {
            Usuario usuario;
            try
            {
                usuario = new Usuario
                {
                    Rut = data.Rut,
                    Password = Usuario.ComputeHash(data.Password),
                    Email = data.Email,
                    FechaNacimiento = data.FechaNacimiento,
                    Roles = data.Roles
                };

                Usuarios.CreateUsuario(usuario);
                return Created($"api/v1/usuario/{usuario.Rut}", new
                {
                    rut = usuario.Rut,
                    email = usuario.Email,
                    roles = usuario.Roles,
                    creacion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                });
            } catch (NullReferenceException)
            {
                return BadRequest(new { error = "Datos incompletos o inválidos" });
            } catch (Exception e)
            {
                return BadRequest(new RequestError("Error no controlado: " + e.Message));
            }
        }
    }
}