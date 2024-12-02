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
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private SqlConnection conn;
        private SqlDataAdapter employeeAdapter;
        private DataSet employeeDataset;

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void OpenConnection()
        {
            if (conn == null)
                conn = new SqlConnection(connStr);

            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        private void CloseConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        private void LoadData()
        {
            try
            {
                OpenConnection();
                string sqlStr = "SELECT * FROM STAFF";

                employeeAdapter = new SqlDataAdapter(sqlStr, conn);
                employeeDataset = new DataSet();
                employeeAdapter.Fill(employeeDataset, "STAFF");

                dgvEmployee.DataSource = employeeDataset.Tables["STAFF"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        //private void ExecuteQuery(string query, Dictionary<string, object> parameters)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        using (SqlCommand command = new SqlCommand(query, conn))
        //        {
        //            foreach (var param in parameters)
        //            {
        //                command.Parameters.AddWithValue(param.Key, param.Value);
        //            }
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi thực thi: {ex.Message}");
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }
        //}

    }
}
