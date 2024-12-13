using QuanLyBenhVien.Classes;
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
    public partial class BillForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public BillForm()
        {
            InitializeComponent();
            LoadBills();
            InitializeCmbRecordID();
            InitializeCmbStaffID();
        }
        public string BillNumber => txtTransactionID.Text;
        private void btnAddBillDetail_Click(object sender, EventArgs e)
        {
            BillDetailForm billDetailForm = new BillDetailForm(this);
            this.Enabled = false;
            billDetailForm.ShowDialog();
            this.Enabled = true;
            LoadBills();
        }

        private void LoadBills()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT \r\n    hd.TransactionID AS \"Mã hóa đơn\", \r\n    hd.RecordID AS \"Mã hồ sơ\", \r\n    hd.StaffID AS \"Mã nhân viên\", \r\n    hd.TransactionDate AS \"Ngày giao dịch\", \r\n    hd.PaymentMethod AS \"Phương thức thanh toán\", \r\n    hd.Total AS \"Tổng tiền\"\r\nFROM \r\n    BILL AS hd;\r\n";
                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "BILL");
                    dgvBill.DataSource = dataset.Tables["BILL"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu hóa đơn: {ex.Message}");
                }
            }
        }

        private void InitializeCmbRecordID()
        {
            string query = "SELECT RecordID FROM MEDICALRECORD";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbRecordID.Items.Add(reader["RecordID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void InitializeCmbStaffID()
        {
            string query = "SELECT StaffID FROM STAFF";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbStaffID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private bool IsValidBill()
        {
            if (string.IsNullOrEmpty(txtTransactionID.Text) ||
                string.IsNullOrEmpty(cmbRecordID.Text) ||
                string.IsNullOrEmpty(cmbStaffID.Text) ||
                string.IsNullOrEmpty(cmbPaymentMethod.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateBill_Click(object sender, EventArgs e)
        {
            if (!IsValidBill())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM BILL WHERE TransactionID = @TransactionID)
                     UPDATE BILL SET RecordID = @RecordID, StaffID = @StaffID, TransactionDate = @TransactionDate,
                     PaymentMethod = @PaymentMethod
                     WHERE TransactionID = @TransactionID
                     ELSE
                     INSERT INTO BILL (TransactionID, RecordID, StaffID, TransactionDate, PaymentMethod)
                     VALUES (@TransactionID, @RecordID, @StaffID, @TransactionDate, @PaymentMethod)";

            var parameters = new Dictionary<string, object>
            {
                {"@TransactionID", txtTransactionID.Text},
                {"@RecordID", cmbRecordID.Text},
                {"@StaffID", cmbStaffID.Text},
                {"@TransactionDate", dtpTransactionDate.Value.Date},
                {"@PaymentMethod", cmbPaymentMethod.Text}
                };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadBills();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveBill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTransactionID.Text))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa.");
                return;
            }

            string query = "DELETE FROM BILL WHERE TransactionID = @TransactionID";
            var parameters = new Dictionary<string, object>
            {
                {"@TransactionID", txtTransactionID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadBills();
        }

        private void btnFindBill_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT \r\n    hd.TransactionID AS \"Mã hóa đơn\", \r\n    hd.RecordID AS \"Mã hồ sơ\", \r\n    hd.StaffID AS \"Mã nhân viên\", \r\n    hd.TransactionDate AS \"Ngày giao dịch\", \r\n    hd.PaymentMethod AS \"Phương thức thanh toán\", \r\n    hd.Total AS \"Tổng tiền\"\r\nFROM \r\n    BILL AS hd\r\n WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtTransactionID.Text))
                    {
                        query += " AND TransactionID = @TransactionID";
                        parameters.Add("@TransactionID", txtTransactionID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbRecordID.Text))
                    {
                        query += " AND RecordID = @RecordID";
                        parameters.Add("@RecordID", cmbRecordID.Text);
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);

                        dgvBill.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm hóa đơn: {ex.Message}");
                }
            }
        }

        private void dgvBill_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBill.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvBill.SelectedRows[0];
                txtTransactionID.Text = selectedRow.Cells[0].Value.ToString();
                cmbRecordID.Text = selectedRow.Cells[1].Value.ToString();
                cmbStaffID.Text = selectedRow.Cells[2].Value.ToString();
                dtpTransactionDate.Text = selectedRow.Cells[3].Value.ToString();
                cmbPaymentMethod.Text = selectedRow.Cells[4].Value.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }
    }
}
