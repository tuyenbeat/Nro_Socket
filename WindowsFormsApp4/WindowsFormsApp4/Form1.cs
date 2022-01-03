using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            connect();
        }
        static List<Socket> ListClient = new List<Socket>();
        private void button1_Click(object sender, EventArgs e)
        {
            sendAll();
        }
        void connect()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    IPAddress address = IPAddress.Parse("127.0.0.1");

                    TcpListener listener = new TcpListener(address, 8888);
                    listener.Start();
                    while (true)
                    {
                        try
                        {
                            Socket socket = listener.AcceptSocket();
                            ListClient.Add(socket);
                            Thread t2 = new Thread(() =>
                            {
                                while (true)
                                {
                                    try
                                    {
                                        byte[] data = new byte[1024 * 3000];
                                        socket.Receive(data);
                                    }
                                    catch (Exception)
                                    {
                                        socket.Close();
                                        ListClient.Remove(socket);
                                    }
                                }
                            });
                            t2.IsBackground = true;     
                            t2.Start();
                        }
                        catch (Exception)
                        {
                        }

                    }
                }
                catch (Exception ex)
                {
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        void sendAll()
        {
            foreach (Socket socket in ListClient)
            {
                socket.Send(Encoding.Unicode.GetBytes(textBox1.Text));
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = ListClient.Count.ToString();
        }
    }
}
