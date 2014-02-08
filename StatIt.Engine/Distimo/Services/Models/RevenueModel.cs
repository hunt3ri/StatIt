using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services.Models
{
    public class RevenueModel
    {
        public List<RevenueByWeek> RevenueByWeek { get; set; }
        public string StartDate { get; set; }
        public int GrossRevenue { get; set; }

        public RevenueModel(List<RevenueByWeek> revenueByWeek, DateTime startDate)
        {
            var amazonTotal = revenueByWeek.Sum(rev => rev.Amazon);
            var appleTotal = revenueByWeek.Sum(rev => rev.Apple);
            var googleTotal = revenueByWeek.Sum(rev => rev.Google);

            RevenueByWeek = revenueByWeek;
            StartDate = startDate.ToString("dd/MM/yyyy");
            GrossRevenue = (amazonTotal + appleTotal + googleTotal);
        }
    }
}
