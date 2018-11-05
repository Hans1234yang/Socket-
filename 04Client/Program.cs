using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _04Client
{
    //Socket客户端
    class Program
    {
        static void Main(string[] args)
        {
            //1.创建一个Socket
            Socket skClient = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            //2.连接对应的服务器
            skClient.Connect("192.168.43.67",9999);

            //3.向服务端发送一个消息
            string msg = "大家好，我叫杨澹，我是.net开发者";
            byte[] buffers = System.Text.Encoding.UTF8.GetBytes(msg);
            skClient.Send(buffers);
            Console.WriteLine("发送完毕！");
            Console.ReadKey();
        }
    }
}
