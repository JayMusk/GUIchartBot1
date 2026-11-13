using System;
using System.IO;

namespace GUIchartBot.Services
{
    public static class ActivityLogger
    {
        private static string path = "activityLog.txt";
        private static object lockObj = new object();

        public static void Log(string action)
        {
            try
            {
                lock (lockObj)
                {
                    string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + action;
                    File.AppendAllText(path, line + Environment.NewLine);
                }
            }
            catch
            {
            }
        }
    }
