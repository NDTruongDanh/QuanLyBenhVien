using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.AI; 

namespace QuanLyBenhVien
{
    public partial class ChatBotForm : Form
    {
        IChatClient client =
            new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2");
        List<ChatMessage> chatHistory = new List<ChatMessage>();
        public ChatBotForm()
        {
            InitializeComponent();
            InitializeAI();
        }
        

        private void InitializeAI()
        {
            string systemMessage = "Your name is Tâm and you are a helpful medical assistant with a friendly and approachable personality." +
                "Answer the user's questions and assist them in a clear, friendly manner." +
                "Only answer questions about medical field and answer in Vietnamese. If there are any questions unrelated to medication, say 'Xin lỗi. Tôi Không thể giúp bạn câu hỏi này'";


            chatHistory.Add(new ChatMessage(ChatRole.System, systemMessage));
        }

        private string FormatResponse(string responseText)
        {
    
            return responseText.Replace("\n", Environment.NewLine);
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                btnGenerate.Enabled = false;

                var userPrompt = txtPrompt.Text;
                if (!string.IsNullOrWhiteSpace(userPrompt))
                {
                    AppendFormattedText("User:\n", Color.Blue, FontStyle.Bold);
                    AppendFormattedText(userPrompt + Environment.NewLine, Color.Black, FontStyle.Regular);

                    txtPrompt.Clear();
                    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

                    txtPrompt.ReadOnly = true;
                    WaitingMessage("Chatbot is typing...");

                    var response = await client.CompleteAsync(chatHistory);
                    var chatbotResponse = response.ToString();
                    chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatbotResponse));
                    txtPrompt.Clear();
                    txtPrompt.ReadOnly = false;
                    AppendFormattedText("\nChatbot:\n", Color.Green, FontStyle.Bold);
                    AppendFormattedText(FormatResponse(chatbotResponse) + Environment.NewLine, Color.Black, FontStyle.Regular);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerate.Enabled = true;
            }
        }

        private void WaitingMessage(string message)
        {
            txtPrompt.SelectionStart = txtPrompt.TextLength;
            txtPrompt.SelectionLength = 0;
            txtPrompt.SelectionColor = Color.Gray;
            txtPrompt.SelectionFont = new Font(txtPrompt.Font, FontStyle.Italic);
            txtPrompt.AppendText(message);
            txtPrompt.SelectionColor = txtPrompt.ForeColor;
        }

        private void AppendFormattedText(string text, Color color, FontStyle fontStyle)
        {
            if (txtChat.InvokeRequired)
            {
                txtChat.Invoke(new Action(() => AppendFormattedText(text, color, fontStyle)));
            }
            else
            {
                txtChat.SelectionStart = txtChat.TextLength;
                txtChat.SelectionLength = 0;
                txtChat.SelectionColor = color;
                txtChat.SelectionFont = new Font(txtChat.Font, fontStyle);
                txtChat.AppendText(text);
                txtChat.SelectionColor = txtChat.ForeColor;

                txtChat.ScrollToCaret();
            }
        }

        private void btnDeleteChat_Click(object sender, EventArgs e)
        {
            txtChat.Clear();
            chatHistory.Clear();
            InitializeAI();
        }
    }
}
