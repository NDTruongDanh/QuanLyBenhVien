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
using QuanLyBenhVien.Classes;

namespace QuanLyBenhVien
{
    public partial class BillDetailForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public BillDetailForm()
        {
            InitializeComponent();
        }
        public BillDetailForm(BillForm bf)
        {
            InitializeComponent();
            lblBillNumber.Text = bf.BillNumber;
            LoadBillDetailData();
        }

        private void LoadBillDetailData()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $"SELECT MedicationID AS [Mã thuốc], Amount AS[Số lượng] FROM BILLDETAIL WHERE TransactionID = '{lblBillNumber.Text}'";
                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "BILLDETAIL");
                    dgvBillDetail.DataSource = dataset.Tables["BILLDETAIL"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu chi tiết hóa đơn: {ex.Message}");
                }
            }
        }

        private bool IsValidBillDetail()
        {
            if (!CommonChecks.IsNumber(txtAmount.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateBillDetail_Click(object sender, EventArgs e)
        {
            if (!IsValidBillDetail())
            {
                MessageBox.Show("Số lượng phải là 1 số.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM BILLDETAIL WHERE TransactionID = @TransactionID AND MedicationID = @MedicationID)
                     UPDATE BILLDETAIL SET Amount = @Amount
                     ELSE
                     INSERT INTO BILLDETAIL (TransactionID, MedicationID, Amount) VALUES (@TransactionID, @MedicationID, @Amount)";

            var parameters = new Dictionary<string, object>
            {
                {"@TransactionID", lblBillNumber.Text},
                {"@MedicationID", cmbMedicationID.Text},
                {"@Amount", txtAmount.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadBillDetailData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveBillDetail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblBillNumber.Text) || string.IsNullOrEmpty(cmbMedicationID.Text))
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                return;
            }

            string query = "DELETE FROM BILLDETAIL WHERE TransactionID = @TransactionID AND MedicationID = @MedicationID";
            var parameters = new Dictionary<string, object>
            {
                {"@TransactionID", lblBillNumber.Text},
                {"@MedicationID", cmbMedicationID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadBillDetailData();
        }

        private void btnFindBillDetail_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT MedicationID AS [Mã thuốc], Amount As[Số lượng] FROM BILLDETAIL WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(cmbMedicationID.Text))
                    {
                        query += " AND MedicationID = @MedicationID";
                        parameters.Add("@MedicationID", cmbMedicationID.Text);
                    }
                    if (!string.IsNullOrEmpty(txtAmount.Text))
                    {
                        query += " AND Amount = @Amount";
                        parameters.Add("@Amount", txtAmount.Text);
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

                        dgvBillDetail.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm chi tiết hóa đơn: {ex.Message}");
                }
            }
        }

        private void dgvBillDetail_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBillDetail.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvBillDetail.SelectedRows[0];

                cmbMedicationID.Text = selectedRow.Cells["MedicationID"].Value.ToString();
                txtAmount.Text = selectedRow.Cells["Amount"].Value.ToString();
            }
        }

    }
}
