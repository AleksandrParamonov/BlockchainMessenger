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
            Random rand = new Random();                                           
            Block bl = new Block();
            Blockchain bla = new Blockchain();
            bla.ValidateBlockchain();
            /*bl.toFile("testfile.txt");
            for(int i = 0; i < 100; i++)
                {
                bl = new Block(bl, (i + 1).ToString());
                bl.toFile("123");


                } */    

            Console.WriteLine("done");
            Console.ReadKey();
        }
        
    }
}
