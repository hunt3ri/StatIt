using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services.Models
{
    public class RevenueModel
    {
        public DateTime OldestDate { get; set; }
        public int MaxPointCount { get; set; }

        public Dictionary<string, List<int>> UnsortedRevenues { get; set; }
        public Dictionary<string, List<int>> StoreRevenues { get; set; }


        public RevenueModel()
        {
            OldestDate = DateTime.Now;
            UnsortedRevenues = new Dictionary<string, List<int>>();
            StoreRevenues = new Dictionary<string, List<int>>();

        }
    }
}
