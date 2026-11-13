using System.Media;
using System.Windows.Forms;

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

            // 1. USE favouriteTopic - Set it
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

            // 2. USE favouriteTopic - Recall it
            if (lowerInput.Contains("what is my favourite") || lowerInput.Contains("my favourite topic"))
            {
                if (!string.IsNullOrEmpty(favouriteTopic))
                    response = $"Your favourite topic is {favouriteTopic}, {userName}. Would you like some tips on it?";
                else
                    response = $"You haven't told me your favourite topic yet, {userName}. Tell me by saying 'my favourite topic is...'";

                textBox1.AppendText("Bot: " + response + Environment.NewLine);
                return;
            }

            // 3. USE lastTopic - Recall it
            if (lowerInput.Contains("what was the last topic") || lowerInput.Contains("last topic we discussed"))
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    response = $"The last topic we discussed was {lastTopic}, {userName}.";
                else
                    response = $"We haven't discussed any topic yet, {userName}.";

                textBox1.AppendText("Bot: " + response + Environment.NewLine);
                return;
            }

            // 4. Normal keyword handling - UPDATE lastTopic
            foreach (var keyword in KeywordResponse.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    var responses = KeywordResponse[keyword];
                    response = responses[random.Next(responses.Count)];
                    lastTopic = keyword; // <-- using lastTopic attribute

                    // Personalize with favouriteTopic
                    if (favouriteTopic == keyword)
                    {
                        response += $" I know this is your favourite topic, {userName}!";
                    }

                    textBox1.AppendText("Bot: " + response + Environment.NewLine);
                    return;
                }
            }

            // Worried words
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

        public void pictureBox1_Click(object sender, EventArgs e)
        {

        }

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
                textBox2.Clear();
                return;
            }

            textBox1.AppendText($"{userName}: {userInput}{Environment.NewLine}");
            Response(userInput);
            textBox2.Clear();

        }

        public void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        public void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile("C:\\Users\\Student\\source\\repos\\GUIchartBot\\Resourse\\session.png");
        }
    }
}
