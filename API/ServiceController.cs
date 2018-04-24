using Admin.BaseClass;
using Admin.BaseClass.App;
using Admin.Code.CustomCode;
using Admin.Code.Filters;
using Admin.CustomCode;
using Admin.Models;
using Admin.ModelsExtra.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [AllowAnonymous]
    [AllowCrossSite]
    public class ServiceController : BaseController
    {
        public static Rootobject RequestJson(string url, string json, string method = "POST")
        {

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = method;
                if (!string.IsNullOrEmpty(json))
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string read = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    read = streamReader.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<Rootobject>(read);
            }
            catch (Exception)
            {
                return null;
            }

        }
        #region POST
        private string EncodeHttpPostedFileBaseToBase64StringImage(HttpPostedFileBase httpPostedFileBase)
        {
            byte[] fileInBytes = new byte[httpPostedFileBase.ContentLength];
            using (BinaryReader reader = new BinaryReader(httpPostedFileBase.InputStream))
            {
                fileInBytes = reader.ReadBytes(httpPostedFileBase.ContentLength);
            }

            string base64String = $"data:image/jpeg;base64,{Convert.ToBase64String(fileInBytes)}";
            return base64String;
        }

        #endregion
    }
}
