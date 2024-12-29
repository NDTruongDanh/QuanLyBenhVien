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
    public partial class DoctorAppointment : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public DoctorAppointment()
        {
            InitializeComponent();
            cmbSelection.Text = "Tuần này";
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = null;
                    if (cmbSelection.SelectedIndex == 1) //Next Week
                    {
                        sql = @"SELECT AppointmentDateTime AS [Ngày khám], FullName AS [Tên bệnh nhân] FROM APPOINTMENT a JOIN PATIENT p ON a.PatientID = p.PatientID 
                                   WHERE AppointmentDateTime > DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- Start of the week (Monday)
                                   AND AppointmentDateTime <= DATEADD(DAY, 15 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- End of the week (Sunday);
                                   ORDER BY AppointmentDateTime";
                    }
                    else // Current week    
                    {
                        sql = @"SELECT AppointmentDateTime AS [Ngày khám], FullName AS [Tên bệnh nhân] FROM APPOINTMENT a JOIN PATIENT p ON a.PatientID = p.PatientID 
                                   WHERE AppointmentDateTime > DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- Start of the week (Monday)
                                   AND AppointmentDateTime <= DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- End of the week (Sunday);
                                   ORDER BY AppointmentDateTime";
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            adapter.Fill(dataSet);

                            if(dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0) 
                            {
                                dgvAppointment.DataSource = dataSet.Tables[0];
                                dgvAppointment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                            }

                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show($"SQL Error: {sqlEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
