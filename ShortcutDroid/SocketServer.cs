namespace ShortcutDroid
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public partial class SocketServer
    {
        string setupstring;
        public event ShortcutDroid.SpinnerSelectedChangedEventHandler SpinnerSelectedEvent;
        public TcpClient client = null;
        public TcpListener server = null;
        public NetworkStream stream = null;
        public bool terminated=false;
        AppList appList;
        public void init(AppList appList)
        {
            server = null;
            this.appList = appList;
            setSetup(0);

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
                    if(!terminated) init(appList);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Terminated:\n"+e);
            }
            catch(System.IO.IOException e)
            {
                Console.WriteLine("Stream closed:\n" + e);
            }
            finally
            {
                server.Stop();
            }
        }

        public void setupInit()
        {
            sendApps();
            if(client!=null&&client.Connected)
            {
                byte[] setupmsg = System.Text.Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
            }
        }

        public void appIndexChanged()
        {
            if (client != null && client.Connected)
            {
                byte[] setupmsg = Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
                Console.WriteLine("Setup: {0}", setupstring);
            }
        }

        public void sendApps()
        {
            if (client != null && client.Connected)
            {
                StringBuilder appsSb = new StringBuilder();
                foreach (var app in appList.Apps)
                {
                    appsSb.Append("<sprtr>" + app.Name);
                }

                byte[] appsmsg = System.Text.Encoding.UTF8.GetBytes("apps" + appsSb.ToString() + "\n");

                stream.Write(appsmsg, 0, appsmsg.Length);
            }
        }

        public void setSetup(int idx)
        {
            StringBuilder setupSb = new StringBuilder();
            setupSb.Append(appList.Apps[idx].Name + "<sprtr>");
            BindingList<Shortcut> list = appList.Apps[idx].ShortcutList;
            for (int i = 0; i < list.Count; i++)
            {
                Shortcut s = list[i];
                Console.WriteLine(s.Keystroke);
                setupSb.Append(s.Label + "<sprtr>" + s.Keystroke + "<sprtr>");
            }
            setupstring = setupSb.ToString();
        }

        public void terminate()
        {
            terminated = true;
            if (server != null) server.Stop();
            if (stream != null) stream.Close();
        }
    }
}
