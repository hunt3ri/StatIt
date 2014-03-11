﻿using JsonFx.Json;
using StatIt.Engine.Flurry.Services.Models;
using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

        public DAUDisplayModel GetNewUsers(DateTime DateStart, DateTime DateEnd)
        {
            Thread.Sleep(1000);
            var iosNewUsers = CreateFlurryActiveUsersRequest("NewUsers", WFSiOSCode, DateStart, DateEnd);
            var iosModelData = PopulateModel(iosNewUsers, new Dictionary<string,DailyActiveUsersModel>(), true);
            Thread.Sleep(1100);

            // Get DAU data for Android devices
            var androidNewUsers = CreateFlurryActiveUsersRequest("NewUsers", WFSAndroidCode, DateStart, DateEnd);
            var modelData = PopulateModel(androidNewUsers, iosModelData, false);

            var displayModel = new DAUDisplayModel(modelData);
            return displayModel;
        }

        public void GetMAU(DateTime DateStart, DateTime DateEnd)
        {
            // TODO - Initial work on MAU, it would need to work seperately from others.
            var iosDauData = CreateFlurryActiveUsersRequest("ActiveUsersByMonth", WFSiOSCode, DateStart, DateEnd);

        }

        public DAUDisplayModel GetActiveUsers(DateTime DateStart, DateTime DateEnd)
        {
            
            // Get DAU data for iOS devices
            var iosDauData = CreateFlurryActiveUsersRequest("ActiveUsers", WFSiOSCode, DateStart, DateEnd);
            var iosModelData = PopulateModel(iosDauData, new Dictionary<string,DailyActiveUsersModel>(), true);

            Thread.Sleep(1100);
            // Get DAU data for Android devices
            var androidDauData = CreateFlurryActiveUsersRequest("ActiveUsers", WFSAndroidCode, DateStart, DateEnd);
            var modelData = PopulateModel(androidDauData, iosModelData, false);

            var displayModel = new DAUDisplayModel(modelData);
            return displayModel;
        }

        private dynamic CreateFlurryActiveUsersRequest(string MetricName, string APIKey, DateTime DateStart, DateTime DateEnd)
        {
            var url = "http://api.flurry.com/appMetrics/" + MetricName + "?apiAccessCode=" + FlurryAPIAccessCode + "&apiKey=" + APIKey + "&startDate=" + DateStart.ToString("yyyy-MM-dd") + "&endDate=" + DateEnd.ToString("yyyy-MM-dd");

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
