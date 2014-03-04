using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services
{
    /// <summary>
    /// Helper class that provides API Auth keys.
    /// </summary>
    public class DistimoAuthService : IDistimoAuthService
    {
        public string DistimoPrivateKey { get; set; }
        public string DistimoPublicKey { get; set; }
        public string DistimoUserName { get; set; }
        public string DistimoPassword { get; set; }

        public DistimoAuthService()
        {
            DistimoPrivateKey = APIKeys.DistimoPrivateKey;
            DistimoPublicKey = APIKeys.DistimoPublicKey;
            DistimoUserName = APIKeys.DistimoUserName;
            DistimoPassword = APIKeys.DistimoPassword;
        }
    }
}
