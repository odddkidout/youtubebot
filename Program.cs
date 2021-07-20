using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Amib.Threading;
namespace maincore
{
    [Serializable]
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
        static int streams = 0;
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
                Console.WriteLine($"Streams in 24 hours : {(streams/diff.TotalSeconds)*3600*24} ");
                Thread.Sleep(1000);

            }
        }
        static void errorlogger(Exception ex, bool console)
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
                    tw.WriteLine(ex+"\n\n");
                }
                
            }
            finally
            {
                errorMutex.ReleaseMutex();
            }

        }
        static void Threadlogger(string Threadid,String update)
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
               //     thread thread = new thread(() => run(convert.toint32(rnd.next(currentconfig.minplaytime, currentconfig.maxplaytime))
               //, probbool(currentconfig.likechances)
               //, probbool(currentconfig.subscribechances)
               //, probbool(currentconfig.subscribechances)
               //, currentconfig.urls[convert.toint32(rnd.next(0, currentconfig.urls.length))]
               //, currentconfig.proxys[convert.toint32(rnd.next(0, currentconfig.proxys.length))]
               //, currentconfig.ua[convert.toint32(rnd.next(0, currentconfig.ua.length))]
               //));
                    //thread.Start();
                    //threads.Add(thread);
                    Thread.Sleep(currentConfig.delayThreads * 1000);
                }
                catch (Exception ex)
                {
                    errorlogger(ex, true);
                }
            }
        }
        static void run()
        {
            String threadId = RandomString();
            Threadlogger(threadId, "Thread started");
            ChromeOptions options = new ChromeOptions();
            IWebDriver driver;
            Threadlogger(threadId, "Loading Chrome options");
            options.AddArguments("--disable-notifications");
            options.AddArguments("--no-zygote");
            options.AddArguments("--disable-accelerated-2d-canvas");
            options.AddArguments("--disable-setuid-sandbox");
            options.AddArguments("--no-first-run");
            options.AddArguments("--disable-2d-canvas-clip-aa");
            options.AddArgument($"--user-agent=${currentConfig.UA[Convert.ToInt32(rnd.Next(0, currentConfig.UA.Length))]}");
            options.AddArguments("start-maximized");
            //options.AddArguments("--blink-settings=imagesEnabled=false");
            //options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.AddUserProfilePreference("profile.default_content_setting_values.css", 2);
            options.AddArguments("--disable-gpu");
            options.AddArgument("ignore-certificate-errors");
            options.AddExcludedArguments(new List<string>() { "enable-automation" });
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.BinaryLocation = Path.Combine(currentPath, "app", "chrome.exe");
            options.AddArguments($"load-extension={Path.Combine(currentPath, "proxy")}");
            string[] proxy = currentConfig.proxys[Convert.ToInt32(rnd.Next(0, currentConfig.proxys.Length))].Split(':');
            if (currentConfig.useProxy == 1)
            {
                Threadlogger(threadId, "Loading proxy into browser");
                string protocol = "http://";
                if (currentConfig.proxyProto == 2) { protocol = "socks5://"; }
                options.AddArguments($"--proxy-server={protocol + proxy[0] + ":" + proxy[1]}");
            }
            Threadlogger(threadId, "Starting chrome");
            driver = new ChromeDriver(currentPath, options);
            Threadlogger(threadId, "Chrome started \nExecuting js to remove detection");
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                string title = (string)js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => false})");
            }
            catch (Exception ex)
            {
                Threadlogger(threadId, ex.ToString());
                errorlogger(ex, true);

            }
            if (currentConfig.useProxy == 1)
            {
                Threadlogger(threadId, "Checking proxy Auth is user pass format");
                if (proxy.Length > 2)
                {
                    Threadlogger(threadId, "Authenticating proxies user pass");
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
                            Threadlogger(threadId, "user pass authenticated");
                            break;
                        }
                        catch (Exception ex)
                        {
                            errorlogger(ex, true);
                            continue;
                        }
                    }
                }
            }
            try
            {
                Threadlogger(threadId, "checking for Youtube consent ");
                driver.Navigate().GoToUrl("https://www.youtube.com");
                Thread.Sleep(rnd.Next(2000, 5000));
            }
            catch (Exception ex)
            {
                errorlogger(ex, true);

            }

            if (!(driver.Url == "https://www.youtube.com/"))
            {
                //consent bypass
                Threadlogger(threadId, $"Consent Page detected : {driver.Url}");
                Thread.Sleep(3000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IWebElement agree;
                try
                {
                    Threadlogger(threadId, "Trying to grab agree button");
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
                    Threadlogger(threadId, "Agree button found");
                    agree.Click();
                    Threadlogger(threadId, "Agree button clicked");
                }
                catch (Exception ex)
                {
                    Threadlogger(threadId,ex.ToString());
                    errorlogger(ex, true);
                }
            }
            if (currentConfig.useAccounts == 1)
            {
                Threadlogger(threadId, "Injecting cookies ");
                setCookies(driver);
                Threadlogger(threadId, "Cookies Injected");
            }
            Thread.Sleep(2000);
            for (int i = 0; i < currentConfig.urls.Length; i++)
            {
                try
                {
                    Threadlogger(threadId, $"streaming url postion : {i}\n url:{currentConfig.urls[i]}");
                    streaming(driver,currentConfig.urls[i],threadId);
                    
                    streams++;
                    Threadlogger(threadId, $"one view done");
                }
                catch (Exception ex)
                {
                    Threadlogger(threadId, ex.ToString());
                    errorlogger(ex, true);

                }
            }
            driver.Quit();
        }
        public static void streaming(IWebDriver driver, string url ,string threadId)
        {
            WebDriverWait wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait4.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            Threadlogger(threadId, "calculating play type");
            int decide = decideplay();
            //direct url
            if (decide == 0)
            {
                Threadlogger(threadId, "direct play type");
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(rnd.Next(2000, 4000));

            }
            //google search
            else if (decide == 1)
            {
                Threadlogger(threadId, $"google search play type \n redirected to google");
                driver.Navigate().GoToUrl(@"https://www.google.com/");
                Thread.Sleep(3000);
                while (true)
                {
                    try
                    {
                        Threadlogger(threadId, "searching video url");
                        IWebElement searchtext = wait4.Until(c => c.FindElement(By.XPath("//input[@type=\"text\"]")));
                        searchtext.SendKeys(url);
                        Thread.Sleep(rnd.Next(500, 2000));
                        searchtext.SendKeys(Keys.Enter);
                        Threadlogger(threadId, $"search done");

                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        Threadlogger(threadId, ex.ToString());
                        errorlogger( ex, true);
                        continue;
                    }
                    catch (WebDriverException ex)
                    {
                        Threadlogger(threadId, ex.ToString());
                        errorlogger( ex, true);
                        driver.Quit();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Threadlogger(threadId, ex.ToString());
                        errorlogger( ex, true);
                    }
                }
                Thread.Sleep(rnd.Next(1000, 2000));
                while (true)
                {
                    try
                    {
                        Threadlogger(threadId,"finding video search result");
                        IWebElement searchresult = wait4.Until(c => c.FindElement(By.XPath($"//a[@href=\"{url}\"]")));
                        searchresult.Click();
                        Threadlogger(threadId, "video link clicked");
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        Threadlogger(threadId, ex.ToString());  
                        errorlogger(ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);
                    }
                }

            }
            //keyword
            else if (decide == 2)
            {
                Threadlogger(threadId, $"Keyword search play");

                while (true)
                {
                    try
                    {
                        Threadlogger(threadId, "searching keyword");
                        IWebElement search = wait4.Until(c => c.FindElement(By.XPath("//input[@id=\"search\"]")));
                        search.SendKeys(currentConfig.searchKeyWords[Convert.ToInt32(rnd.Next(0, currentConfig.searchKeyWords.Length))]);
                        Thread.Sleep(500);
                        search.SendKeys(Keys.Enter);
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        errorlogger(ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                string[] urlparts = url.Split('/');
                Threadlogger(threadId, $"altering html in browser");

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
                        errorlogger( ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "video clicked");

            }
            //homepage
            else if (decide == 3)
            {
                Threadlogger(threadId, $"Homepage view \n altering html");

                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
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
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId, "video clicked");
            }
            //explore
            else if (decide == 4)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                Threadlogger(threadId,"explore view \n redirecting to youtube explore page");
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/feed/explore");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "altering html");
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
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId, "video clicked");

            }
            //Trending
            else if (decide == 5)
            {
                Threadlogger(threadId, "Trending view\nNavigate to Trending Page");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "Altering html");
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
                        errorlogger(ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId, "video clicked");
            }
            //gaming
            else if (decide == 6)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                Threadlogger(threadId,"Gaming View\n redirect to gaming page");
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://www.youtube.com/gaming");
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "altering html");
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
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId, "video clicked  ");
            }
            //music
            else if (decide == 7)
            {
                Threadlogger(threadId, "Music Page view\n redirect to explore page");
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
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "redirected to music page\n altering html");
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
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId,"video clicked");
            }
            //suggested
            else if (decide == 8)
            {
                Threadlogger(threadId, "suggested view\n redirected to suggested video url");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(currentConfig.suggestionUrls[rnd.Next(0, currentConfig.suggestionUrls.Length - 1)]);
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Thread.Sleep(6000);
                Threadlogger(threadId, "editng html");
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
                        Thread.Sleep(500);
                        javaScriptExecutor.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint inline-block style-scope ytd-thumbnail')[0].click();");
                        Thread.Sleep(500);
                        break;
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        Thread.Sleep(6000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                }
                Threadlogger(threadId, "video clicked");
            }
            //search
            else if (decide == 9)
            {
                Threadlogger(threadId, "search view\n searching video url");
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
                    }catch (WebDriverTimeoutException ex)
                    {
                        errorlogger( ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
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
                        errorlogger( ex, true);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        errorlogger( ex, true);
                    }
                }
                Threadlogger(threadId, "video clicked");
            }
            Threadlogger(threadId, "checking if browser url is video url");
            if (!(driver.Url == url))
            {
                Threadlogger(threadId,$"different url detected\ndriver current:- {driver.Url}");
                driver.Navigate().GoToUrl(url);
            }
            Thread.Sleep(rnd.Next(5000, 9000));
            advertdetect(driver,threadId);
            Threadlogger(threadId,"advert skipped");
            Threadlogger(threadId, "checking for likeing video");
            if (probBool(currentConfig.likeChances) & currentConfig.useAccounts == 1)
            {
                likeVideo(driver);
            }
            Threadlogger(threadId, "checking for subscribing video");
            if (probBool(currentConfig.subscribeChances) & currentConfig.useAccounts == 1)
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
                Thread.CurrentThread.Interrupt();
                return;
            }
            var totaltime = wait4.Until(c => c.FindElement(By.ClassName("ytp-time-duration")));
            totaltimesec = TimeSpan.Parse("00:" + totaltime.Text).TotalSeconds;
            var currenttime = wait4.Until(c => c.FindElement(By.ClassName("ytp-time-current")));
            currenttimesec = TimeSpan.Parse("00:" + currenttime.Text).TotalSeconds;
            js.ExecuteScript("document.querySelector(\"#movie_player > div.ytp-chrome-bottom > div.ytp-chrome-controls > div.ytp-left-controls > button\").click()");
            Threadlogger(threadId, $"total video duration : {totaltimesec}\ncurrent playtime : {currenttimesec}");
            int playTime = rnd.Next(currentConfig.minPlaytime, currentConfig.maxPlaytime);
            if (playTime < currenttimesec)
            {
                return;

            }
            else
            {
                Threadlogger(threadId, $"thread slept for : {Convert.ToInt32(playTime - currenttimesec)}");
                Thread.Sleep(Convert.ToInt32(playTime - currenttimesec) * 1000);
                return;
            }
        }
        public static void setCookies(IWebDriver driver)
        {
            var fileStream = File.ReadAllText(Path.Combine(currentPath, "Accounts") + "cookies.txt");
            List<cookie> cookies = JsonConvert.DeserializeObject<List<cookie>>(fileStream);
            for (int i = 0; i < cookies.Count; i++)
            {
                driver.Manage().Cookies.AddCookie(new Cookie(cookies[i].name, cookies[i].value));
            }
            driver.Navigate().Refresh();
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
                            errorlogger( ex, true);
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
                            errorlogger( ex, true);
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
                            errorlogger( ex, true);
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
                            errorlogger( ex, true);
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
                        errorlogger( ex, true);
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
        public static void advertdetect(IWebDriver driver, string threadId)
        {
            Threadlogger(threadId, "Detecting Advertisement");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            bool check=true;
            int adscount = 1;
            while(adscount > 0)
            {
                Threadlogger(threadId, $"Detecting Advertisement Try : {adscount}");
                if (!check)
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                }
                IWebElement checkAdNumber;
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
                        return res;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    Threadlogger(threadId, "none Advertisement detected (timed out detecting advert)");
                    return;
                }
                if (checkAdNumber == null)
                {
                    Threadlogger(threadId, "none Advertisement detected( no such element found exception)");
                    return;
                }
                try
                {
                    Threadlogger(threadId, $"Advert Detected");
                    IWebElement elementAdNumber = wait.Until(c => c.FindElement(By.ClassName("ytp-ad-simple-ad-badge")));
                    elementAdNumber = elementAdNumber.FindElement(By.TagName("div"));
                    string text = elementAdNumber.Text;
                    if (text.Split()[text.Split().Length - 2] == "2" && check)
                    {
                        Threadlogger(threadId, "2 ads detected");
                        adscount = 2;
                    }
                    else if (text.Split()[text.Split().Length - 2] == "3" && check)
                    {
                        Threadlogger(threadId, "3 ads detected");
                        adscount = 3;
                    }
                    else { Threadlogger(threadId, "one advert detected"); }
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
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0"+ totaltime.Text).TotalSeconds)*1000);
                        return;
                    }else if (skipbuttontxt == $@"Video will play
after ads")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Console.WriteLine(totaltime.Text); 
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0" + totaltime.Text).TotalSeconds) * 1000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 1")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 2")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(3000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 3")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(4000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 4")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        adscount--;
                        continue;
                    }else if (skipbuttontxt == $@"Ad will end
in 5")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 8")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(8000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 7")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(7000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 6")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 5")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 4")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 3")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 2")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2500);
                        return;
                    }else if (skipbuttontxt == $@"Your video will
begin in 1")
                    {
                        Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(1000);
                        return;
                    }
                    else
                    {
                        try
                        {
                            Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                            Thread.Sleep(Convert.ToInt32(skipbuttontxt) *1000);
                        }catch (Exception ex)
                        {
                            errorlogger(ex, true);
                            Thread.Sleep(10000);
                        }
                    }
                    try
                    {
                        //click skip Ad button
                        Threadlogger(threadId, $"grabbing skip Ad button");
                        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        IWebElement skipAd = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-skip-button.ytp-button")));
                        skipAd.Click();
                        Threadlogger(threadId,"skip ad clicked");
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (OpenQA.Selenium.WebDriverTimeoutException)
                    {
                        Threadlogger(threadId, "timed out during skip ads click");
                    }
                    catch (Exception ex)
                    {
                        errorlogger(ex, true);

                    }
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    errorlogger( ex, true);

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
                errorlogger( ex, true);
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
                errorlogger( ex, true);
            }
        }


    }
}