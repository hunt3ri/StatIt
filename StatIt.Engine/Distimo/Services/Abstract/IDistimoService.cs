using System;
namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoService
    {
        string GetDownloads();

        string GetRevenues();

        string GetEvents();
    }
}
