using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainMessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCurves();
        }
        static void TestCurves()
        {
            Rebex.Security.Cryptography.Curve25519 TestCurve1 = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            Rebex.Security.Cryptography.Curve25519 TestCurve2 = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);

            byte[] key1 = new byte[32], key2 = new byte[32], sharedkey1 = new byte[32], sharedkey2 = new byte[32];

            key1[0] = 123;
            key1[1] = 111;
            key1[2] = 121;
            key1[3] = 222;
            TestCurve1.FromPrivateKey(key1);

            key2[0] = 120;
            key2[1] = 11;
            key2[2] = 11;
            key2[3] = 2;
            TestCurve2.FromPrivateKey(key2);

            sharedkey1 = TestCurve1.GetSharedSecret(TestCurve2.GetPublicKey());
            sharedkey2 = TestCurve2.GetSharedSecret(TestCurve1.GetPublicKey());

            for (int i = 0; i < 32; i++)
                Console.WriteLine(sharedkey1[i] + " " + sharedkey2[i]);
            Console.ReadKey();
        }
    }
}
