using System;
namespace StatIt.Engine.Web.Services
{
    public interface IWebRequestService
    {
        string GetWebRequest(System.Net.WebRequest request);
    }
}
