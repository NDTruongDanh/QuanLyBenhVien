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
    public partial class BillForm : Form
    {
        public BillForm()
        {
            InitializeComponent();
        }
        public string BillNumber => txtBillNumber.Text;
        private void btnAddBillDetail_Click(object sender, EventArgs e)
        {
            BillDetailForm billDetailForm = new BillDetailForm(this);
            this.Enabled = false;
            billDetailForm.ShowDialog();
            this.Enabled = true;
        }
    }
}
