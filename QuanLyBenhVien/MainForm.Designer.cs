namespace QuanLyBenhVien
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCatalogue = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPatient = new System.Windows.Forms.ToolStripMenuItem();
            this.hồSơBệnhÁnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEmployee = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicine = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManager = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedBill = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBill = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStatistic = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicineStat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPatientStat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicineReport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnIncomeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.khoaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFunction,
            this.btnCatalogue,
            this.btnManager,
            this.btnStatistic});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnFunction
            // 
            this.btnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLogout,
            this.btnExit});
            this.btnFunction.Name = "btnFunction";
            this.btnFunction.Size = new System.Drawing.Size(77, 20);
            this.btnFunction.Text = "Chức năng";
            // 
            // btnLogout
            // 
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(180, 22);
            this.btnLogout.Text = "Đăng xuất";
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "Thoát";
            // 
            // btnCatalogue
            // 
            this.btnCatalogue.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPatient,
            this.btnEmployee,
            this.btnRoom,
            this.khoaToolStripMenuItem,
            this.btnMedicine});
            this.btnCatalogue.Name = "btnCatalogue";
            this.btnCatalogue.Size = new System.Drawing.Size(74, 20);
            this.btnCatalogue.Text = "Danh mục";
            // 
            // btnPatient
            // 
            this.btnPatient.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hồSơBệnhÁnToolStripMenuItem});
            this.btnPatient.Name = "btnPatient";
            this.btnPatient.Size = new System.Drawing.Size(180, 22);
            this.btnPatient.Text = "Bệnh nhân";
            this.btnPatient.Click += new System.EventHandler(this.btnPatient_Click);
            // 
            // hồSơBệnhÁnToolStripMenuItem
            // 
            this.hồSơBệnhÁnToolStripMenuItem.Name = "hồSơBệnhÁnToolStripMenuItem";
            this.hồSơBệnhÁnToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hồSơBệnhÁnToolStripMenuItem.Text = "Hồ sơ bệnh án";
            // 
            // btnEmployee
            // 
            this.btnEmployee.Name = "btnEmployee";
            this.btnEmployee.Size = new System.Drawing.Size(180, 22);
            this.btnEmployee.Text = "Nhân viên";
            this.btnEmployee.Click += new System.EventHandler(this.btnEmployee_Click);
            // 
            // btnRoom
            // 
            this.btnRoom.Name = "btnRoom";
            this.btnRoom.Size = new System.Drawing.Size(180, 22);
            this.btnRoom.Text = "Phòng bệnh";
            this.btnRoom.Click += new System.EventHandler(this.btnRoom_Click);
            // 
            // btnMedicine
            // 
            this.btnMedicine.Name = "btnMedicine";
            this.btnMedicine.Size = new System.Drawing.Size(180, 22);
            this.btnMedicine.Text = "Thuốc";
            this.btnMedicine.Click += new System.EventHandler(this.btnMedicine_Click);
            // 
            // btnManager
            // 
            this.btnManager.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMedBill,
            this.btnBill});
            this.btnManager.Name = "btnManager";
            this.btnManager.Size = new System.Drawing.Size(60, 20);
            this.btnManager.Text = "Quản lý";
            // 
            // btnMedBill
            // 
            this.btnMedBill.Name = "btnMedBill";
            this.btnMedBill.Size = new System.Drawing.Size(180, 22);
            this.btnMedBill.Text = "Đơn thuốc";
            // 
            // btnBill
            // 
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(180, 22);
            this.btnBill.Text = "Hóa đơn";
            // 
            // btnStatistic
            // 
            this.btnStatistic.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMedicineStat,
            this.btnPatientStat,
            this.btnMedicineReport,
            this.btnIncomeReport});
            this.btnStatistic.Name = "btnStatistic";
            this.btnStatistic.Size = new System.Drawing.Size(68, 20);
            this.btnStatistic.Text = "Thống kê";
            // 
            // btnMedicineStat
            // 
            this.btnMedicineStat.Name = "btnMedicineStat";
            this.btnMedicineStat.Size = new System.Drawing.Size(183, 22);
            this.btnMedicineStat.Text = "Thống kê đơn thuốc";
            this.btnMedicineStat.Click += new System.EventHandler(this.btnMedicineStat_Click);
            // 
            // btnPatientStat
            // 
            this.btnPatientStat.Name = "btnPatientStat";
            this.btnPatientStat.Size = new System.Drawing.Size(183, 22);
            this.btnPatientStat.Text = "Thống kê bệnh nhân";
            // 
            // btnMedicineReport
            // 
            this.btnMedicineReport.Name = "btnMedicineReport";
            this.btnMedicineReport.Size = new System.Drawing.Size(183, 22);
            this.btnMedicineReport.Text = "Báo cáo thuốc ";
            // 
            // btnIncomeReport
            // 
            this.btnIncomeReport.Name = "btnIncomeReport";
            this.btnIncomeReport.Size = new System.Drawing.Size(183, 22);
            this.btnIncomeReport.Text = "Báo cáo doanh thu";
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HotTrack = true;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1008, 577);
            this.tabControl.TabIndex = 1;
            // 
            // khoaToolStripMenuItem
            // 
            this.khoaToolStripMenuItem.Name = "khoaToolStripMenuItem";
            this.khoaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.khoaToolStripMenuItem.Text = "Khoa";
            this.khoaToolStripMenuItem.Click += new System.EventHandler(this.khoaToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "QUẢN LÝ BỆNH VIỆN";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnFunction;
        private System.Windows.Forms.ToolStripMenuItem btnLogout;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.ToolStripMenuItem btnCatalogue;
        private System.Windows.Forms.ToolStripMenuItem btnPatient;
        private System.Windows.Forms.ToolStripMenuItem btnEmployee;
        private System.Windows.Forms.ToolStripMenuItem btnRoom;
        private System.Windows.Forms.ToolStripMenuItem btnMedicine;
        private System.Windows.Forms.ToolStripMenuItem btnManager;
        private System.Windows.Forms.ToolStripMenuItem btnMedBill;
        private System.Windows.Forms.ToolStripMenuItem btnStatistic;
        private System.Windows.Forms.ToolStripMenuItem btnBill;
        private System.Windows.Forms.ToolStripMenuItem btnMedicineStat;
        private System.Windows.Forms.ToolStripMenuItem btnPatientStat;
        private System.Windows.Forms.ToolStripMenuItem btnMedicineReport;
        private System.Windows.Forms.ToolStripMenuItem btnIncomeReport;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem hồSơBệnhÁnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem khoaToolStripMenuItem;
    }
}

