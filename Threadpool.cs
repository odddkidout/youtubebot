using System;
using System.IO;
using System.Threading;
using Amib.Threading;
using Console = Colorful.Console;

namespace Youtubebot
{
    public class Threadpool
    {
        public static SmartThreadPool smartThreadPool;
        public Threadpool()
        {
            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = MainClass.rnd.Next(200, 1000) * 1000;
            stpStartInfo.MaxWorkerThreads = Convert.ToInt32(MainClass.currentConfig.maxThreads);
            stpStartInfo.MinWorkerThreads = Convert.ToInt32(1);
            stpStartInfo.PerformanceCounterInstanceName = "Test SmartThreadPool";
            smartThreadPool = new SmartThreadPool(stpStartInfo);
            Console.WriteLine("threadpool started");
            
        }
        public static void threadManager()
        {

            while (true)
            {
                try
                {
                    int diff = MainClass.currentConfig.maxThreads - activeThreadsCount();
                    if (diff > 0)
                    {
                        initialiseThreads(diff);
                    }
                    Thread.Sleep(MainClass.currentConfig.delayThreads * 1000);
                }
                catch (Exception ex)
                {
                    Helper.errorlogger(ex, true);
                }

            }


        }
        static int activeThreadsCount()
        {
            return (Threadpool.smartThreadPool.InUseThreads);
        }
        static void initialiseThreads(int diff)
        {
            for (int i = 0; i < diff; i++)
            {
                if (MainClass.currentConfig.proxys.Length == 0)
                {
                    Console.WriteLine("proxy text file found but no data in proxy text path file");
                    string path = Path.Combine(MainClass.currentPath, "errors.txt");
                    if (File.Exists(path))
                    {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLineAsync("no data in proxy text file" + "\n\n\n\n");
                        }
                    }
                    else
                    {
                        File.WriteAllText(Path.Combine(MainClass.currentPath, "errors.txt"), "no data in proxy text file" + "\n\n");
                    }
                }
                try
                {
                    Threadpool.smartThreadPool.QueueWorkItem(() => new Browser());
                    Thread.Sleep(MainClass.currentConfig.delayThreads * 1000);
                }
                catch (Exception ex)
                {
                    Helper.errorlogger(ex, true);
                }
            }
        }
    }
}