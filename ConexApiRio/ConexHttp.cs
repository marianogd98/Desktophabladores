using System;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace ConexApiRio
{
    public class ConexHttp
    {
        string Token { get; set; }

        public ConexHttp(string token = "")
        {
            Token = token;
        }

        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        public string /*PhysicalAddress*/  GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }

        public Boolean verificarconexion(string url)
        {

            Uri objUrl = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(objUrl);
            //request.Headers.Add("Authorization", "Bearer " + Token);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                response.Close();
                response = null;
                return true;
            }
            catch
            {
                //' Error, exit and return False 
                response = null;
                return false;
            }
        }
        //------------------------------------------
        /*POST*/
        public string EnviarDatosJson(string data)
        {
            string result = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(DefUrls.GetUrlApi("ticket"));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = null;
                try
                {
                    obj = JsonConvert.DeserializeObject(resp);
                }
                catch { return (obj == null) ? "" : "CuotaResponse"; }
                return obj.Estatus;
            }
            return result;
        }

        /*POST*/
        public string Consulta_datos(string url, string parametros = "")
        {
            string ResulRequest = "";//recibe respuesta del server
            Uri uri = new Uri(url);
            try
            {
                string data = parametros;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Post;
                request.CookieContainer = new CookieContainer();
                request.KeepAlive = true;
                request.ContentLength = data.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Bearer " + Token);
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(data);
                writer.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                ResulRequest = reader.ReadToEnd();
                response.Close();
                //ichTextBox1.AppendText(tmp); // log - delete this line 
            }
            catch (WebException e) /*(WebException e)*/
            {
                using (WebResponse response = e.Response)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        ResulRequest = new StreamReader(data).ReadToEnd();

                    }
                }
                //string m = e.HResult.ToString();
                // = "{\"estatus\":-1}";
            }

            return ResulRequest;
        }

        //Envia data al servidor HTTP--- GET
        public string subir_data(string url, string data = "")
        {
            string datos = "";
            try
            {
                url = url + "?" + data;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Authorization", "Bearer " + Token);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                // Read the content.
                datos = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                datos = e.Message;
            }
            return datos;
        }

        //Recibir data desde servidor HTTP--- GET
        public string Get_Data(string url)
        {
            string datos = "";
            try
            {
                //url = url + "?" + data;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Headers.Add("Authorization", "Bearer " + Token);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                // Read the content.
                datos = reader.ReadToEnd();
            }
            catch (WebException e)
            {

                using (WebResponse response = e.Response)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        datos = new StreamReader(data).ReadToEnd();

                    }
                }
            }
            return datos;
        }



        //Obtener Token Usuario
        public string GetToken(string username, string password, string Localidad)
        {
            string url = DefUrls.GetUrlApi("Login");
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {               

                var input = "{\"Username\":\"" + username + "\"," +
                            "\"Localidad\":\"" + Localidad + "\"," +
                            "\"Password\":\"" + password + "\"}";


                streamWriter.Write(input);
                streamWriter.Flush();
                streamWriter.Close();
            }
            string response = "";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();


                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {                
                var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();

                if(resp == null)
                {

                }
                else
                {
                    return (resp.Length > 50) ? "off" : (e.Message.Contains("401")? "Unauthorized": "CuotaResponse");
                }
                dynamic obj = null;
                /*try
                {
                    obj = JsonConvert.DeserializeObject(resp);
                }
                catch { return (obj == null) ? "off" : "CuotaResponse"; }*/
                return obj==null? "Unauthorized" : obj.error;

                /* using (WebResponse responseError = e.Response)
                 {
                     HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                     return httpResponse.StatusCode.ToString();
                 }*/
            }

            //return GetDataResponse(response); //Retiro este para procesar el response como cadena luego de recibirlo
            return response;
        }

        string GetDataResponse(string response, string cutjson = "access_token")
        {
            // Crude way
            var entries = response.TrimStart('{').TrimEnd('}').Replace("\"", String.Empty).Split(',');

            foreach (var entry in entries)
            {
                if (entry.Split(':')[0] == cutjson)
                {
                    return entry.Split(':')[1];
                }
                else if (entry.Split(':')[0] == "error")
                {
                    return entry.Split(':')[1];
                }
            }

            return null;
        }

    }
}
