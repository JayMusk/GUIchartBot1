using System;
using System.Collections.Generic;

namespace GUIchartBot.Services
{
    public class NlpService
    {
        // Keyword -> list of possible answers (same as your Form1 KeywordResponse)
        private Dictionary<string, List<string>> KeywordResponse;
        private List<string> worriedWords;
        private Random random = new Random();

        public NlpService()
        {
            KeywordResponse = new Dictionary<string, List<string>>
            {
                { "hello", new List<string> { "Hello! How can I assist you today?", "Hi there! What would you like to talk about?" } },
                { "password", new List<string> { "Always use strong, unique passwords with at least 12 characters.", "Consider using a password manager." } },
                { "email", new List<string> { "Be cautious of phishing emails.", "Use two-factor authentication for email." } },
                { "social media", new List<string> { "Be mindful of what you share online.", "Regularly review your privacy settings." } },
                { "cybersecurity", new List<string> { "Keep your software up to date.", "Use antivirus and firewalls." } },
                { "privacy", new List<string> { "Be aware of the data you share online.", "Review app permissions." } },
                { "phishing", new List<string> { "Be cautious of phishing attempts. Verify sender email before clicking any link.", "Phishing is fake emails/messages trying to steal your info. Never click, report it as phishing." } },
                { "safe browsing", new List<string> { "Use secure and trusted websites.", "Avoid downloading unknown files." } },
                { "scam", new List<string> { "If it sounds too good to be true, it is a scam. Don't share OTPs or passwords.", "Never share your OTP or password, even if they say they are from the bank." } },
                { "hacked", new List<string> { "If you think you were hacked, change your password now, turn on 2FA, and report it.", "Don't panic. Change password, enable 2FA, and check your account activity." } }
            };

            worriedWords = new List<string> { "worried", "anxious", "nervous", "concerned", "scared", "stressed" };
        }

        // Called by ChatbotEngine / Form1: engine.Nlp.GetResponse(input, userName)
        public string GetResponse(string input, string userName = "", string favouriteTopic = "", string lastTopic = "")
        {
            string s = input.ToLower();

            // 1. SENTIMENT - worried words come FIRST, before keywords
            foreach (var w in worriedWords)
            {
                if (s.Contains(w))
                {
                    string topic = !string.IsNullOrEmpty(lastTopic) ? lastTopic : favouriteTopic;
                    if (string.IsNullOrEmpty(topic)) topic = "cybersecurity";
                    return $"It's okay to feel {w}, {userName}. Take deep breaths. Do you want to talk about {topic}? I can help you step by step.";
                }
            }

            // 2. FAVOURITE TOPIC - recall
            if (s.Contains("what is my favourite") || s.Contains("my favourite topic"))
            {
                if (!string.IsNullOrEmpty(favouriteTopic))
                    return $"Your favourite topic is {favouriteTopic}, {userName}. Would you like some tips on it?";
                return $"You haven't told me your favourite topic yet, {userName}. Tell me by saying 'my favourite topic is...'";
            }

            // 3. LAST TOPIC - recall
            if (s.Contains("what was the last topic") || s.Contains("last topic we discussed"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    return $"The last topic we discussed was {lastTopic}, {userName}.";
                return $"We haven't discussed any topic yet, {userName}.";
            }

            // 4. KEYWORD matching - pick a random answer for that keyword
            foreach (var keyword in KeywordResponse.Keys)
            {
                if (s.Contains(keyword))
                {
                    var responses = KeywordResponse[keyword];
                    string response = responses[random.Next(responses.Count)];
                    if (favouriteTopic == keyword)
                        response += $" I know this is your favourite topic, {userName}!";
                    return response;
                }
            }

            // 5. Small talk
            if (s.Contains("hello") || s.Contains("hi")) return $"Hello {userName}! How can I help you stay safe online today?";
            if (s.Contains("thank")) return "You're welcome! Stay safe online.";
            if (s.Contains("bye")) return $"Goodbye {userName}! Stay safe online.";

            // 6. Fallback
            return $"I'm not sure about that, {userName}. You can ask about password, phishing, privacy, or type 'play quiz' or 'add task'.";
        }

        // Helper for your report - Part 2 sentiment detection
        public string DetectSentiment(string input)
        {
            string s = input.ToLower();
            foreach (var w in worriedWords)
            {
                if (s.Contains(w)) return "Worried";
            }
            if (s.Contains("thank") || s.Contains("great") || s.Contains("awesome")) return "Positive";
            return "Neutral";
        }

        // Helper - what keyword did we find? Used to update lastTopic in Form1
        public string FindKeyword(string input)
        {
            string s = input.ToLower();
            foreach (var keyword in KeywordResponse.Keys)
            {
                if (s.Contains(keyword)) return keyword;
            }
            return "";
        }
    }
}
