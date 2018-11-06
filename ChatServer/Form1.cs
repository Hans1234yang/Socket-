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

namespace ChatServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //1. 创建监听 的socket
        Socket skListener = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if(skListener==null)
            {
                //new 一个socket对象
                skListener = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            }

            //2.绑定ip 和监听的端口
            skListener.Bind(new IPEndPoint( IPAddress.Parse("192.168.43.67"), 9999));

            //3.开始监听 
            skListener.Listen(10);

            txtLog.AppendText("开始监听...."+Environment.NewLine);
            label1.ForeColor = Color.Green;
            label1.Font = new Font("宋体",15);
            label1.Text = "监听";


            //因为 accpet() 方法会阻塞线程，所以写在 thread 里面
            Thread tlisten=new Thread(new ThreadStart(()=>
            {
                while(true)
                {
                    //创建与用户通讯的sosket
                    Socket skClinet = skListener.Accept();// 这句话会阻塞线程所以，需要写在 单独线程里

                    //把连接的客户端显示在listbox中 
                    listboxClients.Items.Add(skClinet.RemoteEndPoint.ToString());

                    //在日志txtLog中显示记录
                    txtLog.AppendText("客户端【" + skClinet.RemoteEndPoint.ToString() + "】 已连接"+Environment.NewLine);
                }
            }));

        }
    }
}
