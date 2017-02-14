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
    public class PoolSpider
    {    
        public void Initial()
        {
            ThreadPool.QueueUserWorkItem(Downloadxicidaili);
            ThreadPool.QueueUserWorkItem(Downkuaidaili);
            ThreadPool.QueueUserWorkItem(Downloadproxy360);
         
            
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
                    string html = HttpHelper.DownloadHtml(url,null);

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
                        Console.WriteLine("来自西刺 " + proxy.Adress + ":" + proxy.port.ToString());
                        Task.Run(() =>
                        {
                            Pool.Add(proxy);
                        }
                           );
                    }
                }
            }catch
            {

            }
        }
        public void Downkuaidaili(object DATA)//下载快代理
        {
            try
            {

                string url = "http://www.kuaidaili.com/proxylist/";
                for (int i = 1; i < 4; i++)
                {
                    string html =HttpHelper.DownloadHtml(url+i.ToString(),null);
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
                        Console.WriteLine("来自快代理 " + proxy.Adress + ":" + proxy.port.ToString());
                        Task.Run(() =>
                        {
                            Pool.Add(proxy);
                        }
                           );
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
                string html =HttpHelper.DownloadHtml(url,null);
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
                    Console.WriteLine("来自proxy360 "+proxy.Adress+":"+proxy.port.ToString());
                    Task.Run(() => {
                        Pool.Add(proxy);
                        }
                       );
                    
                   
                }
            }
            catch
            {

            }
        }
    }
}
