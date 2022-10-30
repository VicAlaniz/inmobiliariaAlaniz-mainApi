using InmobiliariaAlaniz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;


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
        private readonly IWebHostEnvironment environment;


		public InmueblesController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
            this.environment = environment;
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
        public async Task<IActionResult> Post([FromBody] Inmueble inmueble)
        {
            
            try
            {
        
              if (inmueble.Imagen != null)
                {
                    var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                    var LocalPort = feature?.LocalPort.ToString();
                    var ipv4 = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
                    var ipConexion = "http://" + ipv4 + ":" + LocalPort + "/";

                    MemoryStream stream1 = new MemoryStream(Convert.FromBase64String(inmueble.Imagen));
                    IFormFile inmuebleFoto = new FormFile(stream1, 0, stream1.Length, "inmueble", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    string fileName = "inmueble_" + inmueble.PropietarioId + r.Next(0, 100000) + Path.GetExtension(inmuebleFoto.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    inmueble.Imagen = Path.Combine(ipConexion, "Uploads/", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        inmuebleFoto.CopyTo(stream);
                    }

                }
                await contexto.Inmueble.AddAsync(inmueble);
                contexto.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = inmueble.Id }, inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut()]
        public async Task<ActionResult> CambiarEstado([FromBody]Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
					{
						contexto.Inmueble.Update(inmueble);
						await contexto.SaveChangesAsync();
						return Ok(inmueble);
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