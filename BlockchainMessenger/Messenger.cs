using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainMessenger
{
    class Messenger
    {
        Blockchain blockchain;
        string userName;
        private Rebex.Security.Cryptography.Curve25519 EllipticCurve;
        private System.Security.Cryptography.Aes aes;
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
            blockchain = new Blockchain("/blockchain");
            byte[] privateKey = new byte[32];
            System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(privateKey);

            userName = arg;
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            EllipticCurve.FromPrivateKey(privateKey);
        }
        public Messenger(string arg, string privateKey)
        {
            blockchain = new Blockchain("/blockchain");
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
            blockchain = new Blockchain("/blockchain");
            userName = arg;
            if (privateKey.Length != 32)
                throw new System.ArgumentException("wrong key length, must be 32 symbols");
            EllipticCurve = Rebex.Security.Cryptography.Curve25519.Create(Rebex.Security.Cryptography.Curve25519.Curve25519Sha256);
            EllipticCurve.FromPrivateKey(privateKey);
        }

        public void SendMessageTo(string message, byte[] otherPublicKey)
        {
            if (message.Length > 0)
                blockchain.AddBlock(AESCrypto.EncryptToString(message, this.GetSharedSecretString(otherPublicKey)));
        }

        public void CheckMessagesFrom(byte[] otherPublicKey)
        {
            int blocksCount = blockchain.GetBlocksCount();
            for(int i = 1; i < blocksCount; i++)
            {
                string result = AESCrypto.DecryptToString(blockchain.GetBlockData(i), this.GetSharedSecretString(otherPublicKey));
                if (result.Length != 0)
                    Console.WriteLine("block " + i + ":" + result);
                
            }
        }

      

    }
}
