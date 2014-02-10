using JsonFx.Json;
using StatIt.Engine.Distimo.Models;
using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services
{
    public class DistimoService : IDistimoService
    {
        private static string QueryFormat = "format=json";
        //private  string DownloadAPI;

        private static string DistimoPrivateKey;
        private static string DistimoPublicKey;
        private static string DistimoUserName; 
        private static string DistimoPassword;

        enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };

        private readonly IWebRequestService WebRequestService;

        public string DistimoAPIAddress
        {
            get { return "https://analytics.distimo.com/api/v4/"; }
        }

        public DistimoService(IWebRequestService webRequestService)
        {
            //DownloadAPI = DistimoAPIAddress;

            WebRequestService = webRequestService;

            DistimoPrivateKey = APIKeys.DistimoPrivateKey;
            DistimoPublicKey = APIKeys.DistimoPublicKey;
            DistimoUserName = APIKeys.DistimoUserName;
            DistimoPassword = APIKeys.DistimoPassword;
        }


        public HttpWebRequest CreateDistimoRequest(string apiAddress, string queryString)
        {
            // Format QueryString
            if (queryString != String.Empty)
                queryString = queryString + "&" + QueryFormat;
            else
                queryString = queryString + QueryFormat;

            var authToken = CreateAuthToken(queryString);

            string url = apiAddress + "?" + queryString + "&apikey=" + DistimoPublicKey + "&hash=" + authToken.AuthHash + "&t=" + authToken.Time + queryString;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Headers["Authorization"] = String.Concat("Basic ", authToken.Base64Login);

            return request;
        }
      

        /// <summary>
        /// Helper method to create the appropriate Authentication Hash that Distimo expects
        /// </summary>
        /// <returns>Distimo Authentication Hash</returns>
        private DistimoAuthToken CreateAuthToken(string queryString)
        {
           
            var time = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            string data = String.Concat(queryString, time);

            HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(DistimoPrivateKey));
            hmac.Initialize();
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            string hash = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();

            string user = String.Concat(DistimoUserName, ":", DistimoPassword);
            string base64Login = Convert.ToBase64String(Encoding.Default.GetBytes(user));

            return new DistimoAuthToken(hash, base64Login, time);
        }

       


    }
}
