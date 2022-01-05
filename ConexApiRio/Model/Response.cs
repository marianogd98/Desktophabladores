using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Model
{
    public class Response<T>
    {
        public int success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
