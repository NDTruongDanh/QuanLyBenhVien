namespace QuanLyBenhVien
{
    partial class PharmacistView
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
            this.chứcNăngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đổiMặtKhẩuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đăngXuấtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteRemember = new System.Windows.Forms.ToolStripMenuItem();
            this.thôngTinCáNhânToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMedicine = new System.Windows.Forms.ToolStripMenuItem();
            this.lịchTrựcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trợLýẢoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new Guna.UI2.WinForms.Guna2TabControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chứcNăngToolStripMenuItem,
            this.thôngTinCáNhânToolStripMenuItem1,
            this.btnMedicine,
            this.lịchTrựcToolStripMenuItem,
            this.trợLýẢoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 36);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // chứcNăngToolStripMenuItem
            // 
            this.chứcNăngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.đổiMặtKhẩuToolStripMenuItem,
            this.btnDeleteRemember,
            this.đăngXuấtToolStripMenuItem});
            this.chứcNăngToolStripMenuItem.Name = "chứcNăngToolStripMenuItem";
            this.chứcNăngToolStripMenuItem.Size = new System.Drawing.Size(119, 32);
            this.chứcNăngToolStripMenuItem.Text = "Chức năng";
            // 
            // đổiMặtKhẩuToolStripMenuItem
            // 
            this.đổiMặtKhẩuToolStripMenuItem.Name = "đổiMặtKhẩuToolStripMenuItem";
            this.đổiMặtKhẩuToolStripMenuItem.Size = new System.Drawing.Size(250, 32);
            this.đổiMặtKhẩuToolStripMenuItem.Text = "Đổi mặt khẩu";
            this.đổiMặtKhẩuToolStripMenuItem.Click += new System.EventHandler(this.đổiMặtKhẩuToolStripMenuItem_Click);
            // 
            // đăngXuấtToolStripMenuItem
            // 
            this.đăngXuấtToolStripMenuItem.Name = "đăngXuấtToolStripMenuItem";
            this.đăngXuấtToolStripMenuItem.Size = new System.Drawing.Size(250, 32);
            this.đăngXuấtToolStripMenuItem.Text = "Đăng xuất";
            this.đăngXuấtToolStripMenuItem.Click += new System.EventHandler(this.đăngXuấtToolStripMenuItem_Click);
            // 
            // btnDeleteRemember
            // 
            this.btnDeleteRemember.Name = "btnDeleteRemember";
            this.btnDeleteRemember.Size = new System.Drawing.Size(250, 32);
            this.btnDeleteRemember.Text = "Tắt nhớ mật khẩu";
            this.btnDeleteRemember.Click += new System.EventHandler(this.btnDeleteRemember_Click);
            // 
            // thôngTinCáNhânToolStripMenuItem1
            // 
            this.thôngTinCáNhânToolStripMenuItem1.Name = "thôngTinCáNhânToolStripMenuItem1";
            this.thôngTinCáNhânToolStripMenuItem1.Size = new System.Drawing.Size(182, 32);
            this.thôngTinCáNhânToolStripMenuItem1.Text = "Thông tin cá nhân";
            this.thôngTinCáNhânToolStripMenuItem1.Click += new System.EventHandler(this.thôngTinCáNhânToolStripMenuItem1_Click);
            // 
            // btnMedicine
            // 
            this.btnMedicine.Name = "btnMedicine";
            this.btnMedicine.Size = new System.Drawing.Size(116, 32);
            this.btnMedicine.Text = "Kho thuốc";
            this.btnMedicine.Click += new System.EventHandler(this.txtMedicine_Click);
            // 
            // lịchTrựcToolStripMenuItem
            // 
            this.lịchTrựcToolStripMenuItem.Name = "lịchTrựcToolStripMenuItem";
            this.lịchTrựcToolStripMenuItem.Size = new System.Drawing.Size(100, 32);
            this.lịchTrựcToolStripMenuItem.Text = "Lịch trực";
            this.lịchTrựcToolStripMenuItem.Click += new System.EventHandler(this.lịchTrựcToolStripMenuItem_Click);
            // 
            // trợLýẢoToolStripMenuItem
            // 
            this.trợLýẢoToolStripMenuItem.Name = "trợLýẢoToolStripMenuItem";
            this.trợLýẢoToolStripMenuItem.Size = new System.Drawing.Size(100, 32);
            this.trợLýẢoToolStripMenuItem.Text = "Trợ lý ảo";
            this.trợLýẢoToolStripMenuItem.Click += new System.EventHandler(this.trợLýẢoToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(150, 40);
            this.tabControl.Location = new System.Drawing.Point(0, 36);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 414);
            this.tabControl.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.tabControl.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.tabControl.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControl.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.tabControl.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.tabControl.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.tabControl.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tabControl.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControl.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.tabControl.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tabControl.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.tabControl.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.tabControl.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControl.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.tabControl.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.tabControl.TabButtonSize = new System.Drawing.Size(150, 40);
            this.tabControl.TabIndex = 2;
            this.tabControl.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            // 
            // PharmacistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.Name = "PharmacistView";
            this.Text = "DƯỢC SĨ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem chứcNăngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem đổiMặtKhẩuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem đăngXuấtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thôngTinCáNhânToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnMedicine;
        private System.Windows.Forms.ToolStripMenuItem lịchTrựcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trợLýẢoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnDeleteRemember;
        private Guna.UI2.WinForms.Guna2TabControl tabControl;
    }
}