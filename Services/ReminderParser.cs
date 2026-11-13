using System;
using System.Text.RegularExpressions;

namespace GUIchartBot.Services
{
    public static class ReminderParser
    {
        // Input: "yes, remind me in 3 days" / "in 2 hours" / "tomorrow" / "in 30 minutes"
        // Output: DateTime or null if no time found
        public static DateTime? Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            string s = input.ToLower();

            // 1. "tomorrow"
            if (s.Contains("tomorrow"))
            {
                return DateTime.Now.AddDays(1);
            }

            // 2. "next week"
            if (s.Contains("next week"))
            {
                return DateTime.Now.AddDays(7);
            }

            // 3. "in 3 days" / "in 1 day"
            var dayMatch = Regex.Match(s, @"in\s+(\d+)\s+day");
            if (dayMatch.Success)
            {
                int days = int.Parse(dayMatch.Groups[1].Value);
                return DateTime.Now.AddDays(days);
            }

            // 4. "in 2 hours" / "in 1 hour"
            var hourMatch = Regex.Match(s, @"in\s+(\d+)\s+hour");
            if (hourMatch.Success)
            {
                int hours = int.Parse(hourMatch.Groups[1].Value);
                return DateTime.Now.AddHours(hours);
            }

            // 5. "in 30 minutes" / "in 10 mins"
            var minMatch = Regex.Match(s, @"in\s+(\d+)\s+min");
            if (minMatch.Success)
            {
                int mins = int.Parse(minMatch.Groups[1].Value);
                return DateTime.Now.AddMinutes(mins);
            }

            // 6. "in 1 week" / "in 2 weeks"
            var weekMatch = Regex.Match(s, @"in\s+(\d+)\s+week");
            if (weekMatch.Success)
            {
                int weeks = int.Parse(weekMatch.Groups[1].Value);
                return DateTime.Now.AddDays(weeks * 7);
            }

            // 7. No time found - let ChatbotEngine use default 3 days
            return null;
        }
    }
}
