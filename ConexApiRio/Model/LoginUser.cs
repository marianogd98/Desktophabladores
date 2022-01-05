using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Model
{
    public class LoginUser
    {
        public string token { get; set; }
        public string CodigoUser { get; set; }
        public string NombreUser { get; set; }
        public string Result { get; set; }

        public string getCodigoUser()
        {
            return CodigoUser;
        }

        public string getNombreUser()
        {
            return NombreUser;
        }

        public string getToken()
        {
            return token;
        }
    }
}
