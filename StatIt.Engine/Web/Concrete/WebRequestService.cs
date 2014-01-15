using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Web.Concrete
{
    public class WebRequestService
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
