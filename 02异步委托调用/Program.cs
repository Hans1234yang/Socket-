using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace _02异步委托调用
{
    class Program
    {
        static void Main(string[] args)
        {
            //#region 调用 无参数，无返回值的委托 
            ////实例化一个委托
            //MyDelegate1 m1 = new MyDelegate1(T1);
            //IAsyncResult result= m1.BeginInvoke(null,null);///直接调用异步委托

            ////等待异步委托调用结束后，再继续后面的代码。
            ////这里使用EndInvoke()
            //m1.EndInvoke(result);
            //Console.WriteLine("OK");

            //Console.ReadKey();
            //#endregion


            //#region 调用有参数、无返回值的委托
            //MyDelegate2 m2 = new MyDelegate2(T2);
            //IAsyncResult result= m2.BeginInvoke(1, 2,null,null);  //开始异步委托调用
            ////执行异步委托结束后，才执行后面的代码
            //m2.EndInvoke(result);    //线程阻塞，等待异步委托的调用的结束

            //Console.WriteLine("ok");
            //Console.ReadKey();

            //#endregion

            #region  有参数，有返回值 的委托， 所以需要回调函数
            //实例化这个 委托
            Mydelegate3 m3 = new Mydelegate3(T3);

            //开始异步调用委托
            IAsyncResult result = m3.BeginInvoke(1, 5, new AsyncCallback(MyCallBack),null);

            // AsyncCallback 是一个有参数无返回值的委托
            //所以定义的 Mycallback方法 也需要是 有参数 无返回值。 
            //方法 又需要 和委托的 参数类型一致，所以方法 的参数也需要   MyCallBack类型


            Console.WriteLine("异步委托调用开始了");
            Console.ReadKey();

            #endregion
        }

        static void MyCallBack(IAsyncResult ar)  //ar是意思是判断是否执行完毕
        {
            //回调函数先把 IAsyncResult转化为 AsyncResult类型 ，方便后面调用
            AsyncResult result = ar as AsyncResult;

            //获取当前 被调用的委托 m3 
            Mydelegate3 currentDelegate = result.AsyncDelegate as Mydelegate3;

            //获取异步委托m3 获取的返回值
            int sum = currentDelegate.EndInvoke(ar);

            Console.WriteLine("总和是{0}",sum);

        }


        static void T1()
        {
            Console.WriteLine("您好，我是一个无参数,无返回值的方法");
        }

        static void T2(int a, int b)
        {
            Console.WriteLine("我是有参数，无返回值的方法,总和是{0}", a + b);
        }

        static int T3(int a,int b)
        {
            return a + b;
        }




    }

    //定义一个有参数，有返回值的委托
    public delegate int Mydelegate3(int a, int b);

    //定义一个有参数，无返回值的委托
    public delegate void MyDelegate2(int a, int b);

    //定义一个无参数，无返回值的委托
    public delegate void MyDelegate1();
}
