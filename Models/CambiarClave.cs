using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaAlaniz.Models;

public class CambiarClave
{

    [Required, DataType(DataType.Password)]
    public string PassVieja { get; set; }

    [Required, DataType(DataType.Password)]
    public string PassNueva { get; set; }

    [Required, DataType(DataType.Password)]
    public string PassConfirmada { get; set; }

}