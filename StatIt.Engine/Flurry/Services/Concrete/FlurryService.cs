using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Services
{
    public class FlurryService : IFlurryService
    {
        private readonly IWebRequestService WebRequestService;

        private readonly string FlurryAPIAccessCode;
        private readonly string WFSiOSCode;

        public FlurryService(IWebRequestService webRequestService)
        {
            WebRequestService = webRequestService;

            FlurryAPIAccessCode = APIKeys.FlurryAPIAccessCode;
            WFSiOSCode = APIKeys.FlurryWFSiOSKey;
        }

        public void GetActiveUsersByWeek()
        {
            var url = "http://api.flurry.com/appMetrics/ActiveUsersByWeek?apiAccessCode=" + FlurryAPIAccessCode + "&apiKey=" + WFSiOSCode + "&startDate=2013-12-09&endDate=2014-02-17";

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Accept = "application/json";

            // Execute request
            var requestData = WebRequestService.GetWebRequest(request);



        }
    }
}
