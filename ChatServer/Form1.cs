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
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        //1. 创建监听 的socket
        Socket skListener = null;

        //用来保存不同的客户端的socket对象
        Dictionary<string, Socket> dicClients = new Dictionary<string, Socket>();

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

                    //把连接的客户端socket保存在 键值对中
                    dicClients.Add(skClinet.RemoteEndPoint.ToString(),skClinet);

                    //把连接的客户端显示在listbox中 
                    listboxClients.Items.Add(skClinet.RemoteEndPoint.ToString());

                    //在日志txtLog中显示记录
                    txtLog.AppendText("客户端【" + skClinet.RemoteEndPoint.ToString() + "】 已连接"+Environment.NewLine);



                    //与每个客户端进行数据通信(接受数据，发送数据)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
                    {
                        //这里的skObj就是skClient对象
                        Socket skObj = obj as Socket;
                        byte[] buffers = new byte[1024*1024*5];
                        while(true)
                        {
                            int r = skObj.Receive(buffers);
                            if(r==0)
                            {
                                txtLog.AppendText("客户端"+skObj.RemoteEndPoint+"已退出"+Environment.NewLine);
                                listboxClients.Items.Remove(skObj.RemoteEndPoint.ToString());

                                //把对象从集合中移除
                                dicClients.Remove(skObj.RemoteEndPoint.ToString());
                                break;
                            }

                            //将收到的数据显示在文本框中
                            string msg = Encoding.UTF8.GetString(buffers,0,r);

                            //显示在文本框中
                            txtLog.AppendText("客户端"+skObj.RemoteEndPoint.ToString()+"发来消息"+msg+Environment.NewLine);
                          
                        }
                    }),skClinet);
                    
                }
            }));
            tlisten.IsBackground = true;
            tlisten.Start();

        }

        //服务器向指定的客户端发送消息 
        private void 发送_Click(object sender, EventArgs e)
        {
            //1.判断是否选择了客户端
            if(listboxClients.SelectedItem!=null)
            {
                //2.根据用户选择的客户端，找到对应的Socket对象
                Socket sk = dicClients[listboxClients.SelectedItem.ToString()];
                if(sk!=null)
                {
                    //把要发送的字符串转化为 byte数组字节流
                    byte[] buffers = Encoding.UTF8.GetBytes(txtSendMsg.Text.Trim());

                    //3调用socket对象发送数据
                    sk.Send(buffers);

                    //4.写日志
                    txtLog.AppendText("给 客户端"+listboxClients.SelectedItem.ToString()+"发送消息"+txtSendMsg.Text.Trim()+Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("请选择客户端");
            }
        }
    }
}
