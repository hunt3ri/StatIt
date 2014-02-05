using StatIt.Engine.Distimo.Services.Models;
using System;
using System.Collections.Generic;
namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoService
    {
        string GetDownloads();

        RevenueModel GetRevenues(string queryString);

        string GetEvents();
    }
}
