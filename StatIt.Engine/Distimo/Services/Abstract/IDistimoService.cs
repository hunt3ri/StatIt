using StatIt.Engine.Distimo.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoService
    {

        string GetDistimoData(SupportedDistimoApis supportedApi, string queryString);
       
    }
}
