using StatIt.Engine.Flurry.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Services
{
    public interface IFlurryService
    {
        DAUDisplayModel GetActiveUsers();
    }
}
