using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services
{
    public interface IDistimoAuthService
    {
        string DistimoPrivateKey { get; set; }
        string DistimoPublicKey { get; set; }
        string DistimoUserName { get; set; }
        string DistimoPassword { get; set; }

    }
}
