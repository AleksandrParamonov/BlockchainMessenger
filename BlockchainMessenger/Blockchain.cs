using System;
using System.Text;

namespace BlockchainMessenger
{

    public class Block
        {
        int index;
        string data;
        string timestamp;
        string nonce;
        string previousHash;
        string hash;

        void FindGoodHash(int difficulty)
            {  
            Random rand = new Random();
            byte[] noncearray = new byte[8];
            bool goodHash = false;
            while(!goodHash)
                {    
                rand.NextBytes(noncearray);
                nonce = BitConverter.ToString(noncearray).Replace("-", string.Empty);
                hash = Program.CalculateHash(index + data + timestamp + previousHash + nonce);
                goodHash = true;
                for (int i = 0; i < difficulty; i++)
                    if(hash[i] != '0')
                        goodHash = false;
                }
            }

        public Block()
            {
            index = 0;
            timestamp = DateTime.UtcNow.ToString("dd/MM/yyyy,hh-mm-ss");
            previousHash = "null";
            data = "first block";
            FindGoodHash(3);    
            }

        public Block(Block prevBlock, string data)
            {
            index = prevBlock.index + 1;
            timestamp = DateTime.UtcNow.ToString("dd/MM/yyyy,hh-mm-ss");
            previousHash = prevBlock.hash;
            this.data = data;
            FindGoodHash(3);                           
            }  
        
        public void toFile(string fileName)
            {           
            System.IO.File.WriteAllText(index + ".txt", "index:" + index 
                + Environment.NewLine + "data:" + data 
                + Environment.NewLine + "timestamp:" + timestamp
                + Environment.NewLine + "previoushash:" + previousHash 
                + Environment.NewLine + "nonce:" + nonce 
                + Environment.NewLine + "hash:" + hash);
            }
        }


	public class Blockchain
	{
		private string path;

        public void ValidateBlockchain()
            {
            string buffer;
            string Hash = "null";
            string[] arguments = new string[12];
            int i = 0;
             while(System.IO.File.Exists(i + ".txt"))
                {
                buffer = System.IO.File.ReadAllText(i + ".txt");
                arguments = buffer.Split(new string[] {":", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                Hash = Program.CalculateHash(arguments[1] + arguments[3] + arguments[5] + Hash + arguments[9]) ;
                if (arguments[11] != Hash)
                    Console.WriteLine(i + " is wrong\n" + arguments[11] + "\n" + Hash);
                else 
                    Console.WriteLine(i + " is good");
                i++;
                } 
            }

		public Blockchain()
		{

		}

		public Blockchain(string newpath)
        {
			path = newpath;
			newpath += "123";
			Console.WriteLine(path + " " + newpath);

	    }

	}
}