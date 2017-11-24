using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlockchainMessenger
{
    class Program
    {

        public static string CalculateHash(string arg)
        {
            
            System.Security.Cryptography.SHA256 SHA256Calculator = System.Security.Cryptography.SHA256.Create();
            byte[] hash = new byte[32];
            hash = SHA256Calculator.ComputeHash(Encoding.UTF8.GetBytes(arg));
            return BitConverter.ToString(hash).Replace("-", string.Empty); //converts 32 bytes of hash to 64 HEX letters     
        }

        static void Main(string[] args)
        {
            byte[] key1 = new byte[32];
            key1[1] = 128;
            key1[2] = 255;

            byte[] key2 = new byte[32];
            key2[10] = 128;
            key2[20] = 255;
            Messenger test1 = new Messenger("test1", key1);
            Messenger test2 = new Messenger("test2", key2);


            //Console.WriteLine("secret for user1 " + test1.GetSharedSecretString(test2.GetPublicKey()));
            //Console.WriteLine("secret for user2 " + test2.GetSharedSecretString(test1.GetPublicKey()));

           //string a1 = AESCrypto.EncryptToString("123", test1.GetSharedSecretString(test2.GetPublicKey()));//asdf +U0XgypCmeRigcYKGtFITQ==
            //byte[] a2 = AESCrypto.Decrypt(a1, test2.GetPublicKeyString());
            //string a3 = AESCrypto.DecryptToString(a1, test1.GetSharedSecretString(test2.GetPublicKey()));
            //Console.WriteLine(a1);
            Messenger messenger = new Messenger("asd", test1.GetPrivateKey());
            string input = "";           
            messenger.CheckMessagesFrom(test2.GetPublicKey());
            while(true)
            {
                
                input = Console.ReadLine(); 
                if (input.Length >= 0)
                    messenger.SendMessageTo(input, test2.GetPublicKey());

            }
            
            Console.ReadKey();
        }
        
    }
}
