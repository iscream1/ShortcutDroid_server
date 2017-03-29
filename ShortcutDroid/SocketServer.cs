﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutDroid
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public partial class SocketServer
    {
        string setupstring;
        public void init(string setup)
        {
            TcpListener server = null;
            setupstring = setup; //= "button1,button2,button3\n";
            TcpClient client = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 115;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                
                server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                Console.Write("Waiting for a connection... ");
                // Enter the listening loop.
                while (client == null || client.Connected == false)
                {
                    

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    client=server.AcceptTcpClient();
                    
                    // Shutdown and end connection
                    //client.Close();
                }
                if(client.Connected) Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();
                while (client.Connected)
                {
                    data = null;
                    
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        if (data.Equals("setup"))
                        {
                            //client.Close();
                            //client= server.AcceptTcpClient();
                            stream = client.GetStream();
                            Thread.Sleep(100);

                            byte[] setupmsg = System.Text.Encoding.ASCII.GetBytes("setup," + setupstring+"\n");
                            stream.Write(setupmsg, 0, setupmsg.Length);
                            Console.WriteLine("Sent: {0}", setupstring);
                        }
                        else
                        {
                            SendKeys.SendWait(data);
                            Console.WriteLine("Pressed: {0}", data);
                        }
                        Console.WriteLine("Read loop exited.");
                    }
                    client.Close();
                    Console.WriteLine("Client loop exited.");
                    server.Stop();
                    init(setupstring);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                //client.Close();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public void setSetup(string setup)
        {
            setupstring = setup;
        }
    }
}
