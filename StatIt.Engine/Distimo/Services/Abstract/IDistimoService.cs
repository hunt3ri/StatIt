using StatIt.Engine.Distimo.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoService
    {

        string DistimoAPIAddress { get; }

        HttpWebRequest CreateDistimoRequest(string apiAddress, string queryString);
       
    }
}
