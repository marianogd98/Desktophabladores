using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexApiRio.Model
{
    public class DataConfig
    {

        public string IpServer { get; set; }
        public string Impresora { get; set; }
        public string Suc { get; set; }
        public string Dpto { get; set; }
        public string FilePrint { get; set; }

        public DataConfig GetDataConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;
            string server = appSettings["Server"] ?? "Not Found";
            string impresora = appSettings["Impresora"] ?? "Not Found";
            string filePrint = appSettings["FilePrint"] ?? "Not Found";
            string departamento = appSettings["Departamento"] ?? "Not Found";
            string suc = appSettings["Suc"] ?? "Not Found";

            if (server.Equals("Not Found") || impresora.Equals("Not Found") || filePrint.Equals("Not Found") || departamento.Equals("Not Found") || suc.Equals("Not Found"))
            {
                return null;
            }
            else
            {
                return new DataConfig
                {
                    IpServer = server,
                    Impresora = impresora,
                    FilePrint = filePrint,
                    Dpto = departamento,
                    Suc = suc
                };
            }
        }
    }
}
