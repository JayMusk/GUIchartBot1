using System;
using System.Collections.Generic;

namespace GUIchartBot.Services
{
    // NOTE: Class name = TaskDiscription to match your file name TaskDiscription.cs
    // Do NOT rename it to TaskDescriptions or ChatbotEngine will break
    public static class TaskDiscription
    {
        private static Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            { "2fa", "Turn on two-factor authentication for your account to add an extra layer of security." },
            { "two-factor", "Turn on two-factor authentication for your account to add an extra layer of security." },
            { "password", "Change your password to a strong, unique one. Use letters, numbers and symbols." },
            { "update", "Update your software and apps to fix security holes hackers use." },
            { "phishing", "Learn how to spot phishing emails and report them. Never click unknown links." },
            { "backup", "Back up your important files so you don't lose them if you get hacked." },
            { "antivirus", "Run a full antivirus scan and make sure your antivirus is up to date." },
            { "wifi", "Avoid using public Wi-Fi for banking. Use mobile data or a VPN." },
            { "privacy", "Check your privacy settings on social media and turn off location sharing." },
            { "bank", "Check your bank statements for unknown transactions and report anything suspicious." }
        };

        // Called from ChatbotEngine.cs when you add a task
        public static string GetDescription(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "Stay safe online - complete this cybersecurity task.";

            string lower = title.ToLower();

            foreach (var key in descriptions.Keys)
            {
                if (lower.Contains(key))
                    return descriptions[key];
            }

            return "Complete this cybersecurity task to stay safe online.";
        }

        // Called from Form1.cs - shows suggested tasks in tabTasks
        public static List<string> GetSuggestedTasks()
        {
            return new List<string>()
            {
                "Enable 2FA on email",
                "Update password",
                "Check for phishing emails",
                "Update software",
                "Run antivirus scan",
                "Check privacy settings"
            };
        }
    }
}
