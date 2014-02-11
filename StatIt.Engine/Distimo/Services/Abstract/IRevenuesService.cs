using StatIt.Engine.Distimo.Services.Models;
using System;
namespace StatIt.Engine.Distimo.Services
{
    public interface IRevenuesService
    {
        void GetIAPRevenues();
        RevenueModel GetRevenues(string AppId, DateTime StartDate, DateTime EndDate);
    }
}
