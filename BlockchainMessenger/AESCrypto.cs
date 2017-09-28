using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace BlockchainMessenger
{
    class AESCrypto
    {
        public static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        public static byte[] Encrypt(byte[] plain, string password)
        {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(plain, 0, plain.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public static byte[] Encrypt(string plain, string password)
        {
            //return Encrypt(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(plain))), password);
            return Encrypt(((Encoding.UTF8.GetBytes(plain))), password);
        }

        public static string EncryptToString(byte[] plain, string password)
        {
            return Convert.ToBase64String(Encrypt(plain, password));
        }

        public static string EncryptToString(string plain, string password)
        {
            //return Convert.ToBase64String(Encrypt(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(plain))), password));
            return Convert.ToBase64String(Encrypt(((Encoding.UTF8.GetBytes(plain))), password));

        }

        public static byte[] Decrypt(byte[] cipher, string password)
        {
            try
            {
                MemoryStream memoryStream;
                CryptoStream cryptoStream;
                Rijndael rijndael = Rijndael.Create();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(cipher, 0, cipher.Length);
                cryptoStream.Close();
                return memoryStream.ToArray();
            }
            catch   //invalid padding due to wrong key or other issue
            {
                return new byte[0];
            }
        }

        public static byte[] Decrypt(string cipher, string password)
        {
            return Decrypt(Convert.FromBase64String(cipher), password);
        }

        public static string DecryptToString(byte[] cipher, string password)
        {
            return Encoding.UTF8.GetString(Decrypt(cipher, password));
        }

        public static string DecryptToString(string cipher, string password)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(cipher), password));
        }

    }
}
