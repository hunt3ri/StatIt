using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Services.Models
{
    public class DAUDisplayModel
    {
        public List<DailyActiveUsersModel> DailyActiveUsers { get; set; }

        public DAUDisplayModel(Dictionary<string, DailyActiveUsersModel> dauData)
        {
            DailyActiveUsers = new List<DailyActiveUsersModel>();

            foreach (DailyActiveUsersModel dauModel in dauData.Values)
            {
                DailyActiveUsers.Add(dauModel);
            }
        }
    }
}
