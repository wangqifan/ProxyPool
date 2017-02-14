using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPool
{
     public  class Tool
    {
         public static bool IsAvailable(Proxy proxy)
         {
             bool result = false;
             try
             {
                 WebProxy webproxy = new WebProxy(proxy.Adress, proxy.port);
                 string html = HttpHelper.DownloadHtml("https://www.baidu.com/", webproxy);
                 if (html.Contains("百度一下，你就知道"))
                 {
                     result = true;
                 }
             }
             catch
             {

             }
             return result;
         }
    }
}
