using System;
using System.Security.Principal;


namespace Youtubebot
{
    class Settings
    {
        public static string AID = "547962";
        public static string Secret = "RMmBefxxUFMnHUedChMXObYBPc6LGZmX8Ec";
        public static string PremiumAPIKey = "293162872263434855";
        public static string Version = "1.0";
        public static string Hwid()
        {
            return WindowsIdentity.GetCurrent().User.Value;
        }
        public static string Time()
        {
            string time = DateTime.Now.ToString("hh:mm tt MM/dd/yyyy");
            return time;
        }
    }
}
