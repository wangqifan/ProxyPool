using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using System.Net;
using System.IO;

namespace ProxyPool
{
    class TotalJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            PoolManage manager = new PoolManage();
            manager.Initial();
        }
    }
}
