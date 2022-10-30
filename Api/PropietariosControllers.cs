using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using InmobiliariaAlaniz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace InmobiliariaAlaniz.Api
{
    [Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietariosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;

		public PropietariosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				
				var usuario = User.Identity.Name;
				return await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// POST api/<controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/<controller>/5
		[HttpPut()]
		public async Task<IActionResult> Put([FromBody] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					contexto.Propietario.Update(entidad);
                    await contexto.SaveChangesAsync();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

		}

		[HttpPut("cambiarClave")]
        public async Task<IActionResult> cambiarClave([FromForm] CambiarClave usuario)
        {
            try
            {
                string hashedPassVieja = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: usuario.PassVieja,
                   salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 1000,
                   numBytesRequested: 256 / 8));
                string hashedPassNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                       password: usuario.PassNueva,
                       salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                       prf: KeyDerivationPrf.HMACSHA1,
                       iterationCount: 1000,
                       numBytesRequested: 256 / 8));


                var p = await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == User.Identity.Name);
                string PassVieja = p.Clave;

                if (PassVieja == hashedPassVieja)
                {

                    p.Clave = hashedPassNueva;
                    contexto.Propietario.Update(p);
                    await contexto.SaveChangesAsync();
                    return Ok(p);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/<controller>
        [HttpGet("token")]
        public async Task<ActionResult> token()
        {
            try
            {
                var perfil = new
                {
                    Email = User.Identity.Name,
                    Nombre = User.Claims.First(x => x.Type == "FullName").Value,
                    Rol = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value
                };

                Random rand = new Random(Environment.TickCount);
                string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
                string nuevaClave = "";
                for (int i = 0; i < 8; i++)
                {
                    nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
                }

                String nuevaClaveSin = nuevaClave;

                nuevaClave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: nuevaClave,
                            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));


                Propietario original = await contexto.Propietario.AsNoTracking().FirstOrDefaultAsync(x => x.Email == perfil.Email);
                original.Clave = nuevaClave;
                contexto.Propietario.Update(original);
                await contexto.SaveChangesAsync();

                var message = new MimeKit.MimeMessage();
                message.To.Add(new MailboxAddress(perfil.Nombre, "mvicalaniz@gmail.com"));
                message.From.Add(new MailboxAddress(perfil.Nombre, "mvicalaniz@gmail.com"));
                message.Subject = "Testing";
                message.Body = new TextPart("html")
                {
                    Text = @$"<h1>Hola {perfil.Nombre}!</h1>
					<p> Tu nueva clave es: <b>{nuevaClaveSin}</b></p><br>
                    <p> Gracias!</p>",
                };

                message.Headers.Add("Encabezado", "Valor");
                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.ServerCertificateValidationCallback = (object sender,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
                { return true; };
                client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                //			client.Authenticate(config["SMTPUser"], config["SMTPPass"]);
                client.Authenticate("ulp.api.net@gmail.com", "ktitieuikmuzcuup");

                await client.SendAsync(message);
                return Ok(perfil);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET api/<controller>/5
        [HttpPost("reestablecer")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByEmail([FromForm] string email)
        {
            try
            {   //método sin autenticar, busca el propietario xemail
                var entidad = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == email);

                var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, entidad.Email),
                        new Claim("FullName", entidad.Nombre + " " + entidad.Apellido),
                        new Claim("id", entidad.Id + " " ),
                        new Claim(ClaimTypes.Role, "Propietario"),
					};

                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(600),
                    signingCredentials: credenciales
                );
                var to = new JwtSecurityTokenHandler().WriteToken(token);

                var direccion = "http://192.168.2.53:5000/API/Propietarios/token?access_token=" + to;
                try
                {


                    var message = new MimeKit.MimeMessage();
                    message.To.Add(new MailboxAddress(entidad.Nombre, "mvicalaniz@gmail.com"));
                    message.From.Add(new MailboxAddress(entidad.Nombre, "mvicalaniz@gmail.com"));
                    message.Subject = "Testing";
                    message.Body = new TextPart("html")


                    {
                        Text = @$"<h1>Hola</h1>
					<p>Bienvenido, {entidad.Nombre}! <a href={direccion} >Presione aquí para reestablecer su clave.</a> </p>",
                    };

                    message.Headers.Add("Encabezado", "Valor");
                    MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                    client.ServerCertificateValidationCallback = (object sender,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
                    { return true; };
                    client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                    //			client.Authenticate(config["SMTPUser"], config["SMTPPass"]);
                    client.Authenticate("ulp.api.net@gmail.com", "ktitieuikmuzcuup");

                    await client.SendAsync(message);
                    //	return Ok(perfil);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return entidad != null ? Ok(entidad) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

	}
	
}