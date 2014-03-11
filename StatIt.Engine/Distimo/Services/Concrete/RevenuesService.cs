using JsonFx.Json;
using Microsoft.CSharp.RuntimeBinder;
using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using StatIt.Engine.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services
{
    public class RevenuesService : IRevenuesService
    {
        private readonly IDistimoService DistimoService;

        public RevenuesService(IDistimoService distimoService)
        {
            DistimoService = distimoService;
        }

        public RevenueModel GetRevenues(string AppId, DateTime StartDate, DateTime EndDate)
        {
            // Round to nearest Monday so graph looks sane
            // TODO - This increasingly looks like something we should do in JS in the frontend unfortunately :-(
            StartDate = GetNearestMonday(StartDate);

            var revsQueryString = BuildRevenuesQueryString(StartDate, EndDate);

            //TODO - Pass thru appname from frontend
            var filteredList = ExtractAppRevenueData("Winx Fairy School", revsQueryString);
            var revenueModel = new RevenueModel(GetWeeklyRevenues(filteredList), StartDate);

            return revenueModel;
        }

        public string BuildRevenuesQueryString(DateTime StartDate, DateTime EndDate)
        {
            var queryString = "from=" + StartDate.ToString("yyyy-MM-dd") + "&to=" + EndDate.ToString("yyyy-MM-dd") + "&revenue=total&view=line&breakdown=application,appstore,date&interval=week";
            return queryString;
        }

        public List<dynamic> ExtractAppRevenueData(string appName, string queryString)
        {
            var revenueData = DistimoService.GetDistimoData(SupportedDistimoApis.Revenues, queryString);

            var reader = new JsonReader();
            dynamic output = reader.Read(revenueData);

            List<dynamic> filteredList = new List<dynamic>();

            try
            {
                foreach (dynamic line in output.lines)
                {
                    string localAppName = line.data.application;

                    if (localAppName.Contains(appName))
                        filteredList.Add(line);
                }
            }
            catch (RuntimeBinderException)
            {
                // too many requests, try again
                Thread.Sleep(1100);
                return ExtractAppRevenueData(appName, queryString);
            }

            return filteredList;
        }

        public RevenueModel GetIAPRevenues(string AppId, DateTime StartDate, DateTime EndDate)
        {
            // Round to nearest Monday so graph looks sane
            StartDate = GetNearestMonday(StartDate);

            // Sleep on thread so not hitting API with simulatenous requests

            //Thread.Sleep(1000);

            var appIds = GetApplicationIds();

            var iapIds = GetIAPIds(appIds);

            var iapData = DistimoService.GetDistimoData(SupportedDistimoApis.Revenues, "from=" + StartDate.ToString("yyyy-MM-dd") + "&to=" + EndDate.ToString("yyyy-MM-dd") + "&revenue=total&metrics=in_app&view=line&breakdown=application,appstore,date&interval=week");

            var reader = new JsonReader();
            dynamic rawIAPData = reader.Read(iapData);

            List<dynamic> filteredList = new List<dynamic>();

            foreach (dynamic line in rawIAPData.lines)
            {
                string app_id = line.data.application_id;

                if (iapIds.Contains(app_id))
                    filteredList.Add(line);
            }

            var revenueModel = new RevenueModel(GetWeeklyRevenues(filteredList), StartDate);
            return revenueModel;
        }



        /// <summary>
        /// Get a list of all IAP ids that belong to one of the parent applications
        /// </summary>
        /// <param name="parentIds">List of Distimo appIds</param>
        /// <returns>List of IAP Distimo ids</returns>
        private List<string> GetIAPIds(List<string> parentIds)
        {
            var assetData = DistimoService.GetDistimoData(SupportedDistimoApis.FilterAssetRevenues, string.Empty);

            var reader = new JsonReader();
            dynamic rawAssetData = reader.Read(assetData);

            var iapList = new List<string>();

            foreach (dynamic row in rawAssetData)
            {
                if (parentIds.Contains(row.Value.parent_id))
                    iapList.Add(row.Key);

            }

            return iapList;
        }

        /// <summary>
        /// This class extracts the Distimo applicationIds from the raw data
        /// </summary>
        private List<string> GetApplicationIds()
        {
            var appStoreData = DistimoService.GetDistimoData(SupportedDistimoApis.Revenues, "from=all&revenue=total&view=line&breakdown=application,appstore");

            var reader = new JsonReader();
            dynamic apps = reader.Read(appStoreData);

            var applicationIds = new List<string>();

            foreach (dynamic line in apps.lines)
            {
                 string appName = line.data.application;

                 if (appName.Contains("Winx Fairy School"))
                     applicationIds.Add(line.data.application_id);
            }

            return applicationIds;  
        }



        public List<RevenueByWeek> GetWeeklyRevenues(List<dynamic> revenueList)
        {
            var revenueModel = new RevenueParser();

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
                    else
                        dataPoints.Add(0); // add zeros instead of nulls to ensure no insanity with different number of datapoints later
                }

                revenueModel.AddRawRevenueData(AppStore, dataPoints, item.data.application);

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
            List<RevenueByWeek> chartModel = new List<RevenueByWeek>();
            var week = revenueModel.OldestDate;

            for (int i = 0; i <= revenueModel.MaxPointCount - 1; i++)
            {
                var model = new RevenueByWeek() { Week = week.ToShortDateString() };

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




        private List<int> AddMissingPoints(int maxSize, List<int> dataPoints)
        {
            // Insert 0's at start of all arrays to account for missing points
            for (int i = dataPoints.Count; i < maxSize; i++)
            {
                dataPoints.Insert(0, 0);
            }

            return dataPoints;
        }


        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }



        /// <summary>
        /// Helper function returns nearest Monday which is the Day Distimo uses as the start
        /// of the week.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <returns></returns>
        private DateTime GetNearestMonday(DateTime StartDate)
        {
            DayOfWeek monday = DayOfWeek.Monday;
            return StartDate.AddDays(-(StartDate.DayOfWeek - monday));

        }
    }
}
