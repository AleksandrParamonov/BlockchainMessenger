using System;
using System.Text;

namespace BlockchainMessenger
{

    public class Block
        {
        int index;
        public string data;
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

        public Block(string fileName)
        {

            string buffer;
            string[] arguments = new string[12];
            buffer = System.IO.File.ReadAllText(fileName);
            arguments = buffer.Split(new string[] { ":", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            index = int.Parse(arguments[1]);
            data = arguments[3];
            timestamp = arguments[5];
            previousHash = arguments[7];
            nonce = arguments[9];
            hash = arguments[11];
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
        int blocksCount = 0;
		private string path;
        Block[] Blocks = new Block[100];
        public void GetAndValidateBlockchain()
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
                {
                    Console.WriteLine(i + " is wrong\n" + arguments[11] + " expected\n" + Hash + " received");
                    throw new Exception("blockchain is damaged or modified!");
                }
                Blocks[i] = new Block(i + ".txt");
                blocksCount++;
                i++;
                } 
             if (i == 0)
            {
                Blocks[0] = new Block();
                Blocks[0].toFile("");
                blocksCount = 1;
            }
        }

        public void AddBlock(string data)
        {
            Blocks[blocksCount] = new Block(Blocks[blocksCount - 1], data);
            Blocks[blocksCount].toFile("");
            blocksCount++;
        }

		public Blockchain()
		{
            GetAndValidateBlockchain();
            //Blocks[0] = new Block();
            //Blocks[0].toFile("");

        }

		public Blockchain(string newpath)
        {
			path = newpath;
            GetAndValidateBlockchain();
            //Blocks[0] = new Block();
            //Blocks[0].toFile("");
        }
        public int GetBlocksCount()
        {
            return blocksCount;
        }

        public string GetBlockData(int blockNumber)
        {
            return Blocks[blockNumber].data;
        }

	}
}