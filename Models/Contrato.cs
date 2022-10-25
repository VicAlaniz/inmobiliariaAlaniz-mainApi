using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaAlaniz.Models;

public class Contrato {
    [Display(Name = "Código")]
    public int Id {get; set;}

    [Display(Name = "Inmueble")]
    public int IdInmueble { get; set; }
    [ForeignKey(nameof(IdInmueble))]
    public Inmueble Inmueble { get; set; }
 
    [Display(Name = "Inquilino")]
    public int IdInquilino { get; set; }
    [ForeignKey(nameof(IdInquilino))]
    public Inquilino Inquilino { get; set; }
       
   

    [Display(Name = "Fecha Inicio")]
    public DateTime FechaInicio {get; set;}

    [Display(Name = "Fecha Finalización")]
    public DateTime FechaFin {get; set;}
  
}