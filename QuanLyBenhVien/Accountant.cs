using QuanLyBenhVien.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class Accountant : Form
    {
        string userID = null;
        public Accountant()
        {
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
        }
        public Accountant(string userID)
        {
            this.userID = userID;
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
            this.WindowState = FormWindowState.Maximized;
        }
        private void thôngTinCáNhânToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Information information = new Information(userID);
            CommonControls.AddFormToTab(information, information.Text);
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            DoanhthuTHANG dtt = new DoanhthuTHANG();
            CommonControls.AddFormToTab(dtt, dtt.Text);
        }

        private void btnYear_Click(object sender, EventArgs e)
        {
            DoanhthuNAM dtn = new DoanhthuNAM();
            CommonControls.AddFormToTab(dtn, dtn.Text);
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            AccountantBill accountantBill = new AccountantBill();
            CommonControls.AddFormToTab(accountantBill, accountantBill.Text);
        }

        private void đổiMặtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(userID);
            changePassword.ShowDialog();   
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trợLýẢoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatBotForm chatBotForm = new ChatBotForm();
            CommonControls.AddFormToTab(chatBotForm, chatBotForm.Text);
        }
    }
}
