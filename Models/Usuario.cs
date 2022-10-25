using System.ComponentModel.DataAnnotations;

namespace InmobiliariaAlaniz.Models;

public enum Roles{
    Administrador = 1,
    Empleado = 2
}

	public class Usuario{
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
	
		[Required, EmailAddress]
		public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Clave {get; set;}
        public string? Avatar {get; set;}
        //[NotMapped]
        public IFormFile? AvatarFile { get; set; }
        public int Rol {get; set;}
        public string RolesName => Rol>0 ? ((Roles)Rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles(){
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoRol = typeof(Roles);
            foreach (var value in Enum.GetValues(tipoRol)){
                roles.Add((int)value, Enum.GetName(tipoRol, value));
            }
            return roles;
        }
    }