using InmobiliariaAlaniz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaAlaniz.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;

		public InmueblesController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario).Single(e => e.Id == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.PropietarioId = contexto.Propietario.Single(e => e.Email == User.Identity.Name).Id;
                    contexto.Inmueble.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
/*
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]int id, Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid && contexto.Inmueble.AsNoTracking().Include(e=>e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name) != null)
                {
                    entidad.Id = id;
                    contexto.Inmueble.Update(entidad);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        */
        
        [HttpPost("CambiarEstado")]
        public async Task<ActionResult> CambiarEstado([FromForm]int id)
        {
            try
            {
                if(ModelState.IsValid)
					{
						var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
						var inmueble = contexto.Inmueble.Include(x => x.Duenio).Where(y => y.Duenio.Email == propietario.Email).SingleOrDefault(z => z.Id == id);
						inmueble.Estado = !inmueble.Estado;
						contexto.Inmueble.Update(inmueble);
						await contexto.SaveChangesAsync();
						return Ok(inmueble.Estado);
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

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    contexto.Inmueble.Remove(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
		}
/*
		// DELETE api/<controller>/5
		[HttpDelete("BajaLogica/{id}")]
		public async Task<IActionResult> BajaLogica(int id)
		{
			try
			{
				var entidad = contexto.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
				if (entidad != null)
				{
					entidad.Superficie = -1;//cambiar por estado = 0
					contexto.Inmueble.Update(entidad);
					contexto.SaveChanges();
					return Ok();
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}*/
	}
}