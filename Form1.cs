using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using GUIchartBot.Services;
using GUIchartBot.Models;

namespace GUIchartBot
{
    public partial class Form1 : Form
    {
        private string userName = "";
        private bool isNameSet = false;
        private string favouriteTopic = "";
        private string lastTopic = "";
        private Random random = new Random();

        private Dictionary<string, List<string>> KeywordResponse;
        private List<string> worriedWords;

        // NEW: Task 1 + 2 + 4 services
        private ChatbotEngine engine = new ChatbotEngine();
        private object tabMain;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            KeywordResponse = new Dictionary<string, List<string>>
            {
                { "hello", new List<string> { "Hello! How can I assist you today?", "Hi there! What would you like to talk about?" } },
                { "password", new List<string> { "Always use strong, unique passwords with at least 12 characters.", "Consider using a password manager." } },
                { "email", new List<string> { "Be cautious of phishing emails.", "Use two-factor authentication for email." } },
                { "social media", new List<string> { "Be mindful of what you share online.", "Regularly review your privacy settings." } },
                { "cybersecurity", new List<string> { "Keep your software up to date.", "Use antivirus and firewalls." } },
                { "privacy", new List<string> { "Be aware of the data you share online.", "Review app permissions." } },
                { "phishing", new List<string> { "Be cautious of phishing attempts.", "Verify sender email before clicking." } },
                { "safe browsing", new List<string> { "Use secure and trusted websites.", "Avoid downloading unknown files." } }
            };
            worriedWords = new List<string> { "worried", "anxious", "nervous", "concerned" };
        }

        public void Response(string userInput)
        {
            string lowerInput = userInput.ToLower();
            string response = "";

            if (lowerInput.Contains("my favourite topic is") || lowerInput.Contains("my favorite topic is") || lowerInput.Contains("favourite topic is"))
            {
                foreach (var key in KeywordResponse.Keys)
                {
                    if (lowerInput.Contains(key))
                    {
                        favouriteTopic = key;
                        lastTopic = key;
                        response = $"Great, {userName}! I'll remember that your favourite topic is {favouriteTopic}.";
                        textBox1.AppendText("Bot: " + response + Environment.NewLine);
                        return;
                    }
                }
            }

            if (lowerInput.Contains("what is my favourite") || lowerInput.Contains("my favourite topic"))
            {
                if (!string.IsNullOrEmpty(favouriteTopic))
                    response = $"Your favourite topic is {favouriteTopic}, {userName}. Would you like some tips on it?";
                else
                    response = $"You haven't told me your favourite topic yet, {userName}. Tell me by saying 'my favourite topic is...'";
                textBox1.AppendText("Bot: " + response + Environment.NewLine);
                return;
            }

            if (lowerInput.Contains("what was the last topic") || lowerInput.Contains("last topic we discussed"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    response = $"The last topic we discussed was {lastTopic}, {userName}.";
                else
                    response = $"We haven't discussed any topic yet, {userName}.";
                textBox1.AppendText("Bot: " + response + Environment.NewLine);
                return;
            }

            foreach (var keyword in KeywordResponse.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    var responses = KeywordResponse[keyword];
                    response = responses[random.Next(responses.Count)];
                    lastTopic = keyword;
                    if (favouriteTopic == keyword)
                    {
                        response += $" I know this is your favourite topic, {userName}!";
                    }
                    textBox1.AppendText("Bot: " + response + Environment.NewLine);
                    return;
                }
            }

            foreach (var worriedWord in worriedWords)
            {
                if (lowerInput.Contains(worriedWord))
                {
                    response = $"It's okay to feel {worriedWord}, {userName}. Take deep breaths. Do you want to talk about {lastTopic} or {favouriteTopic}?";
                    textBox1.AppendText("Bot: " + response + Environment.NewLine);
                    return;
                }
            }

            response = $"I'm not sure about that, {userName}. You can ask about your favourite topic ({favouriteTopic}) or the last topic ({lastTopic}).";
            textBox1.AppendText("Bot: " + response + Environment.NewLine);
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            textBox1.AppendText("Bot: What should we call you? " + Environment.NewLine);
            Display_Logo();
            PlayGreeting();

            // NEW: TabControl
            tabChat.SelectedTab = tabChart;
            ActivityLogger.Log("App opened");
            RefreshTaskList();
            LoadActivityLog();
        }

        public void Display_Logo()
        {
            pictureBox1.Image = Image.FromFile("C:\\Users\\Student\\source\\repos\\GUIchartBot\\Resourse\\LogoBG.png");
        }

        public void PlayGreeting()
        {
            SoundPlayer player = new SoundPlayer("C:\\Users\\Student\\source\\repos\\GUIChartBot\\Resourse\\chartbot.wav");
            player.Play();
        }

        public void pictureBox1_Click(object sender, EventArgs e) { }
        public void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile("C:\\Users\\Student\\source\\repos\\GUIchartBot\\Resourse\\session.png");
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        public void button1_Click(object sender, EventArgs e)
        {
            PlayGreeting();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            string userInput = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            if (!isNameSet)
            {
                userName = userInput;
                isNameSet = true;
                textBox1.AppendText($"{userName}: {userInput}{Environment.NewLine}");
                textBox1.AppendText($"Bot: Nice to meet you, {userName}! What is your favourite topic? e.g. password, privacy, phishing{Environment.NewLine}");
                ActivityLogger.Log("User set name: " + userName);
                textBox2.Clear();
                return;
            }

            textBox1.AppendText($"{userName}: {userInput}{Environment.NewLine}");
            string lower = userInput.ToLower();

            // --- QUIZ: if quiz is active, answer goes to quiz ---
            if (engine.Quiz.Active)
            {
                CheckQuizAnswer(userInput);
                return;
            }
            if (lower.Contains("play quiz") || lower.Contains("start quiz"))
            {
                StartQuiz();
                textBox2.Clear();
                return;
            }

            // --- TASK: awaiting reminder answer ---
            if (engine.AwaitingReminder)
            {
                textBox1.AppendText("Bot: " + engine.HandleTaskReminder(userInput) + Environment.NewLine);
                RefreshTaskList();
                textBox2.Clear();
                return;
            }

            // --- TASK: add task ---
            if (lower.StartsWith("add task"))
            {
                string title = userInput.Replace("Add task", "", StringComparison.OrdinalIgnoreCase).Replace("-", "").Trim();
                engine.PendingTask = new TaskItem { Title = title, Description = TaskDescriptions.GetDescription(title) };
                textBox1.AppendText($"Bot: Task added with the description \"{engine.PendingTask.Description}\" Would you like a reminder?" + Environment.NewLine);
                engine.AwaitingReminder = true;
                textBox2.Clear();
                return;
            }
            if (lower.Contains("show tasks") || lower.Contains("view tasks"))
            {
                RefreshTaskList();
                textBox1.AppendText("Bot: Here are your tasks from the database:" + Environment.NewLine);
                foreach (var item in listBoxTasks.Items) textBox1.AppendText("- " + item + Environment.NewLine);
                textBox2.Clear();
                return;
            }
            if (lower.Contains("delete task"))
            {
                DeleteSelectedTask();
                textBox2.Clear();
                return;
            }
            if (lower.Contains("complete task"))
            {
                CompleteSelectedTask();
                textBox2.Clear();
                return;
            }

            // --- Normal Part 1 chatbot ---
            Response(userInput);
            ActivityLogger.Log("User asked: " + userInput);
            textBox2.Clear();
        }

        public void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            ActivityLogger.Log("Chat cleared");
        }

        // ===== TABCONTROL - Step 9 names =====
        private void tabChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabChat.SelectedTab == tabTasks)
            {
                RefreshTaskList();
                ActivityLogger.Log("User opened Task Assistant tab");
            }
            else if (tabChat.SelectedTab == tabQuiz)
            {
                ActivityLogger.Log("User opened Quiz tab");
            }
            else if (tabChat.SelectedTab == tabLog)
            {
                LoadActivityLog();
                ActivityLogger.Log("User viewed Activity Log tab");
            }
            else if (tabChat.SelectedTab == tabChart)
            {
                ActivityLogger.Log("User opened Chat tab");
            }
        }

        private void tabLog_Click(object sender, EventArgs e)
        {
            LoadActivityLog();
        }

        // ===== TASKS =====
        private void RefreshTaskList()
        {
            if (listBoxTasks == null) return;
            listBoxTasks.Items.Clear();
            try
            {
                foreach (var t in engine.Repo.GetAll())
                {
                    string s = t.IsCompleted ? "[DONE] " : "[ ] ";
                    listBoxTasks.Items.Add(t.Id + ". " + s + t.Title + " - " + (t.ReminderDate == null ? "No reminder" : "Remind: " + t.ReminderDate.Value.ToString("dd MMM HH:mm")));
                }
            }
            catch { listBoxTasks.Items.Add("Start XAMPP MySQL to load tasks"); }
        }

        private void DeleteSelectedTask()
        {
            if (listBoxTasks.SelectedIndex < 0) { textBox1.AppendText("Bot: Select a task in Task Assistant tab first." + Environment.NewLine); return; }
            int id = int.Parse(listBoxTasks.SelectedItem.ToString().Split('.')[0]);
            engine.Repo.Delete(id);
            RefreshTaskList();
            textBox1.AppendText("Bot: Task deleted from the database." + Environment.NewLine);
        }

        private void CompleteSelectedTask()
        {
            if (listBoxTasks.SelectedIndex < 0) { textBox1.AppendText("Bot: Select a task in Task Assistant tab first." + Environment.NewLine); return; }
            int id = int.Parse(listBoxTasks.SelectedItem.ToString().Split('.')[0]);
            engine.Repo.Complete(id);
            RefreshTaskList();
            textBox1.AppendText("Bot: Task marked as completed in the database." + Environment.NewLine);
        }

        // Add Task button in tabTasks - button4
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text)) return;
            string title = textBox2.Text.Trim();
            engine.PendingTask = new TaskItem { Title = title, Description = TaskDescriptions.GetDescription(title) };
            textBox1.AppendText($"Bot: Task added with the description \"{engine.PendingTask.Description}\" Would you like a reminder?" + Environment.NewLine);
            engine.AwaitingReminder = true;
            tabChat.SelectedTab = tabChart;
            textBox2.Clear();
        }

        // ===== QUIZ =====
        private void button5_Click(object sender, EventArgs e)
        {
            tabChat.SelectedTab = tabChart;
            StartQuiz();
        }

        private void StartQuiz()
        {
            engine.Quiz.Active = true;
            engine.Quiz.Current = 0;
            engine.Quiz.Score = 0;
            labelScore.Text = "Score: 0 / " + engine.Quiz.Questions.Count;
            textBox1.AppendText("Bot: Let's play! Answer with A, B, C, D or True/False." + Environment.NewLine);
            ActivityLogger.Log("Quiz started");
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var q = engine.Quiz.Questions[engine.Quiz.Current];
            textBox1.AppendText(Environment.NewLine + "Question " + (engine.Quiz.Current + 1) + ": " + q.Question + Environment.NewLine);
            foreach (var opt in q.Options) textBox1.AppendText(opt + Environment.NewLine);
            textBox1.AppendText("Your answer: " + Environment.NewLine);
        }

        private void CheckQuizAnswer(string input)
        {
            var q = engine.Quiz.Questions[engine.Quiz.Current];
            string ans = input.Trim().ToLower();
            if (ans.StartsWith("a")) ans = "a";
            if (ans.StartsWith("b")) ans = "b";
            if (ans.StartsWith("c")) ans = "c";
            if (ans.StartsWith("d")) ans = "d";

            bool correct = ans.Equals(q.CorrectAnswer.ToLower());
            if (correct)
            {
                engine.Quiz.Score++;
                textBox1.AppendText("Chatbot: Correct! " + q.Explanation + Environment.NewLine);
            }
            else
            {
                textBox1.AppendText("Chatbot: Wrong. Correct answer was " + q.CorrectAnswer + ". " + q.Explanation + Environment.NewLine);
            }
            labelScore.Text = "Score: " + engine.Quiz.Score + " / " + engine.Quiz.Questions.Count;
            engine.Quiz.Current++;

            if (engine.Quiz.Current >= engine.Quiz.Questions.Count)
            {
                engine.Quiz.Active = false;
                string feedback = engine.Quiz.Score >= 9 ? "Great job! You're a cybersecurity pro!" : engine.Quiz.Score >= 6 ? "Good try! Keep learning to stay safe online!" : "Keep learning to stay safe online!";
                textBox1.AppendText(Environment.NewLine + "Bot: Quiz finished! Final score: " + engine.Quiz.Score + " / " + engine.Quiz.Questions.Count + ". " + feedback + Environment.NewLine);
                ActivityLogger.Log("Quiz finished. Score: " + engine.Quiz.Score);
            }
            else ShowQuestion();
            textBox2.Clear();
        }

        // ===== ACTIVITY LOG =====
        private void LoadActivityLog()
        {
            if (listBoxLog == null) return;
            listBoxLog.Items.Clear();
            if (File.Exists("activityLog.txt"))
            {
                foreach (var line in File.ReadAllLines("activityLog.txt"))
                    listBoxLog.Items.Add(line);
            }
            else listBoxLog.Items.Add("No activity yet.");
        }

        private void tabChart_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
