using  System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Youtubebot
{
    class Browser
    {
        static Random rnd = new Random();
        IWebDriver driver;
        ChromeOptions options;
        string[] urls;
        string UA;
        public Browser()
        {
            options = new ChromeOptions();
            UA = MainClass.currentConfig.UA[Convert.ToInt32(rnd.Next(0, MainClass.currentConfig.UA.Length))];
            urls = MainClass.currentConfig.urls;
            string[] proxy = MainClass.currentConfig.proxys[Convert.ToInt32(rnd.Next(0, MainClass.currentConfig.proxys.Length))].Split(':');
            String threadId = Helper.RandomString();
            Helper.Threadlogger(threadId, "Thread started");
            Helper.Threadlogger(threadId, "Loading Chrome options");
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
            /*options.AddArguments($"load-extension={Path.Combine(MainClass.currentPath, "proxy")}");
            */
            if (MainClass.currentConfig.useProxy == 1)
            {
                Helper.Threadlogger(threadId, "Loading proxy into browser");
                string protocol = "http://";
                if (MainClass.currentConfig.proxyProto == 2) { protocol = "socks5://"; }
                options.AddArguments($"--proxy-server={protocol + proxy[0] + ":" + proxy[1]}");
            }
            Helper.Threadlogger(threadId, "Starting chrome");
            driver = new ChromeDriver(MainClass.currentPath, options);
            Helper.Threadlogger(threadId, "Chrome started \nExecuting js to remove detection");
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
                    Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Helper.Threadlogger(threadId, ex.ToString());
                Helper.errorlogg(ex, true);

            }
            if (MainClass.currentConfig.useProxy == 1)
            {
                Helper.Threadlogger(threadId, "Checking proxy Auth is user pass format");
                if (proxy.Length > 2)
                {
                    Helper.Threadlogger(threadId, "Authenticating proxies user pass");
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
                            Helper.Threadlogger(threadId, "user pass authenticated");
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
                                Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                                Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                            continue;
                        }
                    }
                }
            }
            try
            {
                Helper.Threadlogger(threadId, "checking for Youtube consent ");
                driver.Navigate().GoToUrl("https://www.gmail.com");
                Thread.Sleep(1000000000);
                
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
                    Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
            }
            Thread.Sleep(3000);
            IJavaScriptExecutor js2 = (IJavaScriptExecutor) driver;
            js2.ExecuteScript(
                "document.querySelector(\"#content > div.body.style-scope.ytd-consent-bump-v2-lightbox > div.footer.style-scope.ytd-consent-bump-v2-lightbox > div.buttons.style-scope.ytd-consent-bump-v2-lightbox > ytd-button-renderer:nth-child(2) > a\").click()");
            if (!(driver.Url == "https://www.youtube.com/"))
            {
                //consent bypass
                Helper.Threadlogger(threadId, $"Consent Page detected : {driver.Url}");
                Thread.Sleep(3000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IWebElement agree;
                try
                {
                    Helper.Threadlogger(threadId, "Trying to grab agree button");
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
                    Helper.Threadlogger(threadId, "Agree button found");
                    agree.Click();
                    Helper.Threadlogger(threadId, "Agree button clicked");
                }
                catch (OpenQA.Selenium.WebDriverException ex)
                {
                    if (ex.Message.ToLowerInvariant().Contains("chrome not reachable"))
                    {
                        return;
                    }
                    else
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Helper.Threadlogger(threadId, ex.ToString());
                    Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                }
            }
            if (MainClass.currentConfig.useAccounts == 1)
            {
                Helper.Threadlogger(threadId, "Injecting cookies ");
                setCookies(driver);
                Helper.Threadlogger(threadId, "Cookies Injected");
            }
            Thread.Sleep(2000);
            for (int i = 0; i < MainClass.currentConfig.urls.Length; i++)
            {
                try
                {
                    Helper.Threadlogger(threadId, $"streaming url postion : {i}\n url:{MainClass.currentConfig.urls[i]}");
                    streaming(driver, MainClass.currentConfig.urls[i], threadId);
                    MainClass.streams++;
                    Helper.Threadlogger(threadId, $"one view done");
                }
                catch (Exception ex)
                {
                    Helper.Threadlogger(threadId, ex.ToString());
                    Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());

                }
            }
            driver.Quit();
        }
        public static void streaming(IWebDriver driver, string url, string threadId)
        {
            WebDriverWait wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Helper.Threadlogger(threadId, "calculating play type");
            int decide = MainClass.decideplay();
            //direct url
            if (decide == 0)
            {
                Helper.Threadlogger(threadId, "direct play type");
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
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(2000, 4000));

            }
            //google search
            else if (decide == 1)
            {
                Helper.Threadlogger(threadId, $"google search play type \n redirected to google");
                
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(@"https://www.google.com/");
                        Thread.Sleep(3000);
                        Helper.Threadlogger(threadId, "searching video url");
                        IWebElement searchtext = wait4.Until(c => c.FindElement(By.XPath("//input[@type=\"text\"]")));
                        searchtext.SendKeys(url);
                        Thread.Sleep(rnd.Next(500, 2000));
                        searchtext.SendKeys(Keys.Enter);
                        Helper.Threadlogger(threadId, $"search done");

                        break;
                    }
                    
                    catch (WebDriverTimeoutException ex)
                    {
                        Helper.Threadlogger(threadId, ex.ToString());
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.Threadlogger(threadId, ex.ToString());
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(1000, 2000));
                while (true)
                {
                    try
                    {
                        Helper.Threadlogger(threadId, "finding video search result");
                        IWebElement searchresult = wait4.Until(c => c.FindElement(By.XPath($"//a[@href=\"{url}\"]")));
                        searchresult.Click();
                        Helper.Threadlogger(threadId, "video link clicked");
                        break;
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        Helper.Threadlogger(threadId, ex.ToString());
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }

            }
            //keyword
            else if (decide == 2)
            {
                Helper.Threadlogger(threadId, $"Keyword search play");

                while (true)
                {
                    try
                    {
                        Helper.Threadlogger(threadId, "searching keyword");
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
                        Helper.Threadlogger(threadId, "element wasnt interactable while seachring tried by passing consent popup");
                        continue;
                    }catch (WebDriverTimeoutException ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                string[] urlparts = url.Split('/');
                Helper.Threadlogger(threadId, $"altering html in browser");
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
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");

            }
            //homepage
            else if (decide == 3)
            {
                Helper.Threadlogger(threadId, $"Homepage view \n altering html");

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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");
            }
            //explore
            else if (decide == 4)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                Helper.Threadlogger(threadId, "explore view \n redirecting to youtube explore page");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "altering html");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");

            }
            //Trending
            else if (decide == 5)
            {
                Helper.Threadlogger(threadId, "Trending view\nNavigate to Trending Page");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "Altering html");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");
            }
            //gaming
            else if (decide == 6)
            {
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
                Helper.Threadlogger(threadId, "Gaming View\n redirect to gaming page");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "altering html");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked  ");
            }
            //music
            else if (decide == 7)
            {
                Helper.Threadlogger(threadId, "Music Page view\n redirect to explore page");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "redirected to music page\n altering html");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");
            }
            //suggested
            else if (decide == 8)
            {
                Helper.Threadlogger(threadId, "suggested view\n redirected to suggested video url");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Thread.Sleep(rnd.Next(3000,6000));
                Helper.Threadlogger(threadId, "editng html");
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        continue;
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");
            }
            //search
            else if (decide == 9)
            {
                Helper.Threadlogger(threadId, "search view\n searching video url");
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
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
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
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                    }
                }
                Helper.Threadlogger(threadId, "video clicked");
            }
            Helper.Threadlogger(threadId, "checking if browser url is video url");
            while (true)
            {
                if (!(driver.Url == url))
                {
                    Helper.Threadlogger(threadId, $"different url detected\ndriver current:- {driver.Url}");
                    driver.Navigate().GoToUrl(url);
                }
                else
                {
                    break;
                }
            }
            Thread.Sleep(rnd.Next(5000, 9000));
            advertdetect(driver, threadId);
            Helper.Threadlogger(threadId, "advert skipped");
            Helper.Threadlogger(threadId, "checking for likeing video");
            if (Helper.probBool(MainClass.currentConfig.likeChances) & MainClass.currentConfig.useAccounts == 1)
            {
                likeVideo(driver);
            }
            Helper.Threadlogger(threadId, "checking for subscribing video");
            if (Helper.probBool(MainClass.currentConfig.subscribeChances) & MainClass.currentConfig.useAccounts == 1)
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
            Helper.Threadlogger(threadId, $"total video duration : {totaltimesec}\ncurrent playtime : {currenttimesec}");
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
                Helper.Threadlogger(threadId, $"thread slept for : {Convert.ToInt32(playTime - currenttimesec)}");
                Thread.Sleep(Convert.ToInt32(playTime - currenttimesec) * 1000);
                return;
            }
        }
        public static void advertdetect(IWebDriver driver, string threadId)
        {
            Helper.Threadlogger(threadId, "Detecting Advertisement");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            bool check = true;
            int adscount = 1;
            while (adscount > 0)
            {
                Helper.Threadlogger(threadId, $"Detecting Advertisement Try : {adscount}");
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
                                Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                            }
                        }
                        return res;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    Helper.Threadlogger(threadId, "none Advertisement detected (timed out detecting advert)");
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
                    Helper.Threadlogger(threadId, "none Advertisement detected( no such element found exception)");
                    return;
                }
                try
                {
                    Helper.Threadlogger(threadId, $"Advert Detected");
                    IWebElement elementAdNumber = wait.Until(c => c.FindElement(By.ClassName("ytp-ad-simple-ad-badge")));
                    elementAdNumber = elementAdNumber.FindElement(By.TagName("div"));
                    string text = elementAdNumber.Text;
                    if (text.Split()[text.Split().Length - 2] == "2" && check)
                    {
                        Helper.Threadlogger(threadId, "2 ads detected");
                        adscount = 2;
                    }
                    else if (text.Split()[text.Split().Length - 2] == "3" && check)
                    {
                        Helper.Threadlogger(threadId, "3 ads detected");
                        adscount = 3;
                    }
                    else { Helper.Threadlogger(threadId, "one advert detected"); }
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
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        Helper.Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0" + totaltime.Text).TotalSeconds) * 1000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Video will play
after ads")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"");
                        var totaltime = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-duration-remaining")));
                        totaltime = totaltime.FindElement(By.TagName("div"));
                        Helper.Threadlogger(threadId, $"time left for advert {totaltime}\n Thread gone on sleep till advert ends");
                        Console.WriteLine(totaltime.Text);
                        Thread.Sleep(Convert.ToInt32(TimeSpan.Parse("00:0" + totaltime.Text).TotalSeconds) * 1000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 1")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 2")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(3000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 3")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(4000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 4")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Ad will end
in 5")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        adscount--;
                        continue;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 8")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(8000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 7")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(7000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 6")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(6000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 5")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 4")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 3")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(5000);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 2")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(2500);
                        return;
                    }
                    else if (skipbuttontxt == $@"Your video will
begin in 1")
                    {
                        Helper.Threadlogger(threadId, $"Skip button says \"{skipbuttontxt}\"\n thread gone into sleep till advert end");
                        Thread.Sleep(1000);
                        return;
                    }
                    else
                    {
                        try
                        {
                            Helper.Threadlogger(threadId, $"Skip button says from else \"{skipbuttontxt}\"");
                            Thread.Sleep(Convert.ToInt32(skipbuttontxt) * 1000);
                        }
                        catch (Exception ex)
                        {
                            Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());
                            
                        }
                    }
                    try
                    {
                        //click skip Ad button
                        Helper.Threadlogger(threadId, $"grabbing skip Ad button");
                        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        IWebElement skipAd = wait.Until(c => c.FindElement(By.CssSelector(".ytp-ad-skip-button.ytp-button")));
                        skipAd.Click();
                        Helper.Threadlogger(threadId, "skip ad clicked");
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (OpenQA.Selenium.WebDriverTimeoutException)
                    {
                        Helper.Threadlogger(threadId, "timed out during skip ads click");
                    }
                    catch (Exception ex)
                    {
                        Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());

                    }
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    Helper.errorlogg(ex, true);Helper.Threadlogger(threadId, ex.ToString());

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
                Helper.errorlogg(ex, true);
                
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
                Helper.errorlogg(ex, true);
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
    
}