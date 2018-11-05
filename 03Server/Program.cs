using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03Server
{
    //socket服务端
    class Program
    {
        static void Main(string[] args)
        {
            //1.创建一个socket对象

            //这个socket对象是用来 监听的Socket

            Socket skCon = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            //2.绑定ip和监听端口
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.43.67"),9999);

            skCon.Bind(endPoint);

            //3.开始监听
            skCon.Listen(5);
            Console.WriteLine("开始监听....");

            //创建与客户端通讯的socket，
            //如果 有客户端连接，那么就接受客户端的连接

            //skCon是连接socket
            //sk1才是 通讯socket
            Socket sk1 = skCon.Accept();    //这句话会阻塞线程

            Console.WriteLine("客户端{0} 已经连接！",sk1.RemoteEndPoint.ToString());

            //设置一个缓冲区，接受客户端传过来的数据

            byte[] buffers = new byte[1024*1024*5];   //5m的缓冲区

            //5.接受客户端传过来的数据
            //返回值就是实际接收到的字节个数
            int byte_count = sk1.Receive(buffers);   //这个方法用来等待 客户端发过来的信息，这句话也会阻塞线程

            //6.把用户发送过来的数据转化为字符串输出
            string msg = System.Text.Encoding.UTF8.GetString(buffers,0,byte_count);
            Console.WriteLine(msg);

            Console.ReadKey();
        }
    }
}
