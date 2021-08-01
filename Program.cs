using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Specialized;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Amib.Threading;
using System.Net;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
namespace maincore
{
    [Serializable]
    class user
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
    class Accounts
    {
        public string Emailid { get; set; }
        public string Password { get; set; }
        public List<cookie> accCookies { get; set; }
    }
    class cookie
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    class Configuration
    {
        public string name { get; set; }
        public int minPlaytime { get; set; }
        public int maxPlaytime { get; set; }
        public int likeChances { get; set; }
        public int subscribeChances { get; set; }
        public int commentChances { get; set; }
        public int maxThreads { get; set; }
        public int delayThreads { get; set; }
        public string urlsPath { get; set; }
        public string[] urls { get; set; }
        public int useProxy { get; set; }
        public int proxyProto { get; set; }
        public string proxyPath { get; set; }
        public string[] proxys { get; set; }
        public string[] UA { get; set; }
        public int streams { get; set; }
        public int AdClick { get; set; }
        public int useAccounts { get; set; }
        public string searchKeyWordsPath { get; set; }
        public string[] searchKeyWords { get; set; }
        public string suggestionUrlPath { get; set; }
        public string[] suggestionUrls { get; set; }
        public int searchplaypercent { get; set; }
        public int keywordplaypercent { get; set; }
        public int directplaypercent { get; set; }
        public int suggestedurlplaypercent { get; set; }
        public int googlesearch { get; set; }
        public int homepageplaypercent { get; set; }
        public int trendingpageplaypercent { get; set; }
        public int explorepageplaypercent { get; set; }
        public int gamingpageplaypercent { get; set; }
        public int musicpageplaypercent { get; set; }
    }
    class ASCII
    {
        public static void ASCIII()
        {


            Colorful.Console.WriteLine("");
            Colorful.Console.WriteLine(" ▄████████    ▄████████    ▄████████    ▄█    █▄    ▀█████████▄      ███     ", Color.BlueViolet);
            Colorful.Console.WriteLine("███    ███   ███    ███   ███    ███   ███    ███     ███    ███  ▀█████████▄ ", Color.BlueViolet);
            Colorful.Console.WriteLine("███    █▀    ███    ███   ███    █▀    ███    ███     ███    ███   ▀███▀▀██ ", Color.BlueViolet);
            Colorful.Console.WriteLine("███          ███    ███   ███         ▄███▄▄▄▄███▄▄  ▄███▄▄▄██▀     ███   ▀ ", Color.BlueViolet);
            Colorful.Console.WriteLine("███        ▀███████████ ▀███████████ ▀▀███▀▀▀▀███▀  ▀▀███▀▀▀██▄     ███     ", Color.BlueViolet);
            Colorful.Console.WriteLine("███    █▄    ███    ███          ███   ███    ███     ███    ██▄    ███     ", Color.BlueViolet);
            Colorful.Console.WriteLine("███    ███   ███    ███    ▄█    ███   ███    ███     ███    ███    ███     ", Color.BlueViolet);
            Colorful.Console.WriteLine("████████▀    ███    █▀   ▄████████▀    ███    █▀    ▄█████████▀    ▄████▀   ", Color.BlueViolet);
            Colorful.Console.WriteLine("                                                                                                  ", Color.BlueViolet);
            Colorful.Console.WriteLine("");
            Colorful.Console.WriteLine("");

        }
    }
    class MainClass
    {
        static bool isAuthenticated = false;
        public static string version = "1.0";
        public static int streams = 0;
        static DateTime start;
        public static Configuration currentConfig;
        private static Mutex errorMutex = new Mutex();
        static Random rnd = new Random();
        static public string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static SmartThreadPool smartThreadPool;

        public static string RandomString(int length = 32)
        {
            while (true)
            {
                var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[rnd.Next(chars.Length)];
                }
                if (!(File.Exists(Path.Combine(currentPath, "logs", new String(stringChars)))))
                {
                    return new String(stringChars);
                }
            }

        }
        static void Main()
        {
            if (!isAuthenticated)
            {
                logo();
                new Authentication();
                Console.WriteLine("Welcome to the best youtube bot!\n1.Login");
                int tempauth = Convert.ToInt32(Console.ReadLine());
                if (tempauth == 1)
                {
                    logo();
                    Console.WriteLine("Enter Username");
                    string username = Console.ReadLine();
                    Console.WriteLine("Enter Password");
                    string password = Console.ReadLine();
                    bool auth = Authentication.Login(username, password);
                    if (!auth)
                    {
                        isAuthenticated = false;
                        Main();
                    }
                    else if (auth)
                    {
                        Console.WriteLine("Authenticated");
                        isAuthenticated = true;
                        Main();
                    }
                    
                }
                else
                {
                    Console.WriteLine("invalid Input");
                    Console.ReadLine();
                    Main();
                }
            }
            logo();
            System.Console.Write("\n");
            System.Console.WriteLine("1 => Load config/start bot");
            System.Console.WriteLine("2 => Make config");
            System.Console.Write("3 => setup discord webhook\n\n");
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(System.Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                Thread.Sleep(2000);
                Main();
            }
            catch (Exception ex)
            {
                errorlogger(ex, true);
                Console.Clear();
                Main();
            };
            if (temp == 1)
            {
                try
                {
                    loadConfig();
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                    Console.Clear();
                    Main();
                }
            }
            else if (temp == 2)
            {
                try
                {
                    makeConfig();
                    System.Console.WriteLine("Config Created");
                    Thread.Sleep(2000);
                    Main();
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                    Main();
                }
            }
            else if (temp == 3)
            {
                try
                {
                    System.Console.WriteLine("still in progress");
                    Thread.Sleep(3000);
                    Main();
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                    Console.Clear();
                    Main();
                }
            }
            else
            {
                System.Console.WriteLine("invalid selection");
                Main();
            }
            logo();
            System.Console.WriteLine("No of streams for unlimited put 0");
            try
            {
                currentConfig.streams = Convert.ToInt32(System.Console.ReadLine());
                initialise();
                new Thread(threadManager).Start();
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Clear();
                Main();
            }
            catch (Exception ex)
            {
                errorlogger(ex, true);
                Console.Clear();
                Main();
            }

            while (true)
            {
                logo();
                var diff = DateTime.Now - start;
                Console.WriteLine($"Running from : {diff.Hours}");
                Console.WriteLine($"streams Completed : {streams} ");
                Console.WriteLine($"threads : {activeThreadsCount()}/{smartThreadPool.ActiveThreads} ");
                Console.WriteLine($"Threads Capped at : {currentConfig.maxThreads} ");
                Console.WriteLine($"Streams in 24 hours : {(streams / diff.TotalSeconds) * 3600 * 24} ");
                Thread.Sleep(1000);

            }
        }
        public static void errorlogger(Exception ex, bool console)
        {
            string path = Path.Combine(currentPath, "error.txt");
            if (console)
            {
                Console.Clear();
                Console.BackgroundColor = Color.Blue;
                Console.ForegroundColor = Color.Red;
                Console.WriteLine(ex);
                Console.WriteLine("\tERROR\n\nCheck errors.txt");
                Console.ResetColor();
            }
            errorMutex.WaitOne();
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ex + "\n\n");
                }

            }
            finally
            {
                errorMutex.ReleaseMutex();
            }

        }public static void errorlogg(Exception ex, bool console)
        {
            string path = Path.Combine(currentPath, "error.txt");
            if (console)
            {
                Console.Clear();
                Console.BackgroundColor = Color.Blue;
                Console.ForegroundColor = Color.Red;
                Console.WriteLine(ex);
                Console.WriteLine("\tERROR\n\nCheck errors.txt");
                Console.ResetColor();
            }
            errorMutex.WaitOne();
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ex + "\n\n");
                }

            }
            finally
            {
                errorMutex.ReleaseMutex();
            }

        }
        public static async void Threadlogger(string Threadid, String update)
        {

            string path = Path.Combine(currentPath, "logs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, $"{Threadid}.txt");
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(update);
                }

            }
            catch (Exception ex)
            {
                errorlogger(ex, true);
            }

        }
        static void logo()
        {
            Console.Clear();
            ASCII.ASCIII();
            Console.WriteLine("");
        }
        static int activeThreadsCount()
        {
            return (smartThreadPool.InUseThreads);
        }
        static void initialiseThreads(int diff)
        {
            for (int i = 0; i < diff; i++)
            {
                if (currentConfig.proxys.Length == 0)
                {
                    Console.WriteLine("proxy text file found but no data in proxy text path file");
                    string path = Path.Combine(currentPath, "errors.txt");
                    if (File.Exists(path))
                    {

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLineAsync("no data in proxy text file" + "\n\n\n\n");
                        }
                    }
                    else
                    {
                        File.WriteAllText(Path.Combine(currentPath, "errors.txt"), "no data in proxy text file" + "\n\n");

                    }
                }
                try
                {
                    smartThreadPool.QueueWorkItem(() => run());
                    Thread.Sleep(currentConfig.delayThreads * 1000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
        }

        public static void run()
        {
            new browser();
        }
        public static void initialise()
        {
            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = rnd.Next(200, 1000) * 1000;
            stpStartInfo.MaxWorkerThreads = Convert.ToInt32(currentConfig.maxThreads);
            stpStartInfo.MinWorkerThreads = Convert.ToInt32(1);
            stpStartInfo.PerformanceCounterInstanceName = "Test SmartThreadPool";

            smartThreadPool = new SmartThreadPool(stpStartInfo);
            Console.WriteLine("threadpool started");
            start = DateTime.Now;
        }
        static void threadManager()
        {

            while (true)
            {
                try
                {
                    int diff = currentConfig.maxThreads - activeThreadsCount();
                    if (diff > 0)
                    {
                        initialiseThreads(diff);
                    }
                    Thread.Sleep(currentConfig.delayThreads * 1000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }

            }


        }
        public static void makeConfig()
        {
            Configuration configuration = new Configuration();
            logo();
            bool verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    Console.WriteLine("Enter Config Name");
                    configuration.name = Console.ReadLine();
                    verify = false;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter minimum playtime (in sec)");
                    configuration.minPlaytime = Convert.ToInt32(System.Console.ReadLine());
                    verify = false;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter maximum playtime (in sec)");
                    configuration.maxPlaytime = Convert.ToInt32(System.Console.ReadLine());
                    verify = false;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("use Accounts (0-NO,1-yes)");
                    configuration.useAccounts = Convert.ToInt32(System.Console.ReadLine());
                    if (configuration.useAccounts == 0 ^ configuration.useAccounts == 1)
                    {
                        verify = false;
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            if (configuration.useAccounts == 1)
            {
                verify = true;
                while (verify)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("Enter chances for like (0-100");
                        configuration.likeChances = Convert.ToInt32(System.Console.ReadLine());
                        System.Console.WriteLine("Enter chances for comment (0-100");
                        configuration.commentChances = Convert.ToInt32(System.Console.ReadLine());
                        System.Console.WriteLine("Enter chances for subscribe (0-100");
                        configuration.subscribeChances = Convert.ToInt32(System.Console.ReadLine());
                        verify = false;
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
            }
            else
            {
                configuration.likeChances = 0;
                configuration.commentChances = 0;
                configuration.subscribeChances = 0;
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter chances for Ad Click per 1000 views");
                    configuration.AdClick = Convert.ToInt32(System.Console.ReadLine());
                    verify = false;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter No of Threads to Operate\n");
                    configuration.maxThreads = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("Enter delay\n");
                    configuration.delayThreads = Convert.ToInt32(System.Console.ReadLine());
                    verify = false;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Do you wanna use Proxies? (0-No,1-yes");
                    configuration.useProxy = Convert.ToInt32(System.Console.ReadLine());
                    if (configuration.useProxy == 0 ^ configuration.useProxy == 1)
                    {
                        verify = false;
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            if (configuration.useProxy == 1)
            {

                verify = true;
                while (verify)
                {
                    try
                    {
                        logo();
                        System.Console.WriteLine("proxy protocol\n enter 1 for http/https\n enter 2 for socks");
                        configuration.proxyProto = Convert.ToInt32(System.Console.ReadLine());
                        if (configuration.proxyProto == 1 ^ configuration.proxyProto == 2)
                        {
                            System.Console.WriteLine("Enter proxy path");
                            configuration.proxyPath = System.Console.ReadLine();
                            verify = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Protocol Input");
                        }
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }

            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter urls path");
                    configuration.urlsPath = System.Console.ReadLine();
                    verify = false;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter Keywords Path if using suggestion");
                    configuration.searchKeyWordsPath = System.Console.ReadLine();
                    verify = false;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
            verify = true;
            while (verify)
            {
                try
                {
                    logo();
                    System.Console.WriteLine("Enter suggestion url text file Path if using suggestion");
                    configuration.suggestionUrlPath = System.Console.ReadLine();
                    verify = false;
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }


            while (!(configuration.searchplaypercent
                + configuration.suggestedurlplaypercent
                + configuration.keywordplaypercent
                + configuration.directplaypercent
                + configuration.googlesearch
                + configuration.explorepageplaypercent
                + configuration.gamingpageplaypercent
                + configuration.homepageplaypercent
                + configuration.musicpageplaypercent
                + configuration.trendingpageplaypercent == 100))
            {
                verify = true;
                while (verify)
                {
                    try
                    {
                        logo();
                        Console.ForegroundColor = Color.Yellow;
                        System.Console.WriteLine("Enter Search Play Percent\n\t( It will search video url in search box for impressions increase)", Console.ForegroundColor);
                        try
                        {
                            configuration.searchplaypercent = Convert.ToInt32(System.Console.ReadLine());
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Suggested URL Play Percent ( For suggestion views )");
                        try
                        {
                            configuration.suggestedurlplaypercent = Convert.ToInt32(System.Console.ReadLine());
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Keyword play percent( Train video on keywords)");
                        try
                        {
                            configuration.keywordplaypercent = Convert.ToInt32(System.Console.ReadLine());
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Direct Play Percent");
                        try
                        {
                            configuration.directplaypercent = Convert.ToInt32(System.Console.ReadLine());
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Google Search Play Percent");
                        try
                        {
                            configuration.googlesearch = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Home Page Play Percent");
                        try
                        {
                            configuration.homepageplaypercent = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Explore Page Play Percent");
                        try
                        {
                            configuration.explorepageplaypercent = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Trending Page Play Percent");
                        try
                        {
                            configuration.trendingpageplaypercent = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Gaming Page Play Percent");
                        try
                        {
                            configuration.gamingpageplaypercent = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        System.Console.WriteLine("Enter Music Page Play Percent");
                        try
                        {
                            configuration.musicpageplaypercent = Convert.ToInt32(System.Console.ReadLine()); ;
                        }
                        catch (System.FormatException)
                        {
                            Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                            Thread.Sleep(2000);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                        }
                        verify = false;
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }
            }
            File.WriteAllText(Path.Combine(currentPath, "config", configuration.name + ".txt"), JsonConvert.SerializeObject(configuration, Formatting.Indented));
        }
        public static void loadConfig()
        {
            logo();
            int selection = -1;
            string configPath = Path.Combine(currentPath, "config");
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            string[] configs = Directory.GetFiles(configPath, "*.txt");
            System.Console.Write("Choose Config Number \n");
            for (int i = 0; i < configs.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(configs[i])}");
            }
            Console.WriteLine("\n\nEnter 0 to go back\n input :- ");
            bool verify = true;
            while (verify)
            {
                try
                {
                    selection = Convert.ToInt32(System.Console.ReadLine());
                    verify = false;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("\nOnly numbers allowed!!!! bitch. ");
                    Thread.Sleep(2000);
                }
            }
            if (selection > 0 & selection <= configs.Length)
            {
                var fileStream = File.ReadAllText(configs[selection - 1]);
                Configuration config = JsonConvert.DeserializeObject<Configuration>(fileStream);
                config.urls = System.IO.File.ReadAllLines(config.urlsPath);
                if (config.useProxy == 1)
                {
                    while (true)
                    {
                        try
                        {
                            config.proxys = System.IO.File.ReadAllLines(config.proxyPath);
                            break;
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            Console.WriteLine("Proxy text file not found!!\n\n\tASSHOLE");
                        }
                    }
                }
                else
                {
                    config.proxys = new string[] { "no" };
                }
                try
                {
                    config.suggestionUrls = System.IO.File.ReadAllLines(config.suggestionUrlPath);
                    config.searchKeyWords = System.IO.File.ReadAllLines(config.searchKeyWordsPath);
                    config.UA = System.IO.File.ReadAllLines(Path.Combine(currentPath, "useragents.txt"));
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("\nPath for either of useragent, searchkeywords, suggested url is a zero-length string, contains only white space, or contains one or more invalid characters");
                    Thread.Sleep(2000);
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("\nIncorrect Path for either of useragent, searchkeywords, suggested url. \nCheck the config for paths");
                    Thread.Sleep(2000);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("\nNo file for either of useragent, searchkeywords, suggested url in the directories");
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);

                }
                currentConfig = config;
            }
            else if (selection == 0)
            {
                Main();
            }
            else
            {

                logo();
                Console.WriteLine("Invalid Choice!! nigga");
                Thread.Sleep(1500);
                loadConfig();

            }
        }
        public static bool probBool(int prob)
        {
            Random rand = new Random();
            int chance = rand.Next(1, 101);

            if (chance <= prob)
            {
                return true;
            }
            else
            {
                return false;
            };
        }
        public static int decideplay()
        {
            int chance = rnd.Next(0, 100);
            Console.WriteLine($"chance : {chance}");
            Console.WriteLine($"currentConfig.directplaypercent : {currentConfig.directplaypercent}");
            Console.WriteLine($"currentConfig.googlesearch : {currentConfig.googlesearch}");
            ;
            Console.WriteLine($"currentConfig.suggestedurlplaypercent : {currentConfig.suggestedurlplaypercent}");

            Console.WriteLine($"currentConfig.keywordplaypercent : {currentConfig.keywordplaypercent}");
            if (chance < currentConfig.directplaypercent)
            {
                Console.WriteLine("direct");
                return (0);
            }
            else if ((currentConfig.directplaypercent < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent)))
            {
                Console.WriteLine("google search");
                return (1);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent)))
            {
                Console.WriteLine("keyword");
                return (2);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent)))
            {
                Console.WriteLine("homepage");
                return (3);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent)))
            {
                Console.WriteLine("explore");
                return (4);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent)))
            {
                Console.WriteLine("trending");
                return (5);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent + currentConfig.gamingpageplaypercent)))
            {
                Console.WriteLine("gaming");
                return (6);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent + currentConfig.gamingpageplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent + currentConfig.gamingpageplaypercent + currentConfig.musicpageplaypercent)))
            {
                Console.WriteLine("music");
                return (7);
            }
            else if (((currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent + currentConfig.gamingpageplaypercent + currentConfig.musicpageplaypercent) < chance) & (chance < (currentConfig.googlesearch + currentConfig.directplaypercent + currentConfig.keywordplaypercent + currentConfig.homepageplaypercent + currentConfig.explorepageplaypercent + currentConfig.trendingpageplaypercent + currentConfig.gamingpageplaypercent + currentConfig.musicpageplaypercent + currentConfig.suggestedurlplaypercent)))
            {
                Console.WriteLine("suggested url");
                return (8);
            }
            else
            {
                Console.WriteLine("search");
                return (9);
            }
        }
    }

    class browser
    {
        static Random rnd = new Random();
        IWebDriver driver;
        ChromeOptions options;
        string[] urls;
        string UA;
        public browser()
        {
            options = new ChromeOptions();
            UA = MainClass.currentConfig.UA[Convert.ToInt32(rnd.Next(0, MainClass.currentConfig.UA.Length))];
            urls = MainClass.currentConfig.urls;
            string[] proxy = MainClass.currentConfig.proxys[Convert.ToInt32(rnd.Next(0, MainClass.currentConfig.proxys.Length))].Split(':');
            String threadId = MainClass.RandomString();
            MainClass.Threadlogger(threadId, "Thread started");
            MainClass.Threadlogger(threadId, "Loading Chrome options");
            options.AddArguments("--disable-notifications");
            options.AddArguments("--no-zygote");
            options.AddArguments("--disable-accelerated-2d-canvas");
            options.AddArguments("--disable-setuid-sandbox");
            options.AddArguments("--no-first-run");
            options.AddArguments("--disable-2d-canvas-clip-aa");
            options.AddArgument($"--user-agent=${UA}");
            options.AddArguments("start-maximized");
            options.AddUserProfilePreference("profile.default_content_setting_values.css", 2);
            options.AddArguments("--disable-gpu");
            options.AddArgument("ignore-certificate-errors");
            options.AddExcludedArguments(new List<string>() { "enable-automation" });
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.BinaryLocation = Path.Combine(MainClass.currentPath, "app", "chrome.exe");
            options.AddArguments($"load-extension={Path.Combine(MainClass.currentPath, "proxy")}");
            if (MainClass.currentConfig.useProxy == 1)
            {
                MainClass.Threadlogger(threadId, "Loading proxy into browser");
                string protocol = "http://";
                if (MainClass.currentConfig.proxyProto == 2) { protocol = "socks5://"; }
                options.AddArguments($"--proxy-server={protocol + proxy[0] + ":" + proxy[1]}");
            }
            MainClass.Threadlogger(threadId, "Starting chrome");
            driver = new ChromeDriver(MainClass.currentPath, options);
            MainClass.Threadlogger(threadId, "Chrome started \nExecuting js to remove detection");
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                string title = (string)js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => false})");
            }
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                {
                    return;
                }
                else
                {
                    MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MainClass.Threadlogger(threadId, ex.ToString());
                MainClass.errorlogg(ex, true);

            }
            if (MainClass.currentConfig.useProxy == 1)
            {
                MainClass.Threadlogger(threadId, "Checking proxy Auth is user pass format");
                if (proxy.Length > 2)
                {
                    MainClass.Threadlogger(threadId, "Authenticating proxies user pass");
                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(@"chrome-extension://cpfadechfgcegcfdgfdegbbojfdgpcab/options.html");
                            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                            wait.Until(c => c.FindElement(By.Id("login"))).Clear();
                            wait.Until(c => c.FindElement(By.Id("login"))).SendKeys(proxy[2]);
                            wait.Until(c => c.FindElement(By.Id("password"))).Clear();
                            wait.Until(c => c.FindElement(By.Id("password"))).SendKeys(proxy[3]);
                            wait.Until(c => c.FindElement(By.Id("retry"))).Clear();
                            wait.Until(c => c.FindElement(By.Id("retry"))).SendKeys("5");
                            wait.Until(c => c.FindElement(By.Id("save"))).Click();
                            MainClass.Threadlogger(threadId, "user pass authenticated");
                            break;
                        }
                        catch (OpenQA.Selenium.NoSuchWindowException ex)
                        {
                            if (ex.Message.ToLowerInvariant().Contains("no such window: target window already closed"))
                            {
                                return;
                            }
                            else
                            {
                                MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                            }
                        }
                        catch (OpenQA.Selenium.WebDriverException ex)
                        {
                            if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                            {
                                return;
                            }
                            else
                            {
                                MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                            continue;
                        }
                    }
                }
            }
            try
            {
                MainClass.Threadlogger(threadId, "checking for Youtube consent ");
                driver.Navigate().GoToUrl("https://www.youtube.com");
                Thread.Sleep(rnd.Next(2000, 5000));
            }
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                {
                    return;
                }
                else
                {
                    MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
            }

            if (!(driver.Url == "https://www.youtube.com/"))
            {
                //consent bypass
                MainClass.Threadlogger(threadId, $"Consent Page detected : {driver.Url}");
                Thread.Sleep(3000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IWebElement agree;
                try
                {
                    MainClass.Threadlogger(threadId, "Trying to grab agree button");
                    agree = wait.Until(_driver =>
                    {

                        try
                        {
                            return _driver.FindElement(By.XPath("//input[@value=\"I agree\"]"));
                        }
                        catch (NoSuchElementException)
                        {
                            return _driver.FindElement(By.XPath("//button[@aria-label=\"Agree to the use of cookies and other data for the purposes described\"]"));
                        }
                    });
                    MainClass.Threadlogger(threadId, "Agree button found");
                    agree.Click();
                    MainClass.Threadlogger(threadId, "Agree button clicked");
                }
                catch (OpenQA.Selenium.WebDriverException ex)
                {
                    if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                    {
                        return;
                    }
                    else
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MainClass.Threadlogger(threadId, ex.ToString());
                    MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                }
            }
            if (MainClass.currentConfig.useAccounts == 1)
            {
                MainClass.Threadlogger(threadId, "Injecting cookies ");
                setCookies(driver);
                MainClass.Threadlogger(threadId, "Cookies Injected");
            }
            Thread.Sleep(2000);
            for (int i = 0; i < MainClass.currentConfig.urls.Length; i++)
            {
                try
                {
                    MainClass.Threadlogger(threadId, $"streaming url postion : {i}\n url:{MainClass.currentConfig.urls[i]}");
                    streaming(driver, MainClass.currentConfig.urls[i], threadId);

                    MainClass.streams++;
                    MainClass.Threadlogger(threadId, $"one view done");
                }
                catch (Exception ex)
                {
                    MainClass.Threadlogger(threadId, ex.ToString());
                    MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());

                }
            }
            driver.Quit();
        }
        public static void streaming(IWebDriver driver, string url, string threadId)
        {
            WebDriverWait wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            MainClass.Threadlogger(threadId, "calculating play type");
            int decide = MainClass.decideplay();
            //direct url
            if (decide == 0)
            {
                MainClass.Threadlogger(threadId, "direct play type");
                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch (OpenQA.Selenium.WebDriverException ex)
                {
                    if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                    {
                        return;
                    }
                    else
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(2000, 4000));

            }
            //google search
            else if (decide == 1)
            {
                MainClass.Threadlogger(threadId, $"google search play type \n redirected to google");
                
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(@"https://www.google.com/");
                        Thread.Sleep(3000);
                        MainClass.Threadlogger(threadId, "searching video url");
                        IWebElement searchtext = wait4.Until(c => c.FindElement(By.XPath("//input[@type=\"text\"]")));
                        searchtext.SendKeys(url);
                        Thread.Sleep(rnd.Next(500, 2000));
                        searchtext.SendKeys(Keys.Enter);
                        MainClass.Threadlogger(threadId, $"search done");

                        break;
                    }
                    
                    catch (WebDriverTimeoutException ex)
                    {
                        MainClass.Threadlogger(threadId, ex.ToString());
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.Threadlogger(threadId, ex.ToString());
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(1000, 2000));
                while (true)
                {
                    try
                    {
                        MainClass.Threadlogger(threadId, "finding video search result");
                        IWebElement searchresult = wait4.Until(c => c.FindElement(By.XPath($"//a[@href=\"{url}\"]")));
                        searchresult.Click();
                        MainClass.Threadlogger(threadId, "video link clicked");
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        MainClass.Threadlogger(threadId, ex.ToString());
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }

            }
            //keyword
            else if (decide == 2)
            {
                MainClass.Threadlogger(threadId, $"Keyword search play");

                while (true)
                {
                    try
                    {
                        MainClass.Threadlogger(threadId, "searching keyword");
                        IWebElement search = wait4.Until(c => c.FindElement(By.XPath("//input[@id=\"search\"]")));
                        search.SendKeys(MainClass.currentConfig.searchKeyWords[Convert.ToInt32(rnd.Next(0, MainClass.currentConfig.searchKeyWords.Length))]);
                        Thread.Sleep(500);
                        search.SendKeys(Keys.Enter);
                        break;
                    }
                    catch (OpenQA.Selenium.ElementNotInteractableException ex)
                    {
                        IJavaScriptExecutor javaScriptExecuto = (IJavaScriptExecutor)driver;
                        javaScriptExecuto.ExecuteScript("document.querySelector(\"#content > div.body.style-scope.ytd-consent-bump-v2-lightbox > div.footer.style-scope.ytd-consent-bump-v2-lightbox > div.buttons.style-scope.ytd-consent-bump-v2-lightbox > ytd-button-renderer:nth-child(2) > a\").click()");
                        MainClass.Threadlogger(threadId, "element wasnt interactable while seachring tried by passing consent popup");
                        continue;
                    }catch (WebDriverTimeoutException ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                string[] urlparts = url.Split('/');
                MainClass.Threadlogger(threadId, $"altering html in browser");
                while (true)
                {
                    try
                    {
                        Thread.Sleep(rnd.Next(2000, 5000));
                        
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint style-scope ytd-video-renderer')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint style-scope ytd-video-renderer')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint style-scope ytd-video-renderer')[0].click();");
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");

            }
            //homepage
            else if (decide == 3)
            {
                MainClass.Threadlogger(threadId, $"Homepage view \n altering html");

                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                while (true)
                {
                    try
                    {
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(rnd.Next(3000,8000));
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");
            }
            //explore
            else if (decide == 4)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                MainClass.Threadlogger(threadId, "explore view \n redirecting to youtube explore page");
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/feed/explore");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "altering html");
                Thread.Sleep(4000);
                while (true)
                {
                    try
                    {
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(400);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");

            }
            //Trending
            else if (decide == 5)
            {
                MainClass.Threadlogger(threadId, "Trending view\nNavigate to Trending Page");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "Altering html");
                while (true)
                {
                    try
                    {
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");
            }
            //gaming
            else if (decide == 6)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                MainClass.Threadlogger(threadId, "Gaming View\n redirect to gaming page");
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/gaming");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "altering html");
                while (true)
                {
                    try
                    {
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");

                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked  ");
            }
            //music
            else if (decide == 7)
            {
                MainClass.Threadlogger(threadId, "Music Page view\n redirect to explore page");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;

                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/feed/explore/");
                        wait4.Until(c => c.FindElement(By.XPath("//a[@id=\"destination-content-root\"][@class=\"yt-simple-endpoint style-scope ytd-destination-button-renderer\"]")));
                        driver.FindElements(By.XPath("//a[@id=\"destination-content-root\"][@class=\"yt-simple-endpoint style-scope ytd-destination-button-renderer\"]"))[1].Click();

                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "redirected to music page\n altering html");
                Thread.Sleep(5000);
                while (true)
                {
                    try
                    {
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");
            }
            //suggested
            else if (decide == 8)
            {
                MainClass.Threadlogger(threadId, "suggested view\n redirected to suggested video url");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(MainClass.currentConfig.suggestionUrls[rnd.Next(0, MainClass.currentConfig.suggestionUrls.Length - 1)]);
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(3000,6000));
                MainClass.Threadlogger(threadId, "editng html");
                while (true)
                {
                    try
                    {
                        string tit = (string)javaScriptExecutor.ExecuteScript("document.querySelector(\"#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button\").click()");
                        string[] urlparts = url.Split('/');
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].href = '/" + urlparts[urlparts.Length - 1] + "';");
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].setAttribute('onclick', `window.location.assign('/" + urlparts[urlparts.Length - 1] + "')`);");
                        Thread.Sleep(1000);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        Thread.Sleep(500);
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(5000);
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");
            }
            //search
            else if (decide == 9)
            {
                MainClass.Threadlogger(threadId, "search view\n searching video url");
                while (true)
                {
                    try
                    {
                        IWebElement searchtext = wait4.Until(c => c.FindElement(By.XPath("//input[@id=\"search\"]")));
                        searchtext.SendKeys(url);
                        Thread.Sleep(500);
                        searchtext.SendKeys(Keys.Enter);
                        break;
                    }
                    catch (ElementNotInteractableException)
                    {
                        driver.FindElements(By.CssSelector(".yt-simple-endpoint.style-scope.ytd-button-renderer"))[2].Click();
                        continue;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if(ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }

                while (true)
                {
                    try
                    {
                        Thread.Sleep(5000);
                        IWebElement search = wait4.Until(c => c.FindElement(By.XPath("//*[@id=\"contents\"]/ytd-video-renderer[1]")));
                        search.Click();
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                    catch (OpenQA.Selenium.WebDriverException ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                        {
                            return;
                        }
                        else
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                    }
                }
                MainClass.Threadlogger(threadId, "video clicked");
            }
            MainClass.Threadlogger(threadId, "checking if browser url is video url");
            while (true)
            {
                if (!(driver.Url == url))
                {
                    MainClass.Threadlogger(threadId, $"different url detected\ndriver current:- {driver.Url}");
                    driver.Navigate().GoToUrl(url);
                }
                else
                {
                    break;
                }
            }
            Thread.Sleep(rnd.Next(5000, 9000));
            advertdetect(driver, threadId);
            MainClass.Threadlogger(threadId, "advert skipped");
            MainClass.Threadlogger(threadId, "checking for likeing video");
            if (MainClass.probBool(MainClass.currentConfig.likeChances) & MainClass.currentConfig.useAccounts == 1)
            {
                likeVideo(driver);
            }
            MainClass.Threadlogger(threadId, "checking for subscribing video");
            if (MainClass.probBool(MainClass.currentConfig.subscribeChances) & MainClass.currentConfig.useAccounts == 1)
            {
                subscribeVideo(driver);
            }
            double totaltimesec;
            double currenttimesec;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {

                js.ExecuteScript("document.querySelector(\"#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button\").click()");

            }
            catch (OpenQA.Selenium.WebDriverException)
            {
                IWebElement p = driver.FindElement(By.XPath("//p"));
                if (p.Text == "Sorry for the interruption. We have been receiving a large volume of requests from your network.") { }
                driver.Quit();
                return;
            }
            var totaltime = wait4.Until(c => c.FindElement(By.ClassName("ytp-time-duration")));
            totaltimesec = TimeSpan.Parse("00:" + totaltime.Text).TotalSeconds;
            var currenttime = wait4.Until(c => c.FindElement(By.ClassName("ytp-time-current")));
            currenttimesec = TimeSpan.Parse("00:" + currenttime.Text).TotalSeconds;
            js.ExecuteScript("document.querySelector(\"#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button\").click()");
            MainClass.Threadlogger(threadId, $"total video duration : {totaltimesec}\ncurrent playtime : {currenttimesec}");
            if (Convert.ToInt32(currenttimesec) == 0)
            {
                driver.Navigate().Refresh();
            };
            int playTime = rnd.Next(MainClass.currentConfig.minPlaytime, MainClass.currentConfig.maxPlaytime);
            if (playTime < currenttimesec)
            {
                return;

            }
            else
            {
                MainClass.Threadlogger(threadId, $"thread slept for : {Convert.ToInt32(playTime - currenttimesec)}");
                Thread.Sleep(Convert.ToInt32(playTime - currenttimesec) * 1000);
                return;
            }
        }
        public static void advertdetect(IWebDriver driver, string threadId)
        {
            MainClass.Threadlogger(threadId, "Detecting Advertisement");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            bool check = true;
            int adscount = 1;
            while (adscount > 0)
            {
                MainClass.Threadlogger(threadId, $"Detecting Advertisement Try : {adscount}");
                if (!check)
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                }
                IWebElement checkAdNumber = null;
                try
                {
                    checkAdNumber = wait.Until(c =>
                    {
                        IWebElement res = null;
                        try
                        {
                            res = c.FindElement(By.XPath("//*[@class=\"ytp-ad-player-overlay-instream-info\"]"));
                        }
                        catch (NoSuchElementException)
                        {
                            adscount--;
                            return null;
                        }
                        catch (OpenQA.Selenium.WebDriverException ex)
                        {
                            if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                            {
                                return null;
                            }
                            else
                            {
                                MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                            }
                        }
                        return res;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    MainClass.Threadlogger(threadId, "none Advertisement detected (timed out detecting advert)");
                    return;
                }
                catch (OpenQA.Selenium.WebDriverException ex)
                {
                    if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                    {
                        return;
                    }

                }
                if (checkAdNumber == null)
                {
                    MainClass.Threadlogger(threadId, "none Advertisement detected( no such element found exception)");
                    return;
                }
                try
                {
                    MainClass.Threadlogger(threadId, $"Advert Detected");
                    IWebElement elementAdNumber = wait.Until(c => c.FindElement(By.ClassName("ytp-ad-simple-ad-badge")));
                    elementAdNumber = elementAdNumber.FindElement(By.TagName("div"));
                    string text = elementAdNumber.Text;
                    if (text.Split()[text.Split().Length - 2] == "2" && check)
                    {
                        MainClass.Threadlogger(threadId, "2 ads detected");
                        adscount = 2;
                    }
                    else if (text.Split()[text.Split().Length - 2] == "3" && check)
                    {
                        MainClass.Threadlogger(threadId, "3 ads detected");
                        adscount = 3;
                    }
                    else { MainClass.Threadlogger(threadId, "one advert detected"); }
                    check = false;
                    IWebElement skipbuttondetails = wait.Until(c =>
                    {
                        return c.FindElement(By.CssSelector(".ytp-ad-text.ytp-ad-preview-text"));
                    });
                    string skipbuttontxt = skipbuttondetails.Text;
                    Console.WriteLine(skipbuttontxt);
                    if (skipbuttontxt == $@"Video will play
after ad")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        MainClass.Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0" + totaltime.Text).TotalSeconds) * 1000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Video will play
after ads")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        MainClass.Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Console.WriteLine(totaltime.Text);
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0" + totaltime.Text).TotalSeconds) * 1000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 1")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 2")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(3000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 3")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(4000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 4")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 5")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 8")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(8000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 7")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(7000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 6")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 5")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 4")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 3")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 2")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2500);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 1")
                    {
                        MainClass.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(1000);
                        return;
                    }
                    else
                    {
                        try
                        {
                            MainClass.Threadlogger(threadId, $"Skip button says from else \"{skipbuttontxt}\"");
                            Thread.Sleep(Convert.ToInt32(skipbuttontxt) * 1000);
                        }
                        catch (Exception ex)
                        {
                            MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());
                            
                        }
                    }
                    try
                    {
                        //click skip Ad button
                        MainClass.Threadlogger(threadId, $"grabbing skip Ad button");
                        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        IWebElement skipAd = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-skip-button.ytp-button")));
                        skipAd.Click();
                        MainClass.Threadlogger(threadId, "skip ad clicked");
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (OpenQA.Selenium.WebDriverTimeoutException)
                    {
                        MainClass.Threadlogger(threadId, "timed out during skip ads click");
                    }
                    catch (Exception ex)
                    {
                        MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());

                    }
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    MainClass.errorlogg(ex, true);MainClass.Threadlogger(threadId, ex.ToString());

                }
                adscount--;
            }
        }
        public static void likeVideo(IWebDriver driver)
        {
            try
            {
                var elements = driver.FindElements(By.XPath("//*[@id=\"top-level-buttons-computed\"]/ytd-toggle-button-renderer[1]/a"));
                if (elements.Count > 0)
                {
                    //do ads automation
                    elements[1].Click();
                }
            }
            catch (Exception ex)
            {
                MainClass.errorlogg(ex, true);
                
            }
        }
        public static void subscribeVideo(IWebDriver driver)
        {
            System.Console.WriteLine("subscribe method");
            try
            {
                System.Console.WriteLine("subscribe method");
                var elements = driver.FindElements(By.XPath("//*[@id=\"subscribe-button\"]/ytd-subscribe-button-renderer/tp-yt-paper-button"));
                if (elements.Count > 0)
                {
                    System.Console.WriteLine("subscribe clicking");
                    //do ads automation
                    elements[1].Click();
                    System.Console.WriteLine("subscribe clicked");
                }
            }
            catch (Exception ex)
            {
                MainClass.errorlogg(ex, true);
            }
        }
        public static void setCookies(IWebDriver driver)
        {
            var fileStream = File.ReadAllText(Path.Combine(MainClass.currentPath, "Accounts") + "cookies.txt");
            List<cookie> cookies = JsonConvert.DeserializeObject<List<cookie>>(fileStream);
            for (int i = 0; i < cookies.Count; i++)
            {
                driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookies[i].name, cookies[i].value));
            }
            driver.Navigate().Refresh();
        }


    }
    class Authentication
    {
        public static int RandomNumber()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int pik = rnd.Next(10, 50);
            return pik;
        }
        public static string HashKeys(int length)
        {
            Random rnd = new Random();
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public Authentication()
        {
            Authenticationhelper.OpenEncryption();
            using (var client = new WebClient())
            {
                try
                {
                    client.Proxy = null;
                    HttpWebRequest.DefaultWebProxy = new WebProxy();
                    client.Headers["User-Agent"] = "AuthGG";
                    var values = new NameValueCollection();
                    int iblis = RandomNumber();
                    string optimization = HashKeys(iblis);
                    string mainfollowing = "293162872263434855" + optimization;
                    values["type"] = "start";
                    values["aid"] = "547962";
                    values["secret"] = "RMmBefxxUFMnHUedChMXObYBPc6LGZmX8Ec";
                    values["random"] = iblis.ToString();
                    values["apikey"] = mainfollowing;
                    var response = client.UploadValues("https://api.auth.gg/version2/api.php", values);
                    var resp = System.Text.Encoding.Default.GetString(response);
                    dynamic json = JsonConvert.DeserializeObject(resp);
                    if ((string)json.Status == "Failed")
                    {
                        Console.WriteLine("API key error!");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    if ((string)json.hashed != Settings.PremiumAPIKey)
                    {
                        Console.WriteLine($"Security error, application has been breacheddd!{(string)json.APIKey}");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    if ((string)json.Status == "Disabled")
                    {
                        Console.WriteLine("The Application is currently not available!");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    if ((string)json.DeveloperMode == "Disabled")
                    {
                        if ((string)json.Version != MainClass.version)
                        {
                            Console.WriteLine($"Update [{MainClass.version}] is available!");
                            Console.ReadLine();
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                        }

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong!" + ex.ToString());

                    Console.ReadLine();
                }
            }
            Authenticationhelper.CloseEncryption();
        }

        public static bool Login(string username, string password)
        {
            using (var client = new WebClient())
            {
                try
                {
                    Authenticationhelper.OpenEncryption();
                    client.Proxy = null;
                    HttpWebRequest.DefaultWebProxy = new WebProxy();
                    client.Headers["User-Agent"] = "AuthGG";
                    var values = new NameValueCollection();
                    int iblis = RandomNumber();
                    string optimization = HashKeys(iblis);
                    string mainfollowing = Settings.PremiumAPIKey + optimization;
                    values["type"] = "login";
                    values["username"] = username;
                    values["password"] = password;
                    values["hwid"] = Settings.HWID();
                    values["aid"] = Settings.AID;
                    values["secret"] = Settings.Secret;
                    values["time"] = Settings.Time();
                    values["random"] = iblis.ToString();
                    values["apikey"] = mainfollowing;
                    var response = client.UploadValues("https://api.auth.gg/version2/api.php", values);
                    var resp = System.Text.Encoding.Default.GetString(response);
                    dynamic json = JsonConvert.DeserializeObject(resp);
                    Authenticationhelper.CloseEncryption();
                    if ((string)json.status == "Failed")
                    {
                        Console.WriteLine("API key error!");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    if ((string)json.hashed != Settings.PremiumAPIKey)
                    {
                        Console.WriteLine("Security error, application has been breached!");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    switch ((string)json.result)
                    {
                        case "success":
                            Console.WriteLine($"Welcome back, {username}!");
                            return true;
                        case "invalid_details":
                            Console.WriteLine("Please check your credentials!");
                            Console.ReadLine();
                            return false;
                        case "invalid_hwid":
                            Console.WriteLine("Invalid HWID, please do not attempt to share accounts!");
                            Console.ReadLine();
                            return false;
                        case "hwid_updated":
                            Console.WriteLine("Your HWID has been updated, restart Client!");
                            Console.ReadLine();
                            return false;
                        case "time_expired":
                            Console.WriteLine("Your subscription has expired!");
                            Console.ReadLine();
                            return false;
                        case "net_error":
                            Console.WriteLine("Something went wrong!");
                            Console.ReadLine();
                            return false;
                        default:
                            Console.WriteLine("Something went wrong!");
                            Console.ReadLine();
                            return false;
                    }
                }
                catch
                {
                    Console.WriteLine("Something went wrong!");
                    Console.ReadLine();
                    return false;
                }
            }
        }
        internal class Authenticationhelper
        {
            public static void OpenEncryption()
            {
                ServicePointManager.ServerCertificateValidationCallback += PinPublicKey;
            }
            public static void CloseEncryption()
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            private static bool PinPublicKey(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return certificate != null && certificate.GetPublicKeyString() == _key;
            }
            private const string _key = "04E32E295F50051CD5A5AF5B9B19DFAAB514806DDDEEAEBB38AFCC8AB7D9F1BE5C8E7A782E377DC198E62A1D091A2ADD63F4AC0A320BC4341AD980E34B47C08DB6";
        }

    }
}