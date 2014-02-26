using JsonFx.Json;
using StatIt.Engine.Flurry.Services.Models;
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

        public void GetActiveUsers()
        {
            var url = "http://api.flurry.com/appMetrics/ActiveUsers?apiAccessCode=" + FlurryAPIAccessCode + "&apiKey=" + WFSiOSCode + "&startDate=2013-12-09&endDate=2014-02-19";

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Accept = "application/json";

            // Execute request
            var rawDauData = WebRequestService.GetWebRequest(request);
            var iain = rawDauData.Replace("@", String.Empty);

            var reader = new JsonReader();
            dynamic dauData = reader.Read(iain);

            PopulateModel(dauData);

        }

        private void PopulateModel(dynamic dauData)
        {
            var dauModel = new List<DailyActiveUsersModel>();
            foreach (dynamic datapoint in dauData.day)
            {
                var model = new DailyActiveUsersModel();
                
                model.DAUDate = datapoint.date;
                model.iOSUsers = Convert.ToInt32(datapoint.value);

                dauModel.Add(model);

            }
        }
    }
}
