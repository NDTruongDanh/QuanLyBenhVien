# QuanLyBenhVien

**Do An Lap Trinh Truc Quan**  
Hospital Management System written in C#

---

## 📚 Overview

QuanLyBenhVien is a Windows Forms application designed to assist hospital staff in managing departments, patients, accounts, hospitalizations, and medical statistics. The system supports role-based access for various user types including Admin, Doctor, Pharmacist, Accountant, and Nurse.

## 🏗️ Features

- **User Authentication:** Login system with roles (Admin, Doctor, Pharmacist, Accountant, Nurse).
- **Department Management:** Add, update, find, and remove hospital departments.
- **Patient Management:** View, add, update, and remove patient records.
- **Hospitalization Management:** Handle patient admissions/discharges and room assignments.
- **Account Management:** Manage login accounts for system users.
- **Medical Statistics:** View disease statistics by month/year, medicine stock, and epidemic warnings.

## 🛠️ Installation

### Prerequisites

- Windows OS.
- [.NET Framework 4.7.2+](https://dotnet.microsoft.com/en-us/download/dotnet-framework).
- Microsoft SQL Server (local or remote) with a database named `HospitalDB`.
- Visual Studio (recommended for building and running the app).

### Database Setup

1. **Create Database:**  
   Ensure you have a SQL Server instance running and create a database called `HospitalDB`.

2. **Configure Connection String:**  
   The application uses the following connection string (see files like `DepartmentForm.cs`, `AccountForm.cs`):

   ```
   Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;
   ```

   - Replace `ADMIN-PC` with your SQL Server name as needed.
   - Ensure Windows Authentication is enabled or update for SQL Authentication.

3. **Run SQL Scripts:**  
   - Create required tables: `DEPARTMENT`, `USERLOGIN`, `PATIENT`, etc.
   - Seed initial data for users and roles as needed.

### Building and Running

1. **Clone the repository:**
   ```bash
   git clone https://github.com/NDTruongDanh/QuanLyBenhVien.git
   ```

2. **Open in Visual Studio:**
   - Open the solution file in Visual Studio.

3. **Restore NuGet packages (if any).**

4. **Build the project.**
   - Right-click the project and select `Build`.

5. **Run the application.**
   - Press `F5` or click `Start`.

## 🚀 Usage

- **Login Screen:**  
  On startup, you'll see a login window (`SignIn` form). Enter your credentials.
- **Role-based Navigation:**  
  Depending on your role, you are redirected to different main screens:
  - Admin: `MainForm`
  - Doctor: `DoctorView`
  - Pharmacist: `PharmacistView`
  - Accountant: `Accountant`
  - Nurse: `NurseView`

- **Department Management:**  
  Access via the Admin dashboard. Add, update, find, or remove departments.

- **Patient & Hospitalization Management:**  
  Doctors and Nurses can admit patients and manage records.

- **Account Management:**  
  Admin can manage system user accounts.

- **Statistics & Reports:**  
  View disease statistics by month/year, medicine stock, and epidemic warnings.

## 📝 Customization

- **Connection String:**  
  Update all occurrences of the connection string in source files if your SQL Server instance differs.
- **Database Schema:**  
  If you use different table/column names, adjust queries in the code accordingly.

## 📁 Project Structure

- `QuanLyBenhVien/Program.cs` - Application entry point and role-based navigation logic.
- `QuanLyBenhVien/DepartmentForm.cs` - Department management.
- `QuanLyBenhVien/PatientForm.cs` - Patient management.
- `QuanLyBenhVien/NhapVien.cs` - Hospitalization logic.
- `QuanLyBenhVien/AccountForm.cs` - User account management.
- `QuanLyBenhVien/BenhTheoThang.cs`, `BenhTheoNam.cs` - Medical statistics.
- `Classes/CommonQuery.cs` - Common SQL query logic.

## ⚠️ Notes

- The code assumes the database is named `HospitalDB` and is accessible via Windows Authentication (`Integrated Security=True`). Modify if using SQL Authentication.
- All database interaction uses raw SQL queries. Ensure your tables and columns match those used in the code.

## 💡 Troubleshooting

- **Database Errors:**  
  Double-check your connection string and database schema.
- **Missing Tables/Columns:**  
  Create or update your database as required by the queries in the code.
- **Login Issues:**  
  Seed initial users in the `USERLOGIN` table.

## 📄 License

This project is for educational purposes.

---

## 👥 Credits

| Name                     | Email                       |
|--------------------------|-----------------------------|
| Nguyễn Đại Trường Danh   | 23520242@gm.uit.edu.vn      |
| Lê Hùng Chính            | 23520187@gm.uit.edu.vn      |
| Lưu Bình                 | to be added                 |

---

*For questions or contributions, open an issue or pull request on GitHub!*
