using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services.Models
{
    /// <summary>
    /// Helper class used to help parse raw revenue data from Distimo
    /// </summary>
    public class RevenueParser
    {
        public DateTime OldestDate { get; set; }
        public int MaxPointCount { get; set; }

        public Dictionary<string, List<int>> RawRevenueData { get; set; }
        public Dictionary<string, List<int>> CleanRevenueData { get; set; }


        public RevenueParser()
        {
            OldestDate = DateTime.Now;
            RawRevenueData = new Dictionary<string, List<int>>();
            CleanRevenueData = new Dictionary<string, List<int>>();

        }

        public void AddRawRevenueData(string Appstore, List<int> Datapoints, string Application)
        {

            if (RawRevenueData.ContainsKey(Appstore))
                MergeRawData(Appstore, Datapoints, Application);
            else
                RawRevenueData.Add(Appstore, Datapoints);
        }

        private void MergeRawData(string Appstore, List<int> Datapoints, string Application)
        {
            var currentData = RawRevenueData[Appstore];

            //if (currentData.Count() != Datapoints.Count())
            //    throw new ArgumentOutOfRangeException("Number of datapoints differ HELP!!");

            for (int i = 0; i < Datapoints.Count(); i++)
            {
                currentData[i] = currentData[i] + Datapoints[i];
            }

            // overwrite with new data
            RawRevenueData[Appstore] = currentData;

        }
    }
}
