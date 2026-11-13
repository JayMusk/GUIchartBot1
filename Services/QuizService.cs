using System.Collections.Generic;

namespace GUIchartBot.Services
{
    // One question
    public class QuizQuestion
    {
        public string Question;
        public string[] Options;
        public string CorrectAnswer; // "A", "B", "C", "D", "True", "False"
        public string Explanation;
    }

    public class QuizService
    {
        public List<QuizQuestion> Questions = new List<QuizQuestion>();

        // These 3 are used by Form1.cs - DO NOT rename them
        public int Current = 0;
        public int Score = 0;
        public bool Active = false;

        public QuizService()
        {
            // 1 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new[] { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Correct! Reporting phishing emails helps prevent scams."
            });

            // 2 - True / False
            Questions.Add(new QuizQuestion
            {
                Question = "True or False: You should use the same password for all accounts.",
                Options = new[] { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "False! Use a different strong password for each account."
            });

            // 3 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "Which is the strongest password?",
                Options = new[] { "A) password123", "B) John2000", "C) P@ssw0rd!9xQ", "D) 123456" },
                CorrectAnswer = "C",
                Explanation = "Correct! Long passwords with symbols, numbers and capitals are strongest."
            });

            // 4 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Options = new[] { "A) A type of fishing", "B) Fake emails/messages to steal your info", "C) A virus", "D) A firewall" },
                CorrectAnswer = "B",
                Explanation = "Correct! Phishing tricks you into giving away personal info."
            });

            // 5 - True / False
            Questions.Add(new QuizQuestion
            {
                Question = "True or False: Two-factor authentication (2FA) makes your account safer.",
                Options = new[] { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "True! 2FA adds a second check, like a code on your phone."
            });

            // 6 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "What should you check before entering card details on a website?",
                Options = new[] { "A) It has https and a padlock", "B) It has lots of adverts", "C) It looks colourful", "D) Nothing" },
                CorrectAnswer = "A",
                Explanation = "Correct! HTTPS and the padlock mean the connection is encrypted."
            });

            // 7 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "A stranger calls asking for your OTP code. What do you do?",
                Options = new[] { "A) Give it to them", "B) Never share it, hang up", "C) Ask them to wait", "D) Share only half" },
                CorrectAnswer = "B",
                Explanation = "Correct! Banks will NEVER ask for your OTP. This is social engineering."
            });

            // 8 - True / False
            Questions.Add(new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi is safe for online banking.",
                Options = new[] { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "False! Hackers can steal data on public Wi-Fi. Use mobile data or a VPN."
            });

            // 9 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "What is social engineering?",
                Options = new[] { "A) Building robots", "B) Tricking people to reveal info", "C) Fixing computers", "D) Making friends online" },
                CorrectAnswer = "B",
                Explanation = "Correct! Attackers manipulate people, not just computers."
            });

            // 10 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "How often should you update your software?",
                Options = new[] { "A) Never", "B) Only when it breaks", "C) Regularly, when updates come out", "D) Once a year" },
                CorrectAnswer = "C",
                Explanation = "Correct! Updates fix security holes hackers use."
            });

            // 11 - True / False
            Questions.Add(new QuizQuestion
            {
                Question = "True or False: Clicking links in unknown SMS messages is safe.",
                Options = new[] { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "False! Those links can install malware or steal your info."
            });

            // 12 - Multiple Choice
            Questions.Add(new QuizQuestion
            {
                Question = "What do you do if your account is hacked?",
                Options = new[] { "A) Do nothing", "B) Change password, enable 2FA and report it", "C) Delete the app", "D) Tell your friends only" },
                CorrectAnswer = "B",
                Explanation = "Correct! Act fast: change password, turn on 2FA, and report it."
            });
        }
    }
}
