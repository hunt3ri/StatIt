using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Web.Services
{
    public class WebRequestService : IWebRequestService
    {
        public string GetWebRequest(WebRequest request)
        {
            try
            {
                using (HttpWebResponse res = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream s = res.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(s);
                        return reader.ReadToEnd();
                    }
                };
            }
            catch (WebException e)
            {
                return new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
            }
        }
    }
}
