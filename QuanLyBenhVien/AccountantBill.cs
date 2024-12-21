using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class AccountantBill : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public AccountantBill()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            int month = now.Month;
            int year = now.Year;

            txtMonth.Text = month.ToString();
            txtYear.Text = year.ToString();
            LoadBills();
        }

        private void LoadBills()
        { 
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $@"SELECT TransactionID AS [Mã hóa đơn], RecordID AS [Mã hồ sơ], StaffID AS [Mã nhân viên], TransactionDate AS [Ngày giao dịch] ,PaymentMethod AS [Phương thức thanh toán], Total AS[Thành tiền]
                                   FROM  BILL
                                   WHERE 1=1";
                    if (int.TryParse(txtMonth.Text, out int month))
                    {
                        sql += $"AND MONTH(TransactionDate) = {month}";
                    }
                    if (int.TryParse(txtYear.Text, out int year))
                    {
                        sql += $"AND YEAR(TransactionDate) = {year}";
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataSet dataset = new DataSet())
                        {
                            adapter.Fill(dataset, "BILL");
                            dgvAccountantBill.DataSource = dataset.Tables["BILL"];
                        }
                    }



                    sql = $@"SELECT SUM(Total) AS [Tổng doanh thu]
                            FROM  BILL
                            WHERE 1=1";
                    if (int.TryParse(txtMonth.Text,out month))
                    {
                        sql += $"AND MONTH(TransactionDate) = {month}";
                    }
                    if (int.TryParse(txtYear.Text, out year))
                    {
                        sql += $"AND YEAR(TransactionDate) = {year}";
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataTable dataTable = new DataTable())
                        {
                            adapter.Fill(dataTable);
                            string revenue = dataTable.Rows[0]["Tổng doanh thu"].ToString();
                            if (!string.IsNullOrEmpty(revenue))
                                lblRevenue.Text = revenue + " VNĐ";
                            else
                                lblRevenue.Text = "0 VNĐ";
                        }
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu hóa đơn: {ex.Message}");
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LoadBills();
        }
    }
}
