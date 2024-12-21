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
    public partial class NurseVIew : Form
    {
        string userID = null;
        public NurseVIew(string userID)
        {
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
            this.userID = userID;
        }

        private void thôngTinCáNhânToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Information information = new Information(userID);
            CommonControls.AddFormToTab(information, information.Text);
        }

        private void btnWeeklyAssignment_Click(object sender, EventArgs e)
        {
            StaffAssignment staffAssignment = new StaffAssignment(userID);
            CommonControls.AddFormToTab(staffAssignment, staffAssignment.Text);
        }

        private void btnPatientCare_Click(object sender, EventArgs e)
        {
            PatientCare patientCare = new PatientCare(userID);
            CommonControls.AddFormToTab(patientCare, patientCare.Text);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void đổiMặtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(userID);
            changePassword.ShowDialog();
        }
    }
}
