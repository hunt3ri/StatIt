using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Services.Models
{
    public class FlurryUsersDisplayModel
    {
        public List<FlurryUsersModel> FlurryUserData { get; set; }

        public FlurryUsersDisplayModel(Dictionary<string, FlurryUsersModel> dauData)
        {
            FlurryUserData = new List<FlurryUsersModel>();

            foreach (FlurryUsersModel dauModel in dauData.Values)
            {
                FlurryUserData.Add(dauModel);
            }
        }
    }
}
