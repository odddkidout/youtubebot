using System;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace Youtubebot
{
    public class Helper
    {
        public static string RandomString(int length = 32)
        {
            while (true)
            {
                var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[MainClass.rnd.Next(chars.Length)];
                }
                if (!(File.Exists(Path.Combine(MainClass.currentPath, "logs", new String(stringChars)))))
                {
                    return new String(stringChars);
                }
            }

        }
        public static void errorlogger(Exception ex, bool console)
        {
            string path = Path.Combine(MainClass.currentPath, "error.txt");
            if (console)
            {
                Console.Clear();
                Console.BackgroundColor = Color.Blue;
                Console.ForegroundColor = Color.Red;
                Console.WriteLine(ex);
                Console.WriteLine("\tERROR\n\nCheck errors.txt");
                Console.ResetColor();
            }
            MainClass.errorMutex.WaitOne();
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ex + "\n\n");
                }
            }
            finally
            {
                MainClass.errorMutex.ReleaseMutex();
            }

        }
        public static void errorlogg(Exception ex, bool console)
        {
            string path = Path.Combine(MainClass.currentPath, "error.txt");
            if (console)
            {
                Console.Clear();
                Console.BackgroundColor = Color.Blue;
                Console.ForegroundColor = Color.Red;
                Console.WriteLine(ex);
                Console.WriteLine("\tERROR\n\nCheck errors.txt");
                Console.ResetColor();
            }
            MainClass.errorMutex.WaitOne();
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ex + "\n\n");
                }
            }
            finally
            {
                MainClass.errorMutex.ReleaseMutex();
            }

        }
        public static async void Threadlogger(string Threadid, String update)
        {

            string path = Path.Combine(MainClass.currentPath, "logs");
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
                Helper.errorlogger(ex, true);
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

    }
}