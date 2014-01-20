using System;
namespace StatIt.Engine.Web.Services.Abstract
{
    public interface IWebRequestService
    {
        string GetWebRequest(System.Net.WebRequest request);
    }
}
