using System;
using System.Collections.Generic;
using System.Drawing;

namespace Youtubebot
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
    
}