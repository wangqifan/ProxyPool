using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace ProxyPool
{
     public  class Pool
    {
         public static string  GetProxy()
         {
             string result=string.Empty;
         
             try
             {
                 using (RedisClient client = new RedisClient("127.0.0.1", 6379))
                 {
                     result = client.GetRandomItemFromSet("ProxyPool");
                 }
             }
             catch { 
             }
             return result;
          
         }
         public static void  DeleteProxy(string value)
         {
             try
             {
                 using (RedisClient client = new RedisClient("127.0.0.1", 6379))
                 {
                     client.RemoveItemFromSet("ProxyPool", value);
                 }
             }
             catch
             {
                 Console.WriteLine("删除代理失败！");
             }
         }
         public static void Add(Proxy proxy)
         {
             using (RedisClient client = new RedisClient("127.0.0.1", 6379))
             {
                 if (Tool.IsAvailable(proxy))
                 {
                     Console.WriteLine(proxy.Adress +":"+ proxy.port.ToString()+"入池");
                     client.AddItemToSet("ProxyPool", proxy.Adress + ":" + proxy.port.ToString());
                 }
             }

         }
    }
}
