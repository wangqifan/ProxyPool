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
         public static void  PushProxy(string value)
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
    }
}
