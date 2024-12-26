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
            this.btnChangePW = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCatalogue = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPatient = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicalRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEmployee = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDepartment = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicine = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManager = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBill = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAppointment = new System.Windows.Forms.ToolStripMenuItem();
            this.btnWeeklyAssignment = new System.Windows.Forms.ToolStripMenuItem();
            this.chămSócBệnhNhânToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStatistic = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicineReport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuantityInStock = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExpired = new System.Windows.Forms.ToolStripMenuItem();
            this.btnIncomeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMonthIncomeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnYearIncomeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDiseaseStat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnWarning = new System.Windows.Forms.ToolStripMenuItem();
            this.btnmonANDyearDisease = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMonthDiseaseStat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnYearDiseaseStat = new System.Windows.Forms.ToolStripMenuItem();
            this.trợLýẢoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.btnDeleteRemember = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFunction,
            this.btnCatalogue,
            this.btnManager,
            this.btnStatistic,
            this.trợLýẢoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1344, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnFunction
            // 
            this.btnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLogout,
            this.btnChangePW,
            this.btnDeleteRemember});
            this.btnFunction.Name = "btnFunction";
            this.btnFunction.Size = new System.Drawing.Size(93, 24);
            this.btnFunction.Text = "Chức năng";
            // 
            // btnLogout
            // 
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(224, 26);
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnChangePW
            // 
            this.btnChangePW.Name = "btnChangePW";
            this.btnChangePW.Size = new System.Drawing.Size(224, 26);
            this.btnChangePW.Text = "Đổi mặt khẩu";
            this.btnChangePW.Click += new System.EventHandler(this.btnChangePW_Click);
            // 
            // btnCatalogue
            // 
            this.btnCatalogue.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPatient,
            this.btnEmployee,
            this.btnRoom,
            this.btnDepartment,
            this.btnMedicine});
            this.btnCatalogue.Name = "btnCatalogue";
            this.btnCatalogue.Size = new System.Drawing.Size(90, 24);
            this.btnCatalogue.Text = "Danh mục";
            // 
            // btnPatient
            // 
            this.btnPatient.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMedicalRecord});
            this.btnPatient.Name = "btnPatient";
            this.btnPatient.Size = new System.Drawing.Size(171, 26);
            this.btnPatient.Text = "Bệnh nhân";
            this.btnPatient.Click += new System.EventHandler(this.btnPatient_Click);
            // 
            // btnMedicalRecord
            // 
            this.btnMedicalRecord.Name = "btnMedicalRecord";
            this.btnMedicalRecord.Size = new System.Drawing.Size(188, 26);
            this.btnMedicalRecord.Text = "Hồ sơ bệnh án";
            this.btnMedicalRecord.Click += new System.EventHandler(this.btnMedicalRecord_Click);
            // 
            // btnEmployee
            // 
            this.btnEmployee.Name = "btnEmployee";
            this.btnEmployee.Size = new System.Drawing.Size(171, 26);
            this.btnEmployee.Text = "Nhân viên";
            this.btnEmployee.Click += new System.EventHandler(this.btnEmployee_Click);
            // 
            // btnRoom
            // 
            this.btnRoom.Name = "btnRoom";
            this.btnRoom.Size = new System.Drawing.Size(171, 26);
            this.btnRoom.Text = "Phòng bệnh";
            this.btnRoom.Click += new System.EventHandler(this.btnRoom_Click);
            // 
            // btnDepartment
            // 
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(171, 26);
            this.btnDepartment.Text = "Khoa";
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // btnMedicine
            // 
            this.btnMedicine.Name = "btnMedicine";
            this.btnMedicine.Size = new System.Drawing.Size(171, 26);
            this.btnMedicine.Text = "Thuốc";
            this.btnMedicine.Click += new System.EventHandler(this.btnMedicine_Click);
            // 
            // btnManager
            // 
            this.btnManager.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBill,
            this.btnAppointment,
            this.btnWeeklyAssignment,
            this.chămSócBệnhNhânToolStripMenuItem,
            this.btnAccount});
            this.btnManager.Name = "btnManager";
            this.btnManager.Size = new System.Drawing.Size(73, 24);
            this.btnManager.Text = "Quản lý";
            // 
            // btnBill
            // 
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(229, 26);
            this.btnBill.Text = "Hóa đơn";
            this.btnBill.Click += new System.EventHandler(this.btnBill_Click);
            // 
            // btnAppointment
            // 
            this.btnAppointment.Name = "btnAppointment";
            this.btnAppointment.Size = new System.Drawing.Size(229, 26);
            this.btnAppointment.Text = "Lịch Khám";
            this.btnAppointment.Click += new System.EventHandler(this.btnAppointment_Click);
            // 
            // btnWeeklyAssignment
            // 
            this.btnWeeklyAssignment.Name = "btnWeeklyAssignment";
            this.btnWeeklyAssignment.Size = new System.Drawing.Size(229, 26);
            this.btnWeeklyAssignment.Text = "Lịch làm việc";
            this.btnWeeklyAssignment.Click += new System.EventHandler(this.btnWeeklyAssignment_Click);
            // 
            // chămSócBệnhNhânToolStripMenuItem
            // 
            this.chămSócBệnhNhânToolStripMenuItem.Name = "chămSócBệnhNhânToolStripMenuItem";
            this.chămSócBệnhNhânToolStripMenuItem.Size = new System.Drawing.Size(229, 26);
            this.chămSócBệnhNhânToolStripMenuItem.Text = "Chăm sóc bệnh nhân";
            this.chămSócBệnhNhânToolStripMenuItem.Click += new System.EventHandler(this.chămSócBệnhNhânToolStripMenuItem_Click);
            // 
            // btnAccount
            // 
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(229, 26);
            this.btnAccount.Text = "Tài khoản";
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // btnStatistic
            // 
            this.btnStatistic.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMedicineReport,
            this.btnIncomeReport,
            this.btnDiseaseStat});
            this.btnStatistic.Name = "btnStatistic";
            this.btnStatistic.Size = new System.Drawing.Size(84, 24);
            this.btnStatistic.Text = "Thống kê";
            // 
            // btnMedicineReport
            // 
            this.btnMedicineReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnQuantityInStock,
            this.btnExpired});
            this.btnMedicineReport.Name = "btnMedicineReport";
            this.btnMedicineReport.Size = new System.Drawing.Size(217, 26);
            this.btnMedicineReport.Text = "Báo cáo thuốc ";
            // 
            // btnQuantityInStock
            // 
            this.btnQuantityInStock.Name = "btnQuantityInStock";
            this.btnQuantityInStock.Size = new System.Drawing.Size(176, 26);
            this.btnQuantityInStock.Text = "Tồn kho";
            this.btnQuantityInStock.Click += new System.EventHandler(this.btnQuantityInStock_Click);
            // 
            // btnExpired
            // 
            this.btnExpired.Name = "btnExpired";
            this.btnExpired.Size = new System.Drawing.Size(176, 26);
            this.btnExpired.Text = "Hạn sử dụng";
            this.btnExpired.Click += new System.EventHandler(this.btnExpired_Click);
            // 
            // btnIncomeReport
            // 
            this.btnIncomeReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMonthIncomeReport,
            this.btnYearIncomeReport});
            this.btnIncomeReport.Name = "btnIncomeReport";
            this.btnIncomeReport.Size = new System.Drawing.Size(217, 26);
            this.btnIncomeReport.Text = "Báo cáo doanh thu";
            // 
            // btnMonthIncomeReport
            // 
            this.btnMonthIncomeReport.Name = "btnMonthIncomeReport";
            this.btnMonthIncomeReport.Size = new System.Drawing.Size(133, 26);
            this.btnMonthIncomeReport.Text = "Tháng";
            this.btnMonthIncomeReport.Click += new System.EventHandler(this.btnMonthIncomeReport_Click);
            // 
            // btnYearIncomeReport
            // 
            this.btnYearIncomeReport.Name = "btnYearIncomeReport";
            this.btnYearIncomeReport.Size = new System.Drawing.Size(133, 26);
            this.btnYearIncomeReport.Text = "Năm";
            this.btnYearIncomeReport.Click += new System.EventHandler(this.btnYearIncomeReport_Click);
            // 
            // btnDiseaseStat
            // 
            this.btnDiseaseStat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnWarning,
            this.btnmonANDyearDisease});
            this.btnDiseaseStat.Name = "btnDiseaseStat";
            this.btnDiseaseStat.Size = new System.Drawing.Size(217, 26);
            this.btnDiseaseStat.Text = "Tình hình Bệnh";
            // 
            // btnWarning
            // 
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(224, 26);
            this.btnWarning.Text = "Cảnh báo dịch bệnh";
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnmonANDyearDisease
            // 
            this.btnmonANDyearDisease.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMonthDiseaseStat,
            this.btnYearDiseaseStat});
            this.btnmonANDyearDisease.Name = "btnmonANDyearDisease";
            this.btnmonANDyearDisease.Size = new System.Drawing.Size(224, 26);
            this.btnmonANDyearDisease.Text = "Thống Kê";
            // 
            // btnMonthDiseaseStat
            // 
            this.btnMonthDiseaseStat.Name = "btnMonthDiseaseStat";
            this.btnMonthDiseaseStat.Size = new System.Drawing.Size(133, 26);
            this.btnMonthDiseaseStat.Text = "Tháng";
            this.btnMonthDiseaseStat.Click += new System.EventHandler(this.btnMonthDiseaseStat_Click);
            // 
            // btnYearDiseaseStat
            // 
            this.btnYearDiseaseStat.Name = "btnYearDiseaseStat";
            this.btnYearDiseaseStat.Size = new System.Drawing.Size(133, 26);
            this.btnYearDiseaseStat.Text = "Năm";
            this.btnYearDiseaseStat.Click += new System.EventHandler(this.btnYearDiseaseStat_Click);
            // 
            // trợLýẢoToolStripMenuItem
            // 
            this.trợLýẢoToolStripMenuItem.Name = "trợLýẢoToolStripMenuItem";
            this.trợLýẢoToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            this.trợLýẢoToolStripMenuItem.Text = "Trợ lý ảo ";
            this.trợLýẢoToolStripMenuItem.Click += new System.EventHandler(this.btnAIChatBot_Click);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HotTrack = true;
            this.tabControl.Location = new System.Drawing.Point(0, 28);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1344, 712);
            this.tabControl.TabIndex = 1;
            // 
            // btnDeleteRemember
            // 
            this.btnDeleteRemember.Name = "btnDeleteRemember";
            this.btnDeleteRemember.Size = new System.Drawing.Size(224, 26);
            this.btnDeleteRemember.Text = "Tắt nhớ mật khẩu";
            this.btnDeleteRemember.Click += new System.EventHandler(this.btnDeleteRemember_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1344, 740);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5);
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
        private System.Windows.Forms.ToolStripMenuItem btnChangePW;
        private System.Windows.Forms.ToolStripMenuItem btnCatalogue;
        private System.Windows.Forms.ToolStripMenuItem btnPatient;
        private System.Windows.Forms.ToolStripMenuItem btnEmployee;
        private System.Windows.Forms.ToolStripMenuItem btnRoom;
        private System.Windows.Forms.ToolStripMenuItem btnMedicine;
        private System.Windows.Forms.ToolStripMenuItem btnManager;
        private System.Windows.Forms.ToolStripMenuItem btnStatistic;
        private System.Windows.Forms.ToolStripMenuItem btnBill;
        private System.Windows.Forms.ToolStripMenuItem btnMedicineReport;
        private System.Windows.Forms.ToolStripMenuItem btnIncomeReport;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem btnMedicalRecord;
        private System.Windows.Forms.ToolStripMenuItem btnDepartment;

        private System.Windows.Forms.ToolStripMenuItem btnMonthIncomeReport;
        private System.Windows.Forms.ToolStripMenuItem btnYearIncomeReport;

        private System.Windows.Forms.ToolStripMenuItem trợLýẢoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnAppointment;
        private System.Windows.Forms.ToolStripMenuItem btnQuantityInStock;
        private System.Windows.Forms.ToolStripMenuItem btnExpired;
        private System.Windows.Forms.ToolStripMenuItem btnDiseaseStat;
        private System.Windows.Forms.ToolStripMenuItem btnWarning;

        private System.Windows.Forms.ToolStripMenuItem btnWeeklyAssignment;

        private System.Windows.Forms.ToolStripMenuItem btnmonANDyearDisease;
        private System.Windows.Forms.ToolStripMenuItem btnMonthDiseaseStat;
        private System.Windows.Forms.ToolStripMenuItem btnYearDiseaseStat;
        private System.Windows.Forms.ToolStripMenuItem chămSócBệnhNhânToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnAccount;
        private System.Windows.Forms.ToolStripMenuItem btnDeleteRemember;
    }
}

