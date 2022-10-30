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
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InquilinosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }
        // GET: api/<controller>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(await contexto.Contrato.Include(inq => inq.Inquilino).Include(i => i.Inmueble).SingleOrDefaultAsync(x => x.IdInmueble == id && x.Inmueble.Duenio.Email == usuario && x.FechaInicio <= DateTime.Today.Date && x.FechaFin >= DateTime.Today.Date));
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}