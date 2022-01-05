using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Model
{
    public class Habladores
    {
        public string Codigo { get; set; }
        public string Codigo_barra { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
        public string Precioo { get; set; }
        public int Oferta { get; set; }
        public string Departamento { get; set; }
        public int PorImprimir { get; set; }
        public string Fecha { get; set; }
        public string FechHorImpresion { get; set; }
        public string Moneda { get; set; }

        public Habladores(string Codigo, string Codigo_barra, string Descripcion, String Precio, String Departamento, int PorImprimir, string Fecha, string fechHorImpresion)
        {
            this.Codigo = Codigo;
            this.Codigo_barra = Codigo_barra;
            this.Descripcion = Descripcion;
            this.Precio = Precio;
            this.Departamento = Departamento;
            this.PorImprimir = PorImprimir;
            this.Fecha = Fecha;
            this.FechHorImpresion = fechHorImpresion;
        }

        public string GetCodigo()
        {
            return Codigo;
        }
        public string GetCodigo_barra()
        {
            return Codigo_barra;
        }
        public string GetDescripcion()
        {
            return Descripcion;
        }

        public string GetDepartamento()
        {
            return Departamento;
        }

        public int GetPorImprimir()
        {
            return PorImprimir;
        }

        public string GetFecha()
        {
            return Fecha;
        }

        public string GetFechaImpresion()
        {
            return FechHorImpresion;
        }

        public string GetPrecio()
        {
            return Precio;
        }

        public void SetFechaImpresion(string fechaImpresion)
        {
            FechHorImpresion = fechaImpresion;
        }
    }

        public class HabladoresViewModel
        {
            public string Codigo { get; set; }
            public string Codigo_barra { get; set; }
            public string Descripcion { get; set; }
            public string Precio { get; set; }
            public string Precioo { get; set; }
            public int Oferta { get; set; }
            public string Departamento { get; set; }
            public string PorImprimir { get; set; }
            public string Fecha { get; set; }
            public string FechHorImpresion { get; set; }
        }
    }
