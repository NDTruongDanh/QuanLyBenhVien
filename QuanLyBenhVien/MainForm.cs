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
    public partial class MainForm : Form
    {
        private ContextMenuStrip contextMenu; 

        public MainForm()
        {
            InitializeComponent();
            InitializeTabControl();
        }

        private void InitializeTabControl()
        {
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem closeTabMenuItem = new ToolStripMenuItem("Close Tab");
            closeTabMenuItem.Click += CloseTabMenuItem_Click;
            contextMenu.Items.Add(closeTabMenuItem);
            tabControl.MouseUp += TabControl_MouseUp;
        }

        private void AddFormToTab(Form childForm, string tabName)
        {
            foreach (TabPage existingTab in tabControl.TabPages)
            {
                if (existingTab.Text == tabName)
                {
                    tabControl.SelectedTab = existingTab;
                    return;
                }
            }
            TabPage tabPage = new TabPage(tabName);
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            tabPage.Controls.Add(childForm);
            tabControl.TabPages.Add(tabPage);
            childForm.Size = tabControl.Size;
            childForm.Show();
            tabControl.SelectedTab = tabPage;
        }

        private void TabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Identify the tab under the mouse pointer
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl.SelectedIndex = i; // Select the tab
                        contextMenu.Show(tabControl, e.Location); // Show the context menu
                        break;
                    }
                }
            }
        }

        private void CloseTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab); // Close the selected tab
            }
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            AddFormToTab(patientForm, patientForm.Text);
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            AddFormToTab(employeeForm, employeeForm.Text);
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            RoomForm roomForm = new RoomForm();
            AddFormToTab(roomForm, roomForm.Text);
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            MedicineForm medicineForm = new MedicineForm();
            AddFormToTab(medicineForm, medicineForm.Text);
        }

        private void btnMedicineStat_Click(object sender, EventArgs e)
        {
            MedStatForm medStatForm = new MedStatForm();
            AddFormToTab(medStatForm, medStatForm.Text);
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            DepartmentForm departmentForm = new DepartmentForm();
            AddFormToTab(departmentForm, departmentForm.Text);
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            BillForm billForm = new BillForm();
            AddFormToTab(billForm, billForm.Text);
        }

        private void btnMedicalRecord_Click(object sender, EventArgs e)
        {
            MedicalRecord medicalRecord = new MedicalRecord();
            AddFormToTab(medicalRecord, medicalRecord.Text);
        }

        private void btnMonthIncomeReport_Click(object sender, EventArgs e)
        {
            DoanhthuTHANG dtt = new DoanhthuTHANG();
            dtt.Show();
        }

        private void btnYearIncomeReport_Click(object sender, EventArgs e)
        {
            DoanhthuNAM dtn = new DoanhthuNAM();
            dtn.Show();
        }

        private void btnAIChatBot_Click(object sender, EventArgs e)
        {
            ChatBotForm chatBotForm = new ChatBotForm();
            AddFormToTab(chatBotForm, chatBotForm.Text);
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment();
            AddFormToTab(appointment, appointment.Text);
        }

        private void btnQuantityInStock_Click(object sender, EventArgs e)
        {
            KiemtraThuocTonKho ktt = new KiemtraThuocTonKho();
            ktt.Show();
        }

        private void btnExpired_Click(object sender, EventArgs e)
        {
            KiemtraHANTHUOC ktht = new KiemtraHANTHUOC();
            ktht.Show();
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            EpidemicSituation epidemic = new EpidemicSituation();
            epidemic.Show();
        }
    }
}
