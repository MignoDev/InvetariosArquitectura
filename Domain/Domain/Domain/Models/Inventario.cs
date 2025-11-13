using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Inventario
    {

        int? StockID { get; set; }
        string? ProductoNombre { get; set; }
        int? Cantidad { get; set; }
        int? Ubicacion { get; set; }
        DateTime? FechaActualizacion { get; set; }

    }
}
