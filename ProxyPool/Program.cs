using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();    
            Console.WriteLine("Press any key to close the application");
        }
        private static void Run()
        {
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = factory.GetScheduler();
                scheduler.Start();
                IJobDetail job = JobBuilder.Create<TotalJob>().WithIdentity("job1", "group1").Build();
                ITrigger trigger = TriggerBuilder.Create()
                 .WithIdentity("trigger1", "group1")
                 .StartNow()
                 .WithSimpleSchedule(
                 x => x
                .WithIntervalInMinutes(5)//5分钟一次
                 .RepeatForever()
                ).Build();
                scheduler.ScheduleJob(job, trigger);
          
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
