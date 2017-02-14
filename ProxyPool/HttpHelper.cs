using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPool
{
     public   class HttpHelper
    {
        public static string DownloadHtml(string url,WebProxy proxy)
        {
            string source = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
                request.Proxy = proxy;
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
