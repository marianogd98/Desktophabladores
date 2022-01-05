using ConexApiRio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConexApiRio.Service
{
    public class JsonPlaceHolderApi
    {
        private ConexHttp ConexHttp { get; set; }
        private string Dpto;
        public JsonPlaceHolderApi()
        {
            ConexHttp = new ConexHttp();
            Dpto = "00";
        }

        public JsonPlaceHolderApi(string Dpto)
        {
            this.Dpto = Dpto;
            ConexHttp = new ConexHttp();
        }

        //GET habladores
        public List<Habladores> GetHabladores(string Moneda="0")
        {           
            try
            {
                return JsonConvert.DeserializeObject<List<Habladores>>(ConexHttp.Get_Data(DefUrls.GetUrlApi((Dpto.Equals("00"))?"habladores/moneda/"+Moneda:"habladores/farmacia")));
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        //@GET("hablador/{codigo}")
        public List<Habladores> GetHablador(string codigo)
        {
            try
            {              
                return JsonConvert.DeserializeObject<List<Habladores>>(ConexHttp.Get_Data(DefUrls.GetUrlApi("hablador/"+((Dpto.Equals("05"))? "farmacia/" : "")+codigo)));
            }
            catch
            {
                return null;
            }
        }

        //@GET("hablador/producto/{Descripcion}")
        public List<Habladores> GetHabladoresDescripcion(string Descripcion)
        {
            try
            {
                var mio = JsonConvert.DeserializeObject<List<Habladores>>(ConexHttp.Get_Data(DefUrls.GetUrlApi("hablador/producto/"+ ((Dpto.Equals("05") && Dpto!=null) ? "farmacia/" : "") + Descripcion)));
                return mio;
            }
            catch
            {
                return null;
            }
        }

        //@GET("habladores/pendientes")
        public string GetHayHabladores()
        {
            return "";
        }

        //@GET("habladores/actualizar/producto/{Codigo}/user/{IdUser}/device/{IdDevice}")
        public List<Habladores> SetActualizarHablador(string Codigo, string IdUser, string IdDevice)
        {
            try
            {               
                return JsonConvert.DeserializeObject<List<Habladores>>(ConexHttp.Get_Data(DefUrls.GetUrlApi("habladores/actualizar/producto/" + Codigo + "/user/" + IdUser + "/device/" + IdDevice+"/dpto/"+Dpto)));
            }
            catch
            {
                return null;
            }
        }

        //@GET("habladores/actualizar/sin/producto/{Codigo}/user/{IdUser}/device/{IdDevice}")
        public string SetActualizarHabladorSinImpresion(string Codigo, string IdUser, string IdDevice)
        {
            try
            {
                return ConexHttp.Get_Data(DefUrls.GetUrlApi("habladores/actualizar/sin/producto/" + Codigo + "/user/" + IdUser + "/device/" + IdDevice));
            }
            catch
            {
                return null;
            }
        }

        //@GET("habladores/departamento/{Departamento}")
        public List<Habladores> GetHabladoresDepartamentos(string Departamento)
        {        
            return null;
        }

        //@GET("habladores/impresion/datetime")
        public string GetHabladoresDatetime()
        {
            return "";
        }

        //@GET("existencia/producto/{CodigoBarra}")
        public Producto GetExistenciaProducto(string CodigoBarra)
        {
            try
            {
                var mio = JsonConvert.DeserializeObject<Producto>(ConexHttp.Get_Data(DefUrls.GetUrlApi("existencia/producto/" + CodigoBarra)));
                return mio;
            }
            catch
            {
                return null;
            }
        }

        //@GET("departamento")
        public List<Departamento> getDepartamentos()
        {
            return null;
        }

        //@FormUrlEncoded
        //@POST("Login")
        public LoginUser Login(string Username,string Password,string Localidad)
        {
            LoginUser result= new LoginUser();
            try
            {
                result.Result = ConexHttp.GetToken(Username, Password, Localidad);
                result= JsonConvert.DeserializeObject<LoginUser>(result.Result);
                result.Result = ConexHttp.GetMacAddress();
                return result;
            }
            catch //(JsonException e)
            {
                return result;
            }           
        }

        //@GET("configuracion")
        public Impresion GetImpresion()
        {
            try
            {
                var mio = JsonConvert.DeserializeObject<Impresion>(ConexHttp.Get_Data(DefUrls.GetUrlApi("habladores/rtf")));
                return mio;
            }
            catch
            {
                return null;
            }
        }
    }
}
