using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Redis;
using System.IO.Compression;

namespace ProxyPool
{
    public class PoolManage
    {    
        public void Initial()
        {
            ThreadPool.QueueUserWorkItem(Downloadxicidaili);
            ThreadPool.QueueUserWorkItem(Downkuaidaili);
            ThreadPool.QueueUserWorkItem(Downloadproxy360);
         
            
        }
        public void Add(Proxy proxy)
        {
            using (RedisClient client = new RedisClient("127.0.0.1", 6379))
            {
                if (IsAvailable(proxy))
                {
                    Console.WriteLine(proxy.Adress);
                    client.AddItemToSet("ProxyPool", proxy.Adress + ":" + proxy.port.ToString());
                }
            }

        }
        public void Downloadxicidaili(object DATA)//下载西刺代理的html页面
        {
            try
            {
                List<string> list = new List<string>()
                {
                    "http://www.xicidaili.com/nt/",
                    "http://www.xicidaili.com/nn/",
                    "http://www.xicidaili.com/wn/",
                    "http://www.xicidaili.com/wt/"

                };
                foreach (var utlitem in list)
                {
                    string url = utlitem;
                    string html = DownloadHtml(url);

                    HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNode node = doc.DocumentNode;
                    string xpathstring = "//tr[@class='odd']";
                    HtmlNodeCollection collection = node.SelectNodes(xpathstring);
                    foreach (var item in collection)
                    {
                        Proxy proxy = new Proxy();
                        string xpath = "td[2]";
                        proxy.Adress = item.SelectSingleNode(xpath).InnerHtml;
                        xpath = "td[3]";
                        proxy.port = int.Parse(item.SelectSingleNode(xpath).InnerHtml);
                        Console.WriteLine(proxy.Adress);
                        Add(proxy);

                    }
                }
              
                Console.WriteLine("西刺");
            }catch
            {

            }
        }
        public void Downkuaidaili(object DATA)//下载快代理
        {
            try
            {
           
                string url = "http://www.xicidaili.com/nt/";
                for (int i = 1; i < 4; i++)
                {
                    string html = DownloadHtml(url+i.ToString());
                    string xpath = "//tbody/tr";
                    HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNode node = doc.DocumentNode;
                    HtmlNodeCollection collection = node.SelectNodes(xpath);
                    foreach (var item in collection)
                    {
                        Proxy proxy = new Proxy();
                        proxy.Adress = item.FirstChild.InnerHtml;
                        xpath = "td[2]";
                        proxy.port = int.Parse(item.SelectSingleNode(xpath).InnerHtml);
                        Console.WriteLine(proxy.Adress);
                        Add(proxy);
                    }
                }
                
            }
            catch
            {

            }
           
        }
        public void Downloadproxy360(object DATA)//下载proxy360
        {
            try
            {
                string url = "http://www.proxy360.cn/default.aspx";
                string html = DownloadHtml(url);
                HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                string xpathstring = "//div[@class='proxylistitem']";
                HtmlNode node = doc.DocumentNode;
                HtmlNodeCollection collection = node.SelectNodes(xpathstring);
                foreach (var item in collection)
                {
                    Proxy proxy = new Proxy();
                    var childnode = item.ChildNodes[1];
                    xpathstring = "span[1]";
                    proxy.Adress = childnode.SelectSingleNode(xpathstring).InnerHtml.Trim();
                    xpathstring = "span[2]";
                    proxy.port = int.Parse(childnode.SelectSingleNode(xpathstring).InnerHtml);
                    Console.WriteLine(proxy.Adress);
                    Add(proxy);
                }
                Console.WriteLine("proxy360");
            }
            catch
            {

            }
        }
        public static bool IsAvailable(Proxy proxy)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.baidu.com/");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
                WebProxy webproxy=new WebProxy(proxy.Adress,proxy.port);
                request.Proxy=webproxy;
                request.Timeout = 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
                    {
                        if (reader.ReadToEnd().Contains("百度"))
                        {
                            result = true;
                        }
                      
                    }
                }
                request.Abort();
            }
            catch
            {
                
            }
            return result;
        }
        public string DownloadHtml(string url)
        {
            string source = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    source = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    source = reader.ReadToEnd();
                                }

                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())//原始
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {

                                    source = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                request.Abort();
            }
            catch
            {

            }
            return source;
         
        }
       
    }
}
