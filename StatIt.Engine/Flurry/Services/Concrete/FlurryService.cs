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
        private readonly string WFSAndroidCode;

        public FlurryService(IWebRequestService webRequestService)
        {
            WebRequestService = webRequestService;

            FlurryAPIAccessCode = APIKeys.FlurryAPIAccessCode;
            WFSiOSCode = APIKeys.FlurryWFSiOSKey;
            WFSAndroidCode = APIKeys.FlurryWFSAndroidKey;
        }

        public DAUDisplayModel GetActiveUsers()
        {

            var iosDauData = CreateFlurryActiveUsersRequest(WFSiOSCode);

            var iosModelData = PopulateModel(iosDauData, new Dictionary<string,DailyActiveUsersModel>(), true);

            var androidDauData = CreateFlurryActiveUsersRequest(WFSAndroidCode);

            var modelData = PopulateModel(androidDauData, iosModelData, false);

            var displayModel = new DAUDisplayModel(modelData);

            return displayModel;
        }

        private dynamic CreateFlurryActiveUsersRequest(string APIKey)
        {
            var url = "http://api.flurry.com/appMetrics/ActiveUsers?apiAccessCode=" + FlurryAPIAccessCode + "&apiKey=" + APIKey + "&startDate=2013-12-09&endDate=2014-02-19";

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Accept = "application/json";

            // Execute request
            var rawDauData = WebRequestService.GetWebRequest(request);
            
            var cleanData = rawDauData.Replace("@", String.Empty); // Remove @ symbols from fieldnames

            var reader = new JsonReader();
            dynamic dauData = reader.Read(cleanData);

            return dauData;

        }

        private Dictionary<string, DailyActiveUsersModel> PopulateModel(dynamic dauData, Dictionary<string, DailyActiveUsersModel> dauDict, bool isIosData)
        {
            foreach (dynamic datapoint in dauData.day)
            {
                var model = new DailyActiveUsersModel();
                
                model.DAUDate = datapoint.date;

                // First run is iOS so add data, then insert Android values on second run
                if (isIosData)
                {
                    model.iOSUsers = Convert.ToInt32(datapoint.value);
                    dauDict.Add(model.DAUDate, model);
                }
                else
                {
                    dauDict[model.DAUDate].AndroidUsers = Convert.ToInt32(datapoint.value);
                }
                    
            }

            return dauDict;
        }
    }
}
