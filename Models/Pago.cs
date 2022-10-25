using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaAlaniz.Models;

public class Pago {
    [Display(Name = "Código")]
    public int Id {get; set;}
    [Required]
     [Display(Name = "Fecha Pago")]
    public DateTime FechaPago {get; set;}
    [Required]
    [Display(Name = "Importe")]
    public decimal Importe { get; set; }
    [Required]
     [Display(Name = "Contrato")]
    public int IdContrato { get; set; }
    [ForeignKey(nameof(IdContrato))]
    public Contrato Contrato { get; set; }
 
}