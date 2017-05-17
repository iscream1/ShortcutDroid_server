namespace ShortcutDroid
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public partial class SocketServer : IDisposable
    {
        private string setupstring;
        public event ShortcutDroid.SpinnerSelectedChangedEventHandler SpinnerSelectedEvent;
        private TcpClient client = null;
        private TcpListener server = null;
        private NetworkStream stream = null;
        private bool terminated=false;
        private AppList appList;
        public void init(AppList appList)
        {
            server = null;
            this.appList = appList;
            setSetup(0);

            SendKeysWrapper wrapper = new SendKeysWrapper();
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

                        //string got from client, syntax e.g.: "keystroke<sprtr>\c\s" as ctrl+s shortcut
                        string[] dataArray=data.Split(new[] { "<sprtr>" }, StringSplitOptions.None);

                        //initial setup needed when connecting
                        if (dataArray[0]=="setup")
                        {
                            setupInit();
                        }
                        //the user selected another app on the client
                        else if (dataArray[0]=="selected")
                        {
                            if (SpinnerSelectedEvent != null)
                                SpinnerSelectedEvent(dataArray[1]); //send the app specific shortcuts
                        }
                        //received a keystroke which should be processed and executed
                        else if (dataArray[0]=="keystroke")
                        {
                            wrapper.Send(dataArray[1]);
                        }
                        Console.WriteLine("Read loop exited.");
                    }
                    //connection was closed here 
                    client.Close();
                    Console.WriteLine("Client loop exited.");
                    server.Stop();
                    //if server is not terminated, continue listening for connections
                    if (!terminated) init(appList);
                }
            }
            //exceptions mainly appearing when closing the application
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

        //send initial setupstring
        public void setupInit()
        {
            sendApps();
            if(client!=null&&client.Connected)
            {
                byte[] setupmsg = System.Text.Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
            }
        }

        //callback called when another application is selected in the ComboBox on server UI
        public void appIndexChanged()
        {
            if (client != null && client.Connected)
            {
                byte[] setupmsg = Encoding.UTF8.GetBytes("setup<sprtr>" + setupstring + "\n");

                stream.Write(setupmsg, 0, setupmsg.Length);
                Console.WriteLine("Setup: {0}", setupstring);
            }
        }

        //send the applications list for the spinner to display on the client
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

        //set up class member setupstring with syntax: "Save all"<sprtr>"\\c\\s"<sprtr>
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

        //terminate server from outside
        public void terminate()
        {
            terminated = true;
            if (server != null) server.Stop();
            if (stream != null) stream.Close();
        }

        public void Dispose()
        {
            terminate();
        }
    }
}
