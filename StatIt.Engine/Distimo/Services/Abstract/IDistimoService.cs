using System;
namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoService
    {
        string GetDownloads();

        string GetRevenues(string queryString);

        string GetEvents();
    }
}
