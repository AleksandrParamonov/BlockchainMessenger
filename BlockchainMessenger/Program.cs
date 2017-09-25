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
            Messenger test1 = new Messenger("test1");
            Messenger test2 = new Messenger("test2");

            Console.WriteLine("secret for user1 " + test1.GetSharedSecretString(test2.GetPublicKey()));
            Console.WriteLine("secret for user2 " + test2.GetSharedSecretString(test1.GetPublicKey()));

            Console.ReadKey();
        }
        
    }
}
