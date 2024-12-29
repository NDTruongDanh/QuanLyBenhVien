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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class KiemtraThuocTonKho : Form
    {
        public KiemtraThuocTonKho()
        {
            InitializeComponent();
            CommonControls.InitializeCmbCategory(cmbCategory);
        }

        string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private void btnOK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MedicationID AS [Mã thuốc], MedicationName AS[Tên thuốc], QuantityInStock AS[Số lượng tồn kho] FROM MEDICATION  WHERE 1=1";

                    if (!string.IsNullOrEmpty(cmbCategory.Text))
                        query += $"AND Category = N'{cmbCategory.Text}'";
                    if (!string.IsNullOrEmpty(txtQuantityInStock.Text))
                        query += $"AND QuantityInStock <= {txtQuantityInStock.Text}";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        using (DataTable table = new DataTable())
                        {
                            adapter.Fill(table);
                            dgvCheckMedicine.DataSource = table;
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
       
    }
}
