ProxyPool
======

应用场景
----
爬虫过于频繁的抓取网站信息会被反爬虫机制屏蔽掉，或者有些网站对我们的Ip有限制，一个IP之能操作一次，这个时候就需要设置代理了。这方面需求还是很大的，有专门的服务商提供代理，没钱的自己动手打造一个代理池吧。<br>

所用的工具
------
          * Redis的C#驱动-ServiceStack.Redis
          * Html解析-HtmlAgilityPack
          * 任务调度-Quartz.NET
      
      
基本思路
----
   部分网站上有免费的代理IP信息，比如xicidaili.com，proxy360.cn。这些网站有很多免费代理IP，然而有些质量不好，需要程序及时从代理池中删掉质量低的代理，不断加入优质代理。<br>



原理图
--
![image](https://github.com/wangqifan/ProxyPool/blob/zuin/814953-20170108104513050-156986470.png)


备注
--------------
[思路来自知乎](https://www.zhihu.com/question/25566731)<br>
[博客地址](https://www.cnblogs.com/zuin)<br>
[我的知乎爬虫](https://github.com/wangqifan/ZhiHu)
