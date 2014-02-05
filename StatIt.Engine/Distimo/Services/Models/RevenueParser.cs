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
    }
}
