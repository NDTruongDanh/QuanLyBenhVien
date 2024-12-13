using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                "Only answer questions about medication and answer in Vietnamese. If there are any questions unrelated to medication, say 'Xin lỗi. Tôi Không thể giúp bạn câu hỏi này" +
                "'";


            chatHistory.Add(new ChatMessage(ChatRole.System, systemMessage));
        }

        private string FormatResponse(string responseText)
        {
            // Handle line breaks and ensure formatting appears correctly in the TextBox
            return responseText.Replace("\n", Environment.NewLine);
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                txtResponse.Text = "Generating...";
                var userPrompt = txtPrompt.Text;
                btnGenerate.Enabled = false;
                chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

                var response = await client.CompleteAsync(chatHistory);
                chatHistory.Add(new ChatMessage(ChatRole.Assistant, response.ToString()));
                txtResponse.Text = FormatResponse(response.ToString());
                txtPrompt.Clear();
                btnGenerate.Enabled = true;
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
    }
}
