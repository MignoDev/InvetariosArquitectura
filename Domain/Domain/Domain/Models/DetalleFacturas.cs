using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class DetalleFacturas
{

    public int? IdDetalleFactura { get; set; }
    public int? IdFactura { get; set; }
    public int? IdProducto { get; set; }
    public string? NombreProducto { get; set; }
    public int? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public decimal? Subtotal { get; set; }

}
