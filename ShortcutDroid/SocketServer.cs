namespace ShortcutDroid
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public partial class SocketServer
    {
        string setupstring;
        string apps;
        public event ShortcutDroid.SpinnerSelectedChangedEventHandler SpinnerSelectedEvent;
        public TcpClient client = null;
        public TcpListener server = null;
        public NetworkStream stream;
        public void init(string setup, string apps)
        {
            server = null;
            setupstring = setup;
            this.apps = apps;
            

            KeysStringWrapper wrapper = new KeysStringWrapper();
            try
            {
                Int32 port = 115;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                
                server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                Console.Write("Waiting for a connection... ");
                while (client == null || client.Connected == false)
                {
                    // Perform a blocking call to accept requests.
                    client=server.AcceptTcpClient();
                }
                if(client.Connected) Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                stream = client.GetStream();
                while (client.Connected)
                {
                    data = null;
                    
                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
                        string[] dataArray=data.Split(new[] { "<sprtr>" }, StringSplitOptions.None);

                        if (dataArray[0]=="setup")
                        {
                            //client.Close();
                            //client= server.AcceptTcpClient();
                            Console.WriteLine("sending setup");
                            setupInit();
                        }
                        else if (dataArray[0]=="selected")
                        {
                            if (SpinnerSelectedEvent != null)
                                SpinnerSelectedEvent(dataArray[1]);
                        }
                        else if (dataArray[0]=="keystroke")
                        {
                            //SendKeys.SendWait(data);
                            wrapper.Send(dataArray[1]);
                            //Console.WriteLine("Got: {0}", wrapper.Send(data));
                            //Console.WriteLine("Pressed: {0}", data);
                        }
                        Console.WriteLine("Read loop exited.");
                    }
                    client.Close();
                    Console.WriteLine("Client loop exited.");
                    server.Stop();
                    init(setupstring, apps);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }

        public void setupInit()
        {
            if(client!=null&&client.Connected)
            {
                byte[] setupmsg = System.Text.Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");
                byte[] appsmsg = System.Text.Encoding.UTF8.GetBytes("apps" + apps + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
                stream.Write(appsmsg, 0, appsmsg.Length);
                Console.WriteLine("Setup: {0}\n Apps: {1}", setupstring, apps);
            }
        }

        public void appIndexChanged()
        {
            if (client != null && client.Connected)
            {
                byte[] setupmsg = System.Text.Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
                Console.WriteLine("Setup: {0}", setupstring);
            }
        }

        public void setSetup(string setup)
        {
            setupstring = setup;
        }
    }
}
