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

namespace ChatClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        //1.声明一个通信 socket
        Socket skClient = null;
        private void button1_Click(object sender, EventArgs e)
        {          
            if(skClient==null)
            {
                skClient = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            }
            //2.连接服务器
            skClient.Connect(new IPEndPoint(IPAddress.Parse(txtIP.Text.Trim()),int.Parse(txtPort.Text.Trim())));

            txtLog.AppendText("连接服务器"+txtIP.Text.Trim()+":"+txtPort.Text.Trim()+"成功"+Environment.NewLine);

            //连接完毕后，需要不断接收用户发来的消息
            //使用另外一个线程接受用户发来的消息
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                while(true)
                {
                    //判断是否有可用的字节，如果有，然后再接收
                    if(skClient!=null&&skClient.Available>0)
                    {
                        //接受服务器传来的消息
                        int r = skClient.Receive(buffer); //这句话会阻塞线程

                        string msg = Encoding.UTF8.GetString(buffer,0,r);

                        txtLog.AppendText("服务器发来消息"+msg+Environment.NewLine);
                    }
                }
            }));
        }
        //客户端向服务端发送消息
        private void button2_Click(object sender, EventArgs e)
        {
            //1.获取用户的输入
            string msg = txtSendMsg.Text.Trim();

            //2.调用Socket发送数据 
            if(skClient!=null&&skClient.Connected)
            {
                //向服务器发消息
                skClient.Send(Encoding.UTF8.GetBytes(msg));
                //3.写日志
                txtLog.AppendText("向服务器发送消息："+msg+Environment.NewLine);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            skClient.Shutdown(SocketShutdown.Both);
            skClient.Close();
            skClient.Dispose();
            skClient = null;
        }
    }
}
