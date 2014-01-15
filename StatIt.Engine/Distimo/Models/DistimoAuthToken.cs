using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Models
{

    public class DistimoAuthToken
    {
        public string AuthHash { get; set; }
        public string Base64Login { get; set; }
        public int Time { get; set; }

        public DistimoAuthToken (string authHash, string base64Login, int time)
        {
            AuthHash = authHash;
            Base64Login = base64Login;
            Time = time;
        }
    }
}
