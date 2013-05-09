using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace UoFiddler.Telnet
{
    class Response
    {
        private int m_Code;
        private string m_Text;
        private string[] m_Data;
        private DateTime m_DateTime;

        public DateTime DateTime { get { return m_DateTime; } }

        public int Code { get{ return m_Code; } }
        
        public string this[int i] { get{ return m_Data[i]; } }

        public int Count { get{ return m_Data.Length; } }

        public override string ToString()
        {
            if (m_Code == 212)
            {
                return String.Format("[{0}] {1}", m_DateTime.ToString("HH':'mm':'ss"), m_Text);
            }

            return m_Text;//"@@@@";
        }

        private const char separator = ' ';

        static Response()
        {
        }

        public Response (string data)
        {
            string[] temp = data.Split(separator);
            Int32.TryParse(temp[0], out m_Code);

            int index = 0;
            if (m_Code == 212)
            {
                m_DateTime = System.DateTime.FromFileTime(Int32.Parse(temp[1]));
                index = 2;
            }
            else
            {
                m_DateTime = System.DateTime.Now;
                index = 1;
            }

            List <string> list = new List<string>();
            for(int i = index; i < temp.Length - 1; ++i)
                if (temp[i] != separator.ToString())
                {
                    m_Text += " " + temp[i];
                    list.Add(temp[i]);
                }
            if(list.Count > 0)
                m_Data = list.ToArray();
        }
    }

    public class Client
    {
        public static string Host { get{ return m_Host;} }
        private static string m_Host = 
                                        #if DEBUG 
                                        "localhost";
                                        #else
                                        "localhost";
                                        #endif

        public static int Port { get{ return m_Port;} }
        private static int m_Port = 
                                        #if DEBUG 
                                        0000;       // USE NOT ZERO VALUE TO ACTIVATE TELNET CONECTION
                                        #else 
                                        0000; 
                                        #endif

        public static string Character { get{ return m_Character;} set{ m_Character = value;} }
        private static string m_Character = "StaticZ";



        public void WriteLine(string line)
        {
        }

        public void ReadLine(string line)
        {
            telnet.Send(line + telnet.CRLF);
        }

        private static BackgroundWorker worker;
        private static TelnetWrapper telnet; 
		private static bool done = false;
		private static StreamReader sr;
        private static bool m_logrflag = false;
       

        public Client() 
		{
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(InteractiveSession);

            telnet = new TelnetWrapper();

            if (Port <= 0 || Port >= 65536) {
                Console.WriteLine("Telnet client disable (see source code to turn ON)\nNOTE: There is bug - once client is enabled Fiddler+ will crash if server will unavailable...");
                return;
            }

            telnet.Disconnected += new DisconnectedEventHandler(this.OnDisconnect);
            telnet.DataAvailable += new DataAvailableEventHandler(this.OnDataAvailable);

            telnet.TerminalType = "NETWORK-VIRTUAL-TERMINAL";
            telnet.Hostname = Host;
            telnet.Port = Port;
            telnet.CRLF = "\n";

			Console.WriteLine("Connecting ...");
            telnet.Connect();

            try
            {
                InteractiveSession(null, null);// edit the method for specific cmds
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has occurred: " +
                    e.Message + "\n\n" + e.StackTrace + "\n");
            }

            //worker.RunWorkerAsync();
		}

		public void ScriptedSession()
		{
			Console.WriteLine("Not implemented.");
		}

		private static void InteractiveSession(object sender, DoWorkEventArgs e)
		{
			int i;
			char ch;
		
			try 
			{
                telnet.Receive();
			
				//while (!done)
                    //telnet.Send(Console.ReadLine() + telnet.CRLF);
			}
			catch 
			{
                telnet.Disconnect();
				throw;
			}
		}

		private void OnDisconnect(object sender, EventArgs e)
		{
			done = true;
			Console.WriteLine("\nDisconnected.");
		}

        private static List<string> str = new List<string>();

		private void OnDataAvailable(object sender, DataAvailableEventArgs e)
		{
		    string data = e.Data;
            //Response response = new Response(data);
            str.Add(data);
            switch (data.Split(' ')[0])
		    {
                case "220": if(FiddlerControls.Options.Telnet_active)
                                telnet.Send("USER " + FiddlerControls.Options.Account + telnet.CRLF);
                            break;
                case "331": if (FiddlerControls.Options.Telnet_active) 
                                telnet.Send("PASS " + FiddlerControls.Options.Password + telnet.CRLF);
                            break;
                case "230": if (FiddlerControls.Options.Telnet_conl)
                                telnet.Send("CONL on" + telnet.CRLF); 
                            break;
                case "200": if (FiddlerControls.Options.Telnet_logr > 0 && !m_logrflag)
                            {
                                telnet.Send("LOGR 1000" + telnet.CRLF);
                                m_logrflag = true;
                            }
                            break; 
		    }
            //return;
		    //string text = response.ToString();
            Console.WriteLine(data);
		}
    }
}
