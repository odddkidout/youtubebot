using System;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
namespace Youtubebot
{
    
    class MainClass
    {
        static bool isAuthenticated = false;
        public static string version = "1.0";
        public static int streams = 0;
        static DateTime start;
        public static Configuration currentConfig;
        public static Mutex errorMutex = new Mutex();
        public static Random rnd = new Random();
        static public string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static Threadpool Threadpool;

        static void Main()
        {
            /*if (!isAuthenticated)
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
            }*/
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
                Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                new Thread(Threadpool.threadManager).Start();
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Clear();
                Main();
            }
            catch (Exception ex)
            {
                Helper.errorlogger(ex, true);
                Console.Clear();
                Main();
            }

            /*while (true)
            {
                logo();
                var diff = DateTime.Now - start;
                Console.WriteLine($"Running from : {diff.Hours}");
                Console.WriteLine($"streams Completed : {streams} ");
                Console.WriteLine($"threads : {activeThreadsCount()}/{smartThreadPool.ActiveThreads} ");
                Console.WriteLine($"Threads Capped at : {currentConfig.maxThreads} ");
                Console.WriteLine($"Streams in 24 hours : {(streams / diff.TotalSeconds) * 3600 * 24} ");
                Thread.Sleep(1000);
            }*/
        }
        static void logo()
        {
            Console.Clear();
            ASCII.ASCIII();
            Console.WriteLine("");
        }
        public static void initialise()
        {
            Threadpool = new Threadpool();
            start = DateTime.Now;
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                        Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                        Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
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
                            Helper.errorlogger(ex, true);
                        }
                        verify = false;
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogger(ex, true);
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
                    Helper.errorlogger(ex, true);

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
        public static int decideplay()
        {
            int chance = rnd.Next(0, 100);
            Console.WriteLine($"chance : {chance}");
            Console.WriteLine($"currentConfig.directplaypercent : {currentConfig.directplaypercent}");
            Console.WriteLine($"currentConfig.googlesearch : {currentConfig.googlesearch}");
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

   
}