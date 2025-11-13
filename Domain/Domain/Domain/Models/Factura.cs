using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class Factura
{

    public int? IdFactura { get; set; }
    public DateTime? FechaFactura { get; set; }
    public int? IdCliente { get; set; }
    public string? Cliente { get; set; }
    public int? IdEmpleado { get; set; }    
    public string? Empleado { get; set; }
    public decimal? Total { get; set; }
    public string? Estado { get; set; }


}
