﻿using System;
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
    public partial class BillDetailForm : Form
    {
        public BillDetailForm()
        {
            InitializeComponent();
        }
        public BillDetailForm(BillForm bf)
        {
            InitializeComponent();
            lblBillNumber.Text = bf.BillNumber;
        }
    }
}
