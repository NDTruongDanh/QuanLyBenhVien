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
    public partial class DoctorView : Form
    {
        string userID = null;
        public DoctorView()
        {
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
        }
        public DoctorView(string userID)
        {
            InitializeComponent();
            this.userID = userID;
            CommonControls.InitializeTabControl(tabControl);
        }

        private void thôngTinCáNhânToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Information doctorInformation = new Information(userID);
            CommonControls.AddFormToTab(doctorInformation, doctorInformation.Text);
        }

        private void lịchLàmViệcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoctorAppointment doctorAppointment = new DoctorAppointment();
            CommonControls.AddFormToTab(doctorAppointment, doctorAppointment.Text);
        }

        private void btnWeeklyAssignment_Click(object sender, EventArgs e)
        {
            StaffAssignment doctorAssignment = new StaffAssignment(userID);
            CommonControls.AddFormToTab(doctorAssignment, doctorAssignment.Text);
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


        private void btnChatbot_Click(object sender, EventArgs e)

        {
            ChatBotForm chatBotForm = new ChatBotForm();
            CommonControls.AddFormToTab(chatBotForm, chatBotForm.Text);
        }
    }
}
