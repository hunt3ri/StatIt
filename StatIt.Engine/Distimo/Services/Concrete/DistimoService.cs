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
        private static string DistimoAPIAddress = "https://analytics.distimo.com/api/v4/";
        private static string QueryFormat = "format=json";
        private static string DownloadAPI = DistimoAPIAddress;

        private static string DistimoPrivateKey;
        private static string DistimoPublicKey;
        private static string DistimoUserName; 
        private static string DistimoPassword;

        private readonly IWebRequestService WebRequestService;

        public DistimoService(IWebRequestService webRequestService)
        {
            WebRequestService = webRequestService;

            DistimoPrivateKey = APIKeys.DistimoPrivateKey;
            DistimoPublicKey = APIKeys.DistimoPublicKey;
            DistimoUserName = APIKeys.DistimoUserName;
            DistimoPassword = APIKeys.DistimoPassword;
        }

        public string GetDownloads()
        {
            var downloadRequest = CreateDistimoRequest(DownloadAPI + "downloads", "breakdown=application,appstore&from=all&view=line");

            var downloadData = WebRequestService.GetWebRequest(downloadRequest);

            return downloadData;

        }

        public string GetRevenues(string queryString)
        {
            // from=all&revenue=total&view=line&breakdown=application,appstore
            //"from=all&revenue=total&view=line&breakdown=application,appstore,date&interval=week"
            var revenueRequest = CreateDistimoRequest(DownloadAPI + "revenues", "from=all&revenue=total&view=line&breakdown=application,appstore,date&interval=week");

            var revenueData = WebRequestService.GetWebRequest(revenueRequest);

            var reader = new JsonReader();
            dynamic output = reader.Read(revenueData);



            List<dynamic> filteredList = new List<dynamic>();

            foreach (dynamic line in output.lines)
            {
                string appName = line.data.application;

                if (appName.Contains("Winx Fairy School"))
                    filteredList.Add(line);
            }

            GetWeeklyRevenues(filteredList);
            
            return revenueData;
        }

        // TODO refactor this into Factory Class
        public List<RevenueChartModel> GetWeeklyRevenues(List<dynamic> revenueList)
        {
            var revenueModel = new RevenueModel();
           
            // Get bounds for data, max number of points and oldest data point
            foreach (dynamic item in revenueList)
            {
                var startDate = FromUnixTime(Convert.ToInt64(item.pointStart));
                var AppStore = item.data.appstore;

                var dataPoints = new List<int>();
                foreach (int? i in item.points)
                {
                    if (i.HasValue)
                        dataPoints.Add(i.Value);
                }

                revenueModel.RawRevenueData.Add(AppStore, dataPoints);

                // Calculate oldest date
                if (startDate < revenueModel.OldestDate)
                {
                    revenueModel.OldestDate = startDate; 
                    revenueModel.MaxPointCount = dataPoints.Count; // Must be highest if date is oldest
                }    
            }

            // Clean up data suitable for loading into Chart Model
            foreach (KeyValuePair<string, List<int>> unsortedList in revenueModel.RawRevenueData)
            {
                var cleanData = AddMissingPoints(revenueModel.MaxPointCount, unsortedList.Value);
                revenueModel.CleanRevenueData.Add(unsortedList.Key, cleanData);
            }

            // Populate model
            List<RevenueChartModel> chartModel = new List<RevenueChartModel>();
            var week = revenueModel.OldestDate;

            for (int i = 0; i <= revenueModel.MaxPointCount - 1; i++)
            {
                var model = new RevenueChartModel() { Week = week };

                foreach (KeyValuePair<string, List<int>> sortedList in revenueModel.CleanRevenueData)
                {
                    switch (sortedList.Key)
                    {
                        case "Amazon Appstore":
                            model.Amazon = sortedList.Value[i];
                            break;
                        case "Apple App Store":
                            model.Apple = sortedList.Value[i];
                            break;
                        case "Google Play Store":
                            model.Google = sortedList.Value[i];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unexpected appstore value");

                    }

                }

                chartModel.Add(model);
                week = week.AddDays(7);
            }

            var iain = chartModel;
            return chartModel;
        }

        public List<int> AddMissingPoints(int maxSize, List<int> dataPoints)
        {
            // Insert 0's at start of all arrays to account for missing points
            for (int i = dataPoints.Count; i < maxSize; i++)
            {
                dataPoints.Insert(0, 0);
            }

            return dataPoints;
        }


        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public string GetEvents()
        {
            var eventRequest = CreateDistimoRequest(DownloadAPI + "events", "from=all&types=price");

            var eventData = WebRequestService.GetWebRequest(eventRequest);

            return eventData;

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

        private HttpWebRequest CreateDistimoRequest(string apiAddress, string queryString)
        {
            // Format QueryString
            queryString = queryString + "&" + QueryFormat;
            var authToken = CreateAuthToken(queryString);

            string url = apiAddress + "?" + queryString + "&apikey=" + DistimoPublicKey + "&hash=" + authToken.AuthHash + "&t=" + authToken.Time + queryString;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Headers["Authorization"] = String.Concat("Basic ", authToken.Base64Login);

            return request;
        }


    }
}
