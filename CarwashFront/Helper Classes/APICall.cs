using CarwashFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CarwashFront.Helper_Classes
{
    class APICall
    {
        public static string SendRequest(string requestType, string json, string api)
        {
            try
            {
                string TokenId = "1666723Dx";
                var url = $"https://localhost:44358/api/{api}";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = requestType;

                httpRequest.ServerCertificateValidationCallback = delegate { return true; };
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                Crypt crypt = new Crypt();
                var req = new ApiModel (crypt.Encrypter(json, "13334448853"), TokenId);
                var msg = JsonConvert.SerializeObject(req);


                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(msg);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                return streamReader.ReadToEnd();
            }
            catch
            {
                return "Error";
            }
        }
    }
}
