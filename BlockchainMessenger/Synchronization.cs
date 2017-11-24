using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockchainMessenger
{
    class Synchronization
    {
        static Object SyncingMutex = new Object();
        static string[] IPForSync = { "127.0.0.1", "192.168.1.4", "192.168.1.8" };
        public static void TryToSyncWith(string address, int port)
        {
            lock (SyncingMutex)
            {
                try
                {
                    TcpClient client = new TcpClient();
                    try
                    {
                        IAsyncResult result = client.BeginConnect(address, port, null, null);
                        bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(0.3), true);
                        //client = new TcpClient("127.0.0.1", 12000); 
                        if (!client.Connected)
                            throw new Exception("Failed to connect.");

                        client.EndConnect(result);

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex);
                        client.Close();
                        client = null;
                        return;
                    }
                    client.ReceiveTimeout = 1000;
                    client.SendTimeout = 1000;

                    int blockchainSize = System.IO.Directory.GetFiles("./blockchain").Length;

                    //byte[] buf = System.IO.File.ReadAllBytes("Program.cs");
                    byte[] buf2 = new byte[10000];
                    //Console.WriteLine(Encoding.ASCII.GetChars(buf));
                    //stream.Write(buf, 0, buf.Length);               //способ через Stream
                    //Thread.Sleep(1000);
                    int len = client.Client.Receive(buf2);
                    Array.Resize<byte>(ref buf2, len);

                    int serverBlockchainSize = int.Parse(Encoding.UTF8.GetString(buf2));
                    client.Client.Send(Encoding.UTF8.GetBytes(blockchainSize.ToString()));
                    Console.WriteLine(((IPEndPoint)client.Client.LocalEndPoint).Port);
                    if (serverBlockchainSize <= blockchainSize)
                    {
                        goto disconnectclient;
                    }

                    for (int i = 1; i <= serverBlockchainSize; i++)
                    {
                        Array.Resize<byte>(ref buf2, 10000);
                        len = client.Client.Receive(buf2);
                        Array.Resize<byte>(ref buf2, len);
                        System.IO.File.WriteAllBytes("./blockchain/" + i + ".txt", buf2);

                        client.Client.Send(Encoding.UTF8.GetBytes("done"));


                    }




                    disconnectclient:
                    client.Client.Disconnect(true);
                    client.Close();
                    client = null;
                    Console.WriteLine("client");


                    //client.Client.Send(buf);                      //способ через Socket, также можно SendFile
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            Thread.Sleep(10);
        }
        public static void Server()
        {
            TcpListener listener = null;
            Server_beginning:
            try
            {
                listener = new TcpListener(IPAddress.Any, 12000);
            }
            catch
            {
                Thread.Sleep(1000);
                goto Server_beginning;
            }

            while (true)
                lock (SyncingMutex)
                {
                    try
                    {
                        Console.WriteLine("server");
                        listener.Start();
                        TcpClient client = null;

                        try
                        {
                            IAsyncResult result = listener.BeginAcceptTcpClient(null, null);
                            bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(0.3), true);
                            //client = new TcpClient("127.0.0.1", 12000); 
                            if (!success)
                                throw new Exception("Server failed to connect");
                            client = listener.EndAcceptTcpClient(result);

                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex);
                            listener.Stop();
                            //listener.Start();
                            continue;
                        }

                        client.SendTimeout = 500;
                        client.ReceiveTimeout = 500;
                        /* if (listener.Pending())
                             client = listener.AcceptTcpClient();
                         else continue;   */
                        //Console.WriteLine(((IPEndPoint)client.Client.RemoteEndPoint).Address);
                        byte[] buf = new byte[10000];

                        int blockchainSize = System.IO.Directory.GetFiles("./blockchain").Length;

                        client.Client.Send(Encoding.UTF8.GetBytes(blockchainSize.ToString()));
                        //return;
                        int len = client.Client.Receive(buf);

                        Array.Resize<byte>(ref buf, len);
                        int clientBlockchainSize = int.Parse(Encoding.UTF8.GetString(buf));
                        if (clientBlockchainSize >= blockchainSize)
                        {
                            goto disconnectserver;
                        }
                        Array.Resize<byte>(ref buf, 10000);
                        for (int i = 1; i <= blockchainSize; i++)
                        {


                            client.Client.Send(System.IO.File.ReadAllBytes("./blockchain/" + i + ".txt"));
                            client.Client.Receive(buf);
                        }


                        disconnectserver:
                        client.Client.Disconnect(true);
                        client.Close();
                        listener.Stop();


                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex);
                    }
                }

        }
    }
}
