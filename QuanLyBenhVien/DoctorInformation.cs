using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class DoctorInformation : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public DoctorInformation()
        {
            InitializeComponent();
            LoadData(); 
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT st.StaffID, st.FullName, st.Gender, st.DateOfBirth, st.TypeOfStaff, DepartmentName, st.DateOfJoining, st.Email, st.PhoneNumber
                                   FROM STAFF st JOIN DEPARTMENT d ON st.DepartmentID = d.DepartmentID 
                                   WHERE StaffID = 'ST0001'";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataTable dataTable = new DataTable())
                        {
                            dataAdapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow row = dataTable.Rows[0];
                                lblDoctorID.Text = row["StaffID"].ToString();
                                lblFullName.Text = row["FullName"].ToString();
                                lblGender.Text = row["Gender"].ToString();
                                lblDateOfBirth.Text = Convert.ToDateTime(row["DateOfBirth"]).ToString("dd/MM/yyyy");
                                lblTypeOfDoctor.Text = row["TypeOfStaff"].ToString();
                                lblDepartment.Text = row["DepartmentName"].ToString();
                                lblDateOfJoining.Text = Convert.ToDateTime(row["DateOfJoining"]).ToString("dd/MM/yyyy");
                                lblEmail.Text = row["Email"].ToString();
                                lblPhoneNumber.Text = row["PhoneNumber"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No data found for the specified StaffID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }   
            }
        }
    }
}
