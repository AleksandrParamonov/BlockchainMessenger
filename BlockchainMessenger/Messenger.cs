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
        private Rebex.Security.Cryptography.Curve25519 EllipticCurve;
        public string GetPrivateKeyString() //following functions will return formatted keys for current user
        {
            return BitConverter.ToString(EllipticCurve.GetPrivateKey()).Replace("-", string.Empty);
        }
        public string GetPublicKeyString()
        {
            return BitConverter.ToString(EllipticCurve.GetPublicKey()).Replace("-", string.Empty);
        }
        public byte[] GetPrivateKey()
        {
            return EllipticCurve.GetPrivateKey();
        }
        public byte[] GetPublicKey()
        {
            return EllipticCurve.GetPublicKey();
        }

        public string GetSharedSecretString(byte[] otherPublicKey)
        {
            return BitConverter.ToString(EllipticCurve.GetSharedSecret(otherPublicKey)).Replace("-", string.Empty);   
        }
        public byte[] GetSharedSecret(byte[] otherPublicKey)
        {
            return EllipticCurve.GetSharedSecret(otherPublicKey);
        }
        public Messenger(string arg)  
        {
            byte[] privateKey = new byte[32];
            System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(privateKey);

            userName = arg;
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            EllipticCurve.FromPrivateKey(privateKey);
        }
        public Messenger(string arg, string privateKey)
        {
            userName = arg;
            privateKey = privateKey.ToLower();
            if (privateKey.Length != 64)
                throw new System.ArgumentException("wrong hex key length, must be 64 symbols");
            for (int i = 0; i < privateKey.Length; i++)
                if (!((privateKey[i] >= '0' && privateKey[i] <= '9') || (privateKey[i] >= 'a' && privateKey[i] <= 'f')))
                    throw new System.ArgumentException("wrong hex key symbols");
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            EllipticCurve.FromPrivateKey(System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(privateKey).Value);
        }

        public Messenger(string arg, byte[] privateKey)
        {
            userName = arg;
            if (privateKey.Length != 32)
                throw new System.ArgumentException("wrong key length, must be 32 symbols");
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            EllipticCurve.FromPrivateKey(privateKey);
        }

    }
}
