using ConexApiRio.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Service
{
    public class ProductoService
    {
        static private ConexHttp ConexHttp { get; set; }
        const string endPoint = "product";
        const string uri = "http://172.54.0.14:8400/api/";
        static public Response<Producto> GetProduct(string cod)
        {
            ConexHttp = new ConexHttp();
            return JsonConvert.DeserializeObject<Response<Producto>>( ConexHttp.Get_Data( $"{uri}{endPoint}/{cod}").ToString() );
        }

    }
}
