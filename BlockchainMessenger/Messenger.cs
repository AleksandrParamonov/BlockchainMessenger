using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainMessenger
{
    class Messenger
    {
        string userName;
        Rebex.Security.Cryptography.Curve25519 EllipticCurve;
        public Messenger(string arg)
        {
            userName = arg;
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
        }
    }
}
