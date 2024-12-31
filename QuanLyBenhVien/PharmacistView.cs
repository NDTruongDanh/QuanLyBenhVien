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
    public partial class PharmacistView : Form
    {
        string userID = null;
        bool isDepartment = false;
        public PharmacistView()
        {
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
        }

        public PharmacistView(string userID, bool isDepartment)
        {
            this.userID = userID;
            this.isDepartment = isDepartment;
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
            this.WindowState = FormWindowState.Maximized;

        }


        private void thôngTinCáNhânToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Information information = new Information(userID);
            CommonControls.AddFormToTab(information, information.Text);
        }

        private void txtMedicine_Click(object sender, EventArgs e)
        {
            if (isDepartment)
            {
                MedicineForm medicineForm = new MedicineForm();
                CommonControls.AddFormToTab(medicineForm, medicineForm.Text);
            }

            else
            {
                NormalMedicine normalizationForm = new NormalMedicine();
                CommonControls.AddFormToTab(normalizationForm, normalizationForm.Text);
            }
        }

        private void lịchTrựcToolStripMenuItem_Click(object sender, EventArgs e)
        {
           StaffAssignment staffAssignment = new StaffAssignment(userID);
            CommonControls.AddFormToTab(staffAssignment, staffAssignment.Text);
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

        private void btnDeleteRemember_Click(object sender, EventArgs e)
        {
            CommonControls.DisableRememberMe(userID);
        }
    }
}
