using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaAlaniz.Models;

public class Inmueble {
    public int Id {get; set;}
    public string Direccion {get; set;}
    public string Uso {get; set;}
    public string Tipo {get; set;}
    public int CantAmbientes {get; set;}
    public decimal? CoordenadaN { get; set; }
	public decimal? CoordenadaE { get; set; }
    public Boolean Estado {get; set;}
    public decimal Precio {get; set;}
    public int PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }
    public String? Imagen { get; set; }
  
}