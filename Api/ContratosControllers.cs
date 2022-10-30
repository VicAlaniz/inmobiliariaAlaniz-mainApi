using InmobiliariaAlaniz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaAlaniz.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public ContratosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<Contrato>> Contratos()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Contrato.Include(i => i.Inmueble).Include(inq => inq.Inquilino).Where(c => c.Inmueble.Duenio.Email == usuario && c.FechaInicio <= DateTime.Today.Date && c.FechaFin >= DateTime.Today.Date).ToList());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}