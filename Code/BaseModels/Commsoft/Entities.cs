using Admin.BaseModels.ViewModels;
using Admin.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Admin.BaseModels.Commsoft
{
    public class Commsoft
    {
        private static HttpClient httpClient = new HttpClient();

       
        public static string RequestJson(string url, string json, string method = "POST")
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 3600000; // 60 minutos

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string read = "";

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    read = streamReader.ReadToEnd();
                }

                return read;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

    public class TokenRequest
    {
        public string interfaceName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string port { get; set; }
    }

    public class TokenResponse
    {
        public string loginToken { get; set; }
    }
}