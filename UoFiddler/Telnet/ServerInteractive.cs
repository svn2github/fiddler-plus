using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UoFiddler.Telnet
{
    public partial class ServerInteractive : UserControl
    {
        private StringRedir RedirConsole;
        private TextWriter ConsoleWriter;

        public ServerInteractive()
        {
            InitializeComponent();
            ConsoleWriter = Console.Out;	// Save the current console TextWriter. 
            RedirConsole = new StringRedir(ref this.richTextBox1);
            Console.SetOut(RedirConsole);	// Set console output to the StringRedir clas
        }

        private void onLoad(object sender, EventArgs e)
        {
            //UoFiddler form = this.ParentForm as UoFiddler;
            //( form.Controls[form.Controls.IndexOf(this.Parent)] as TabControl ).SelectTab(21);
            //this.DataBindings.Add(Console.Out)
        }

        private static bool echo = true;
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                string command = textBox1.Text;

                if (command.ToLower() == "clrscr")
                {
                    richTextBox1.Text = String.Empty;
                    if (echo)
                    {
                        Console.WriteLine(">> " + command + " : \r\n");
                        Console.WriteLine();
                    }
                }
                else if (command.ToLower().StartsWith("echo"))
                {
                    foreach (string arg in command.ToLower().Split(' '))
                    {
                        if(arg == "echo" || arg == String.Empty)
                            continue;
                        else if(arg == "on")
                        {
                            if(echo)
                                Console.WriteLine(">> " + command + " : \r\n");
                            echo = true;
                            Console.WriteLine("<< Режим вывода команд на экран (ECHO) включен.\r\n");
                            break;
                        }
                        else if (arg == "off")
                        {
                            if (echo)
                                Console.WriteLine(">> " + command + " : \r\n");
                            echo = false;
                            Console.WriteLine("<< Режим вывода команд на экран (ECHO) отключен.\r\n");
                            break;
                        }
                        else
                        {
                            if (echo)
                            {
                                Console.WriteLine(">> " + command + " : \r\n");
                                Console.WriteLine("<< Режим вывода команд на экран (ECHO) включен.\r\n");
                            }
                            else
                                Console.WriteLine("<< Режим вывода команд на экран (ECHO) отключен.\r\n");
                            break;
                        }
                    }
                }
                else
                {
                    if (echo)
                    {
                        Console.WriteLine(">> " + command + " : \r\n");
                        Console.WriteLine();
                    }
                    UoFiddler.Telnet.ReadLine(command);
                }

                textBox1.Text = String.Empty;
            }
        }

    }

    public class StringRedir : StringWriter
    { // Redirecting Console output to RichtextBox
        private RichTextBox outBox;

        #region WinApi Import 
        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const int WM_VSCROLL = 277;
        const int SB_BOTTOM = 7;

        private void ScrollToEnd(Control rtb)
        {
            IntPtr ptrWparam = new IntPtr(SB_BOTTOM);
            IntPtr ptrLparam = new IntPtr(0);
SendMessage(rtb.Handle, WM_VSCROLL, ptrWparam, ptrLparam); // Дебагер не пашет с етим
        }
        #endregion

        public StringRedir(ref RichTextBox textBox)
        {
            outBox = textBox;
        }

        public override void WriteLine(string x)
        {
outBox.Text += x;// +"\n";  // Дебагер не пашет с етим
            //outBox.Refresh();
            ScrollToEnd(outBox);
        }
    }
}
