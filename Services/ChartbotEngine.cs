using GUIchartBot.Data;
using GUIchartBot.Models;

namespace GUIchartBot.Services
{
    public class ChatbotEngine
    {
        // Task 1 - Database
        public TaskRepository Repo = new TaskRepository();

        // Part 2 - NLP keyword / sentiment
        public NlpService Nlp = new NlpService();

        // Task 2 - Quiz
        public QuizService Quiz = new QuizService();

        // Task 1 - Reminder flow
        // Form1 sets PendingTask when user says "add task - ..."
        // Then waits for "yes, remind me in 3 days" or "no"
        public bool AwaitingReminder = false;
        public TaskItem PendingTask;

        // Called from Form1 button2_Click when AwaitingReminder == true
        public string HandleTaskReminder(string input)
        {
            string lower = input.ToLower();

            // User said YES - try to parse "in 3 days / in 2 hours / in 30 minutes"
            if (lower.Contains("yes") || lower.Contains("remind"))
            {
                DateTime? rem = ReminderParser.Parse(input);

                // If they just said "yes" with no time, default to 3 days
                if (rem == null) rem = DateTime.Now.AddDays(3);

                PendingTask.ReminderDate = rem;
                Repo.Add(PendingTask);
                AwaitingReminder = false;

                ActivityLogger.Log("Task added: " + PendingTask.Title + " | Reminder: " + rem.Value);
                return "Got it! I'll remind you on " + rem.Value.ToString("dd MMM HH:mm") + ".";
            }

            // User said NO
            PendingTask.ReminderDate = null;
            Repo.Add(PendingTask);
            AwaitingReminder = false;

            ActivityLogger.Log("Task added (no reminder): " + PendingTask.Title);
            return "Okay, task saved with no reminder.";
        }
    }
}
