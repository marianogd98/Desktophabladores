using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Model
{
    public class Producto
    {
        public int id { get; set; }
        public string codigoProducto { get; set; }
        public object codigoSubgrupo { get; set; }
        public int codigoTipo { get; set; }
        public string barra { get; set; }
        public object codigoBalanza { get; set; }
        public bool pesado { get; set; }
        public bool manejaSerial { get; set; }
        public bool regulado { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public double costo { get; set; }
        public double cantidad { get; set; }
        public double precioDetal { get; set; }
        public double precioMayor { get; set; }
        public double precioOferta { get; set; }
        public bool activoVenta { get; set; }
        public object cantidadAplicaDescuento { get; set; }
        public object fechaOfertaIni { get; set; }
        public double descuento { get; set; }
        public object fechaOfertaFin { get; set; }
        public int punto { get; set; }
        public string codigoMoneda { get; set; }
        public double precioDolar { get; set; }
        public double precioBolivar { get; set; }
        public double iva { get; set; }
        public object serial { get; set; }
        public object image { get; set; }
        public double total { get; set; }
    }
}
