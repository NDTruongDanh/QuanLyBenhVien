DROP DATABASE HospitalDB

CREATE DATABASE HospitalDB

USE HospitalDB

--Thống nhất tất cả các ID đều có đúng 6 ký tự bao gồm 2 chữ 4 số--
CREATE TABLE PATIENT (
    PatientID CHAR(6) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(15),
    AddressPatient NVARCHAR(255),
    Email VARCHAR(255)
)

--SELECT * FROM PATIENT
CREATE TABLE STAFF (
    StaffID CHAR(6) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    TypeOfStaff NVARCHAR(100),   -- Ví dụ: bác sĩ, điều dưỡng, quản lý, v.v.
    Gender NVARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    PhoneNumber VARCHAR(15),
    DateOfJoining DATE,
    Email VARCHAR(255),
    Salary MONEY,
    DepartmentID VARCHAR(6)
)


CREATE TABLE DEPARTMENT (
    DepartmentID VARCHAR(6) PRIMARY KEY,
    DepartmentName NVARCHAR(255) NOT NULL,
    EmployeeNumber INT,
    HeadDepartmentID CHAR(6),  -- Trưởng khoa (là StaffID)
    PhoneNumber VARCHAR(15),
    LocationDPM NVARCHAR(255)
)

CREATE TABLE APPOINTMENT (
    AppointmentID CHAR(6) PRIMARY KEY,
    PatientID CHAR(6),
    DoctorID CHAR(6),  -- Bác sĩ điều trị
    DepartmentID VARCHAR(6),
    AppointmentDateTime DATETIME,  -- Thời gian hẹn khám (ngày + giờ)
    AppointmentStatus NVARCHAR(50)  -- Trạng thái cuộc hẹn: đã xác nhận, hủy, hoàn thành, v.v.
)


CREATE TABLE MEDICALRECORD (
    RecordID CHAR(6) PRIMARY KEY,
    PatientID CHAR(6),
    DoctorID CHAR(6),
    VisitDate DATE,
    Diagnosis NVARCHAR(500),
    TestResults NVARCHAR(500),
    TreatmentPlan NVARCHAR(500)
)

CREATE TABLE NURSECARE (
	CareID CHAR(6) PRIMARY KEY,
	NurseID CHAR(6),
	PatientID CHAR(6),
	RoomID CHAR(6),
	CareDateTime SMALLDATETIME,
	CareType NVARCHAR(100), --Type of care provided (e.g., medication, vitals check).
	Notes NVARCHAR(500) --Additional details or observations by the nurse.
)


CREATE TABLE BILL (
    TransactionID CHAR(6) PRIMARY KEY,
    RecordID CHAR(6),
	StaffID CHAR(6),
    TransactionDate DATE,
    PaymentMethod NVARCHAR(50),  -- Phương thức thanh toán (tiền mặt, thẻ, bảo hiểm,...)
    Total MONEY
)

CREATE TABLE BILLDETAIL (
    TransactionID CHAR(6),
    MedicationID CHAR(6),
	MedicationName NVARCHAR(255),
    Amount INT,  -- Số lượng thuốc
    PRIMARY KEY (TransactionID, MedicationName)
)

CREATE TABLE MEDICATION (
    MedicationID CHAR(6) PRIMARY KEY,
    MedicationName NVARCHAR(255),
    Dosage NVARCHAR(100), --Liều lượng
	DosageUnit NVARCHAR(6),
    Category NVARCHAR(100),  -- Ví dụ: kháng sinh, giảm đau, v.v.
    QuantityInStock INT,  -- Số lượng thuốc trong kho
    Price MONEY,  -- Giá thuốc
    ExpiryDate DATE,-- Ngày hết hạn
	ManufacturingDate DATE,
    Manufacturer NVARCHAR(255)  -- NƯỚC SẢN XUẤT
)


CREATE TABLE ROOM (
    RoomID CHAR(6) PRIMARY KEY,
    DepartmentID VARCHAR(6),
    BedCount INT,  -- Số giường trong phòng
    RoomType NVARCHAR(50) , -- Loại phòng: thường, VIP, hồi sức, v.v.
	EmptyBed INT
)



CREATE TABLE WEEKLYASSIGNMENT (
    AssignmentID CHAR(6) PRIMARY KEY,
    StaffID CHAR(6),
    AssignmentDate DATE,
    ShiftType NVARCHAR(50)  -- Loại ca: Sáng, Chiều, Tối
)

CREATE TABLE USERLOGIN(
	UserID CHAR(6) PRIMARY KEY,
	Pass VARCHAR(20) NOT NULL,
	FLAG SMALLINT DEFAULT(0)
)

CREATE TABLE HOSPITALIZATION
(
	HospitalizationID CHAR(6) PRIMARY KEY,
	PatientID CHAR(6),
	RoomID CHAR(6),
	AdmissionDate DATETIME,
	DischargeDate DATETIME
)




--THÊM KHOÁ NGOẠI--
ALTER TABLE HOSPITALIZATION
ADD CONSTRAINT FK_HOS_PA FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID)

ALTER TABLE HOSPITALIZATION
ADD CONSTRAINT FK_HOS_RO FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID)

ALTER TABLE STAFF
ADD CONSTRAINT FK_STA_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE DEPARTMENT
ADD CONSTRAINT FK_DE_STA FOREIGN KEY (HeadDepartmentID) REFERENCES STAFF(StaffID) 

ALTER TABLE APPOINTMENT
ADD CONSTRAINT FK_AP_PA FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID) 

ALTER TABLE APPOINTMENT
ADD CONSTRAINT FK_AP_STA FOREIGN KEY (DoctorID) REFERENCES STAFF(StaffID) 

ALTER TABLE APPOINTMENT
ADD CONSTRAINT  FK_AP_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE MEDICALRECORD 
ADD CONSTRAINT FK_MERE_PA FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID) 

ALTER TABLE MEDICALRECORD 
ADD CONSTRAINT FK_MERE_STA  FOREIGN KEY (DoctorID) REFERENCES STAFF(StaffID) 

ALTER TABLE BILL
ADD CONSTRAINT FK_BI_MERE FOREIGN KEY (RecordID) REFERENCES MEDICALRECORD(RecordID) 

ALTER TABLE BILL
ADD CONSTRAINT FK_BI_STA FOREIGN KEY (StaffID) REFERENCES STAFF(StaffID)

ALTER TABLE BILLDETAIL
ADD CONSTRAINT FK_BIDE_BI FOREIGN KEY (TransactionID) REFERENCES BILL(TransactionID) ON DELETE CASCADE

ALTER TABLE BILLDETAIL
ADD CONSTRAINT FK_BIDE_ME FOREIGN KEY (MedicationID) REFERENCES MEDICATION(MedicationID) ON DELETE SET NULL

ALTER TABLE USERLOGIN
ADD CONSTRAINT FK_US_ST FOREIGN KEY (UserID) REFERENCES STAFF (StaffID)

ALTER TABLE ROOM
ADD CONSTRAINT FK_RO_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_STA FOREIGN KEY (StaffID) REFERENCES STAFF(StaffID) 

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_ST FOREIGN KEY (NurseID) REFERENCES STAFF(StaffID)

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_PT FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID)

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_R FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID)




-- Example data for NURSECARE table
INSERT INTO NURSECARE (CareID, NurseID, PatientID, RoomID, CareDateTime, CareType, Notes)
VALUES
('C00001', 'ST0009', 'PA0001', 'RO0001', '2024-12-20 09:00:00', N'Cấp phát thuốc', N'Đã cấp phát kháng sinh theo chỉ định.'),
('C00002', 'ST0010', 'PA0002', 'RO0002', '2024-12-20 11:30:00', N'Thay băng vết thương', N'Đã thay băng và làm sạch vết thương. Vết thương đang hồi phục tốt.'),
('C00003', 'ST0011', 'PA0003', 'RO0003', '2024-12-20 14:00:00', N'Kiểm tra dấu hiệu sinh tồn', N'Huyết áp: 120/80, Nhiệt độ: 37°C.'),
('C00004', 'ST0029', 'PA0004', 'RO0004', '2024-12-21 08:00:00', N'Vệ sinh bệnh nhân', N'Hỗ trợ bệnh nhân trong việc vệ sinh buổi sáng.'),
('C00005', 'ST0031', 'PA0005', 'RO0005', '2024-12-21 10:15:00', N'Theo dõi truyền dịch', N'Kiểm tra đường truyền IV và điều chỉnh tốc độ truyền dịch khi cần.'),
('C00006', 'ST0039', 'PA0006', 'RO0006', '2024-12-21 13:30:00', N'Hỗ trợ dinh dưỡng', N'Hỗ trợ bệnh nhân trong bữa ăn trưa và đảm bảo đủ nước.'),
('C00007', 'ST0042', 'PA0007', 'RO0007', '2024-12-21 15:45:00', N'Giáo dục bệnh nhân', N'Giải thích các bước chăm sóc sau khi xuất viện và chế độ thuốc.'),
('C00008', 'ST0047', 'PA0008', 'RO0008', '2024-12-22 09:00:00', N'Quản lý đau', N'Đã cấp phát thuốc giảm đau theo chỉ định của bác sĩ.'),
('C00009', 'ST0050', 'PA0009', 'RO0009', '2024-12-22 11:00:00', N'Theo dõi bệnh nhân', N'Theo dõi các dấu hiệu sinh tồn của bệnh nhân sau khi phẫu thuật.'),
('C00010', 'ST0055', 'PA0010', 'RO0010', '2024-12-22 14:30:00', N'Phản ứng cấp cứu', N'Đã phản ứng kịp thời với sự giảm huyết áp đột ngột và ổn định bệnh nhân.'),
('C00011', 'ST0057', 'PA0011', 'RO0011', '2024-12-23 08:30:00', N'Cấp phát thuốc', N'Đã cấp phát thuốc giảm đau.'),
('C00012', 'ST0009', 'PA0012', 'RO0012', '2024-12-23 10:45:00', N'Thay băng vết thương', N'Thay băng, vết thương có dấu hiệu phục hồi.'),
('C00013', 'ST0010', 'PA0013', 'RO0013', '2024-12-23 13:00:00', N'Kiểm tra dấu hiệu sinh tồn', N'Tất cả dấu hiệu sinh tồn ổn định, bệnh nhân cảm thấy tốt hơn.'),
('C00014', 'ST0011', 'PA0014', 'RO0014', '2024-12-23 15:15:00', N'Vệ sinh bệnh nhân', N'Hỗ trợ bệnh nhân trong việc vệ sinh, bệnh nhân cảm thấy thoải mái.'),
('C00015', 'ST0029', 'PA0015', 'RO0015', '2024-12-24 09:15:00', N'Theo dõi truyền dịch', N'Đường truyền IV hoạt động tốt.'),
('C00016', 'ST0031', 'PA0016', 'RO0016', '2024-12-24 11:30:00', N'Hỗ trợ dinh dưỡng', N'Khuyến khích bệnh nhân ăn uống đủ chất dinh dưỡng.'),
('C00017', 'ST0039', 'PA0017', 'RO0017', '2024-12-24 13:45:00', N'Giáo dục bệnh nhân', N'Thảo luận về các mục tiêu phục hồi của bệnh nhân.'),
('C00018', 'ST0042', 'PA0018', 'RO0018', '2024-12-24 16:00:00', N'Quản lý đau', N'Đã cấp phát thuốc giảm đau theo chỉ định.'),
('C00019', 'ST0047', 'PA0019', 'RO0019', '2024-12-25 08:00:00', N'Cấp phát thuốc', N'Đảm bảo cấp phát thuốc đúng giờ.'),
('C00020', 'ST0050', 'PA0020', 'RO0020', '2024-12-25 10:15:00', N'Thay băng vết thương', N'Làm sạch và thay băng vết thương sau phẫu thuật.'),
('C00021', 'ST0055', 'PA0021', 'RO0021', '2024-12-25 12:30:00', N'Kiểm tra dấu hiệu sinh tồn', N'Dấu hiệu sinh tồn trong phạm vi bình thường.'),
('C00022', 'ST0057', 'PA0022', 'RO0022', '2024-12-25 14:45:00', N'Vệ sinh bệnh nhân', N'Hỗ trợ bệnh nhân trong việc vệ sinh buổi tối.'),
('C00023', 'ST0009', 'PA0023', 'RO0023', '2024-12-26 09:00:00', N'Theo dõi truyền dịch', N'Đảm bảo dịch truyền được cấp phát đúng theo chỉ định.'),
('C00024', 'ST0010', 'PA0024', 'RO0024', '2024-12-26 11:30:00', N'Hỗ trợ dinh dưỡng', N'Giúp bệnh nhân trong bữa ăn.'),
('C00025', 'ST0011', 'PA0025', 'RO0025', '2024-12-26 13:45:00', N'Giáo dục bệnh nhân', N'Giải thích kế hoạch điều trị cho bệnh nhân và gia đình.'),
('C00026', 'ST0029', 'PA0026', 'RO0026', '2024-12-26 16:00:00', N'Quản lý đau', N'Đã cấp thuốc giảm đau theo chỉ định của bác sĩ.'),
('C00027', 'ST0031', 'PA0027', 'RO0027', '2024-12-27 08:30:00', N'Cấp phát thuốc', N'Đã cấp kháng sinh theo chỉ định.'),
('C00028', 'ST0039', 'PA0028', 'RO0028', '2024-12-27 10:45:00', N'Thay băng vết thương', N'Kiểm tra và làm sạch băng vết thương, bệnh nhân đang hồi phục.'),
('C00029', 'ST0042', 'PA0029', 'RO0029', '2024-12-27 13:00:00', N'Kiểm tra dấu hiệu sinh tồn', N'Dấu hiệu sinh tồn của bệnh nhân ổn định.'),
('C00030', 'ST0047', 'PA0030', 'RO0030', '2024-12-27 15:15:00', N'Vệ sinh bệnh nhân', N'Hoàn thành công tác vệ sinh buổi sáng.'),
('C00031', 'ST0050', 'PA0031', 'RO0001', '2024-12-28 09:00:00', N'Theo dõi truyền dịch', N'Đảm bảo dòng chảy dịch truyền đúng cách.'),
('C00032', 'ST0055', 'PA0032', 'RO0002', '2024-12-28 11:15:00', N'Hỗ trợ dinh dưỡng', N'Khuyến khích bệnh nhân uống đủ nước.'),
('C00033', 'ST0057', 'PA0033', 'RO0003', '2024-12-28 13:30:00', N'Giáo dục bệnh nhân', N'Thảo luận về mục tiêu sức khỏe hàng ngày.'),
('C00034', 'ST0009', 'PA0034', 'RO0004', '2024-12-28 15:45:00', N'Quản lý đau', N'Cấp phát thuốc giảm đau và ghi chép đầy đủ.'),
('C00035', 'ST0010', 'PA0035', 'RO0005', '2024-12-29 08:00:00', N'Cấp phát thuốc', N'Cung cấp thuốc theo lịch trình.'),
('C00036', 'ST0011', 'PA0036', 'RO0006', '2024-12-29 10:30:00', N'Thay băng vết thương', N'Vết thương đang hồi phục như mong đợi.'),
('C00037', 'ST0029', 'PA0037', 'RO0007', '2024-12-29 12:45:00', N'Kiểm tra dấu hiệu sinh tồn', N'Bệnh nhân ổn định, không có bất thường.'),
('C00038', 'ST0031', 'PA0038', 'RO0008', '2024-12-29 14:15:00', N'Vệ sinh bệnh nhân', N'Hỗ trợ bệnh nhân trong việc vệ sinh buổi sáng.'),
('C00039', 'ST0039', 'PA0039', 'RO0009', '2024-12-30 09:00:00', N'Theo dõi truyền dịch', N'Kiểm tra đường truyền IV và sự thoải mái của bệnh nhân.'),
('C00040', 'ST0042', 'PA0040', 'RO0010', '2024-12-30 11:30:00', N'Hỗ trợ dinh dưỡng', N'Đảm bảo bệnh nhân tiêu thụ đầy đủ dinh dưỡng.');



INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0000','1') ---ADMIN
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0009','1') ----Điều dưỡng 
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0001','1') ---- Bác sĩ
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0019','1') ---- Kế toán
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0015','1') ---- Dược sĩ (trưởng khoa)
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0061','1') ---- Dược sĩ



INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email)
VALUES 
('PA0001', N'Nguyễn Văn A', '1990-05-01', N'Nam', '0901234567', N'Hà Nội', 'a.nguyen@example.com'),
('PA0002', N'Phạm Thị B', '1985-10-15', N'Nữ', '0902345678', N'Đà Nẵng', 'b.pham@example.com'),
('PA0003', N'Ngô Văn C', '2000-07-20', N'Nam', '0903456789', N'Hồ Chí Minh', 'c.ngo@example.com'),
('PA0004', N'Trần Thị D', '1992-02-10', N'Nữ', '0904567890', N'Quảng Ninh', 'd.tran@example.com'),
('PA0005', N'Lê Minh E', '1980-11-11', N'Nam', '0905678901', N'Bình Dương', 'e.le@example.com'),
('PA0006', N'Vũ Thị F', '1995-03-22', N'Nữ', '0906789012', N'Vũng Tàu', 'f.vu@example.com'),
('PA0007', N'Đỗ Văn G', '1998-08-18', N'Nam', '0907890123', N'Cần Thơ', 'g.do@example.com'),
('PA0008', N'Hoàng Thị H', '1983-04-05', N'Nữ', '0908901234', N'Hải Phòng', 'h.hoang@example.com'),
('PA0009', N'Nguyễn Thị I', '1997-01-25', N'Nữ', '0909012345', N'Gia Lai', 'i.nguyen@example.com'),
('PA0010', N'Phạm Văn J', '1989-09-09', N'Nam', '0910123456', N'Nghệ An', 'j.pham@example.com'),
('PA0011', N'Trần Minh K', '2001-06-30', N'Nam', '0911234567', N'Lâm Đồng', 'k.tran@example.com'),
('PA0012', N'Lê Thị L', '1993-12-20', N'Nữ', '0912345678', N'Tây Ninh', 'l.le@example.com'),
('PA0013', N'Vũ Văn M', '1987-07-01', N'Nam', '0913456789', N'Nam Định', 'm.vu@example.com'),
('PA0014', N'Đặng Thị N', '1990-10-10', N'Nữ', '0914567890', N'Hà Tĩnh', 'n.dang@example.com'),
('PA0015', N'Tô Minh O', '1986-03-15', N'Nam', '0915678901', N'Khánh Hòa', 'o.to@example.com'),
('PA0016', N'Trịnh Thị P', '1999-11-25', N'Nữ', '0916789012', N'Bắc Ninh', 'p.trinh@example.com'),
('PA0017', N'Nguyễn Văn Q', '1985-06-12', N'Nam', '0917890123', N'Đồng Nai', 'q.nguyen@example.com'),
('PA0018', N'Phạm Thị R', '1991-08-19', N'Nữ', '0918901234', N'An Giang', 'r.pham@example.com'),
('PA0019', N'Vũ Văn S', '1988-02-22', N'Nam', '0919012345', N'Hà Nam', 's.vu@example.com'),
('PA0020', N'Lê Thị T', '1996-05-08', N'Nữ', '0920123456', N'Hòa Bình', 't.le@example.com'),
('PA0021', N'Đỗ Văn U', '1994-04-14', N'Nam', '0921234567', N'Thái Bình', 'u.do@example.com'),
('PA0022', N'Hoàng Thị V', '1992-07-21', N'Nữ', '0922345678', N'Ninh Thuận', 'v.hoang@example.com'),
('PA0023', N'Nguyễn Minh W', '1981-12-30', N'Nam', '0923456789', N'Hà Giang', 'w.nguyen@example.com'),
('PA0024', N'Phạm Thị X', '1989-11-11', N'Nữ', '0924567890', N'Phú Yên', 'x.pham@example.com'),
('PA0025', N'Ngô Văn Y', '1993-03-17', N'Nam', '0925678901', N'Tiền Giang', 'y.ngo@example.com'),
('PA0026', N'Trần Thị Z', '1997-09-09', N'Nữ', '0926789012', N'Sóc Trăng', 'z.tran@example.com'),
('PA0027', N'Đặng Văn AA', '1982-06-06', N'Nam', '0930123456', N'Quảng Nam', 'aa.dang@example.com'),
('PA0028', N'Lê Thị BB', '1984-01-05', N'Nữ', '0931234567', N'Bắc Giang', 'bb.le@example.com'),
('PA0029', N'Trịnh Minh CC', '1990-10-01', N'Nam', '0932345678', N'Thái Nguyên', 'cc.trinh@example.com'),
('PA0030', N'Tô Thị DD', '1999-08-24', N'Nữ', '0933456789', N'Bạc Liêu', 'dd.to@example.com'),
('PA0031', N'Nguyễn Văn EE', '2000-04-12', N'Nam', '0934567890', N'Quảng Ngãi', 'ee.nguyen@example.com'),
('PA0032', N'Lê Thị FF', '1983-07-07', N'Nữ', '0935678901', N'Hà Tây', 'ff.le@example.com'),
('PA0033', N'Đỗ Văn GG', '1991-09-14', N'Nam', '0936789012', N'Kon Tum', 'gg.do@example.com'),
('PA0034', N'Trần Thị HH', '1987-11-11', N'Nữ', '0940123456', N'Cà Mau', 'hh.tran@example.com'),
('PA0035', N'Vũ Văn II', '1995-12-20', N'Nam', '0941234567', N'Kiên Giang', 'ii.vu@example.com'),
('PA0036', N'Phạm Thị JJ', '1998-02-25', N'Nữ', '0942345678', N'Bình Phước', 'jj.pham@example.com'),
('PA0037', N'Hoàng Minh KK', '1989-06-18', N'Nam', '0943456789', N'Đồng Tháp', 'kk.hoang@example.com'),
('PA0038', N'Trịnh Thị LL', '1994-03-28', N'Nữ', '0944567890', N'Lạng Sơn', 'll.trinh@example.com'),
('PA0039', N'Lê Văn MM', '1986-11-01', N'Nam', '0945678901', N'Tuyên Quang', 'mm.le@example.com'),
('PA0040', N'Nguyễn Thị NN', '1997-05-14', N'Nữ', '0946789012', N'Yên Bái', 'nn.nguyen@example.com');




INSERT INTO DEPARTMENT (DepartmentID, DepartmentName, EmployeeNumber, HeadDepartmentID, PhoneNumber, LocationDPM)
VALUES
('KN', N'Khoa Nội', 11, 'ST0002', '0123456789', N'Tầng 1'),
('KNg', N'Khoa Ngoại', 4, 'ST0003', '0123456790', N'Tầng 2'),
('KS', N'Khoa Sản', 4, 'ST0001', '0123456791', N'Tầng 3'),
('KNh', N'Khoa Nhi', 4, 'ST0008', '0123456792', N'Tầng 4'),
('KHSCC', N'Khoa Hồi sức cấp cứu', 9, 'ST0014', '0123456793', N'Tầng 5'),
('KUB', N'Khoa Ung bướu', 6, 'ST0007', '0123456794', N'Tầng 6'),
('KTM', N'Khoa Tim mạch', 5, 'ST0004', '0123456795', N'Tầng 7'),
('KTK', N'Khoa Thần kinh', 2, 'ST0005', '0123456796', N'Tầng 8'),
('KDL', N'Khoa Da liễu', 4, 'ST0006', '0123456797', N'Tầng 9'),
('KTMH', N'Khoa Tai Mũi Họng', 2, 'ST0024', '0123456798', N'Tầng 10'),
('KM', N'Khoa Mắt', 1, 'ST0018', '0123456799', N'Tầng 11'),
('KRHM', N'Khoa Răng Hàm Mặt', 2, 'ST0035', '0123456700', N'Tầng 12'),
('KCDHA', N'Khoa Chẩn đoán hình ảnh', 2, 'ST0013', '0123456701', N'Tầng 13'),
('KXN', N'Khoa Xét nghiệm', 2, 'ST0012', '0123456702', N'Tầng 14'),
('KVLTL', N'Khoa Vật lý trị liệu – Phục hồi chức năng', 1, 'ST0021', '0123456703', N'Tầng 15'),
('KD', N'Khoa Dược', 2, 'ST0015', '0123756703', N'Tầng 16')





INSERT INTO STAFF (StaffID, FullName, TypeOfStaff, Gender, DateOfBirth, PhoneNumber, DateOfJoining, Email, Salary, DepartmentID)
VALUES
('ST0000', N'ADMIN', N'ADMIN', N'Nam', '2005-01-15', '0912345678', '2024-10-01', 'admin@gmail.com', 999999999, 'KN'),
('ST0001', N'Nguyễn Văn A', N'Bác sĩ Đa khoa', N'Nam', '1980-01-01', '0912345678', '2015-01-01', 'a.nguyen@example.com', 20000000, 'KN'),
('ST0002', N'Lê Thị B', N'Bác sĩ Nội khoa', N'Nữ', '1985-02-15', '0913456789', '2016-02-01', 'b.le@example.com', 22000000, 'KN'),
('ST0003', N'Trần Văn C', N'Bác sĩ Ngoại khoa', N'Nam', '1978-03-20', '0914567890', '2017-03-01', 'c.tran@example.com', 25000000, 'KNg'),
('ST0004', N'Phạm Thị D', N'Bác sĩ Tim mạch', N'Nữ', '1982-04-25', '0915678901', '2018-04-01', 'd.pham@example.com', 23000000, 'KTM'),
('ST0005', N'Vũ Văn E', N'Bác sĩ Thần kinh', N'Nam', '1987-05-30', '0916789012', '2019-05-01', 'e.vu@example.com', 24000000, 'KTK'),
('ST0006', N'Ngô Thị F', N'Bác sĩ Da liễu', N'Nữ', '1989-06-10', '0917890123', '2020-06-01', 'f.ngo@example.com', 22000000, 'KDL'),
('ST0007', N'Lê Quang G', N'Bác sĩ Ung bướu', N'Nam', '1983-07-15', '0918901234', '2021-07-01', 'g.le@example.com', 26000000, 'KUB'),
('ST0008', N'Trần Minh H', N'Bác sĩ Nhi khoa', N'Nam', '1990-08-20', '0919012345', '2022-08-01', 'h.tran@example.com', 21000000, 'KNh'),
('ST0009', N'Phạm Thị I', N'Điều dưỡng Tổng quát', N'Nữ', '1992-09-25', '0910123456', '2023-09-01', 'i.pham@example.com', 15000000, 'KN'),
('ST0010', N'Nguyễn Văn J', N'Điều dưỡng ICU', N'Nam', '1985-10-30', '0911234567', '2014-10-01', 'j.nguyen@example.com', 17000000, 'KHSCC'),
('ST0011', N'Lê Thị K', N'Điều dưỡng Sản khoa', N'Nữ', '1994-11-05', '0912345678', '2015-11-01', 'k.le@example.com', 16000000, 'KS'),
('ST0012', N'Vũ Văn L', N'Kỹ thuật viên Xét nghiệm', N'Nam', '1988-12-10', '0913456789', '2016-12-01', 'l.vu@example.com', 18000000, 'KXN'),
('ST0013', N'Phạm Thị M', N'Kỹ thuật viên Hình ảnh', N'Nữ', '1993-01-15', '0914567890', '2017-01-01', 'm.pham@example.com', 19000000, 'KCDHA'),
('ST0014', N'Trần Văn N', N'Bác sĩ Hồi sức cấp cứu', N'Nam', '1995-02-20', '0915678901', '2018-02-01', 'n.tran@example.com', 17000000, 'KHSCC'),
('ST0015', N'Ngô Thị O', N'Dược sĩ', N'Nữ', '1997-03-25', '0916789012', '2019-03-01', 'o.ngo@example.com', 20000000, 'KD'),
('ST0016', N'Phan Văn P', N'Hộ lý', N'Nam', '1980-04-30', '0917890123', '2020-04-01', 'p.phan@example.com', 14000000, 'KHSCC'),
('ST0017', N'Lê Thị Q', N'Nhân viên Nghiên cứu Y khoa', N'Nữ', '1983-05-10', '0918901234', '2021-05-01', 'q.le@example.com', 25000000, 'KUB'),
('ST0018', N'Vũ Văn R', N'Bác sĩ Mắt', N'Nam', '1985-06-15', '0919012345', '2022-06-01', 'r.vu@example.com', 30000000, 'KM'),
('ST0019', N'Nguyễn Thị S', N'Kế toán', N'Nữ', '1990-07-20', '0910123456', '2023-07-01', 's.nguyen@example.com', 18000000, 'KTMH'),
('ST0020', N'Trần Văn T', N'Quản lý', N'Nam', '1987-08-25', '0911234567', '2014-08-01', 't.tran@example.com', 25000000, 'KHSCC'),
('ST0021', N'Nguyễn Văn U', N'Bác sĩ Đa khoa', N'Nam', '1990-09-15', '0912345678', '2015-09-01', 'u.nguyen@example.com', 20000000, 'KN'),
('ST0022', N'Lê Thị V', N'Bác sĩ Nội khoa', N'Nữ', '1985-10-20', '0913456789', '2016-10-01', 'v.le@example.com', 22000000, 'KN'),
('ST0023', N'Trần Văn W', N'Bác sĩ Ngoại khoa', N'Nam', '1978-11-25', '0914567890', '2017-11-01', 'w.tran@example.com', 25000000, 'KNg'),
('ST0024', N'Phạm Thị X', N'Bác sĩ Tai Mũi Họng', N'Nữ', '1982-12-30', '0915678901', '2018-12-01', 'x.pham@example.com', 23000000, 'KTMH'),
('ST0025', N'Vũ Văn Y', N'Bác sĩ Thần kinh', N'Nam', '1987-01-05', '0916789012', '2019-01-01', 'y.vu@example.com', 24000000, 'KTK'),
('ST0026', N'Ngô Thị Z', N'Bác sĩ Da liễu', N'Nữ', '1989-02-10', '0917890123', '2020-02-01', 'z.ngo@example.com', 22000000, 'KDL'),
('ST0027', N'Lê Quang AA', N'Bác sĩ Ung bướu', N'Nam', '1983-03-15', '0918901234', '2021-03-01', 'aa.le@example.com', 26000000, 'KUB'),
('ST0028', N'Trần Minh BB', N'Bác sĩ Nhi khoa', N'Nam', '1990-04-20', '0919012345', '2022-04-01', 'bb.tran@example.com', 21000000, 'KNh'),
('ST0029', N'Phạm Thị CC', N'Điều dưỡng Tổng quát', N'Nữ', '1992-05-25', '0910123456', '2023-05-01', 'cc.pham@example.com', 15000000, 'KN'),
('ST0030', N'Nguyễn Văn DD', N'Kế toán', N'Nam', '1985-06-30', '0911234567', '2014-06-01', 'dd.nguyen@example.com', 17000000, 'KHSCC'),
('ST0031', N'Lê Thị EE', N'Điều dưỡng Sản khoa', N'Nữ', '1994-07-05', '0912345678', '2015-07-01', 'ee.le@example.com', 16000000, 'KS'),
('ST0032', N'Vũ Văn FF', N'Kỹ thuật viên Xét nghiệm', N'Nam', '1988-08-10', '0913456789', '2016-08-01', 'ff.vu@example.com', 18000000, 'KXN'),
('ST0033', N'Phạm Thị GG', N'Kỹ thuật viên Hình ảnh', N'Nữ', '1993-09-15', '0914567890', '2017-09-01', 'gg.pham@example.com', 19000000, 'KCDHA'),
('ST0034', N'Trần Văn HH', N'Kỹ thuật viên Vật lý trị liệu', N'Nam', '1995-10-20', '0915678901', '2018-10-01', 'hh.tran@example.com', 17000000, 'KVLTL'),
('ST0035', N'Ngô Thị II', N'Bác sĩ Răng Hàm Mặt', N'Nữ', '1997-11-25', '0916789012', '2019-11-01', 'ii.ngo@example.com', 20000000, 'KRHM'),
('ST0036', N'Phan Văn JJ', N'Hộ lý', N'Nam', '1980-12-30', '0917890123', '2020-12-01', 'jj.phan@example.com', 14000000, 'KHSCC'),
('ST0037', N'Lê Thị KK', N'Nhân viên Nghiên cứu Y khoa', N'Nữ', '1983-01-10', '0918901234', '2021-01-01', 'kk.le@example.com', 25000000, 'KUB'),
('ST0038', N'Vũ Văn LL', N'Giảng viên Y khoa', N'Nam', '1985-02-15', '0919012345', '2022-02-01', 'll.vu@example.com', 30000000, 'KRHM'),
('ST0039', N'Lê Văn HH', N'Điều dưỡng ICU', N'Nam', '1987-07-14', '0923456739', '2021-07-01', 'h39.le@example.com', 17000000, 'KHSCC'),
('ST0040', N'Nguyễn Văn KK', N'Bác sĩ Tim mạch', N'Nam', '1985-08-22', '0923456740', '2020-08-01', 'k40.nguyen@example.com', 23000000, 'KTM'),
('ST0041', N'Trần Thị NN', N'Bác sĩ Nội khoa', N'Nữ', '1985-03-15', '0923456781', '2016-03-01', 'nn.tran@example.com', 22000000, 'KN'),
('ST0042', N'Lê Văn OO', N'Điều dưỡng Tổng quát', N'Nam', '1990-06-20', '0923456782', '2017-06-01', 'oo.le@example.com', 15000000, 'KN'),
('ST0043', N'Phạm Thị PP', N'Bác sĩ Ngoại khoa', N'Nữ', '1988-07-25', '0923456783', '2018-07-01', 'pp.pham@example.com', 25000000, 'KNg'),
('ST0044', N'Nguyễn Văn QQ', N'Bác sĩ Tim mạch', N'Nam', '1979-04-10', '0923456784', '2019-04-01', 'qq.nguyen@example.com', 23000000, 'KTM'),
('ST0045', N'Vũ Thị RR', N'Kế toán', N'Nữ', '1992-08-15', '0923456785', '2020-08-01', 'rr.vu@example.com', 16000000, 'KS'),
('ST0046', N'Trần Văn SS', N'Bác sĩ Nhi khoa', N'Nam', '1983-11-05', '0923456786', '2021-11-01', 'ss.tran@example.com', 21000000, 'KNh'),
('ST0047', N'Lê Thị TT', N'Điều dưỡng ICU', N'Nữ', '1986-01-25', '0923456787', '2022-01-01', 'tt.le@example.com', 17000000, 'KHSCC'),
('ST0048', N'Phạm Văn UU', N'Bác sĩ Da liễu', N'Nam', '1980-02-15', '0923456788', '2023-02-01', 'uu.pham@example.com', 22000000, 'KDL'),
('ST0049', N'Nguyễn Thị VV', N'Bác sĩ Ung bướu', N'Nữ', '1982-09-30', '0923456789', '2015-09-01', 'vv.nguyen@example.com', 26000000, 'KUB'),
('ST0050', N'Trần Văn WW', N'Điều dưỡng Tổng quát', N'Nam', '1993-10-20', '0923456790', '2016-10-01', 'ww.tran@example.com', 15000000, 'KN'),
('ST0051', N'Phạm Thị XX', N'Bác sĩ Tim mạch', N'Nữ', '1985-12-15', '0923456791', '2017-12-01', 'xx.pham@example.com', 23000000, 'KTM'),
('ST0052', N'Lê Văn YY', N'Kế toán', N'Nam', '1990-01-30', '0923456792', '2018-01-01', 'yy.le@example.com', 17000000, 'KHSCC'),
('ST0053', N'Nguyễn Thị ZZ', N'Bác sĩ Nội khoa', N'Nữ', '1989-03-10', '0923456793', '2019-03-01', 'zz.nguyen@example.com', 22000000, 'KN'),
('ST0054', N'Trần Văn AA1', N'Bác sĩ Ngoại khoa', N'Nam', '1984-04-25', '0923456794', '2020-04-01', 'aa1.tran@example.com', 25000000, 'KNg'),
('ST0055', N'Lê Thị BB1', N'Điều dưỡng Sản khoa', N'Nữ', '1991-06-15', '0923456795', '2021-06-01', 'bb1.le@example.com', 16000000, 'KS'),
('ST0056', N'Vũ Văn CC1', N'Bác sĩ Nhi khoa', N'Nam', '1987-07-05', '0923456796', '2022-07-01', 'cc1.vu@example.com', 21000000, 'KNh'),
('ST0057', N'Nguyễn Thị DD1', N'Điều dưỡng Tổng quát', N'Nữ', '1994-08-20', '0923456797', '2023-08-01', 'dd1.nguyen@example.com', 15000000, 'KN'),
('ST0058', N'Trần Văn EE1', N'Bác sĩ Da liễu', N'Nam', '1983-09-15', '0923456798', '2015-09-01', 'ee1.tran@example.com', 22000000, 'KDL'),
('ST0059', N'Lê Thị FF1', N'Bác sĩ Ung bướu', N'Nữ', '1981-10-10', '0923456799', '2016-10-01', 'ff1.le@example.com', 26000000, 'KUB'),
('ST0060', N'Vũ Văn GG1', N'Bác sĩ Tim mạch', N'Nam', '1988-11-25', '0923456800', '2017-11-01', 'gg1.vu@example.com', 23000000, 'KTM'),
('ST0061', N'Vũ Văn HH1', N'Dược sĩ', N'Nam', '1987-11-25', '0923356800', '2017-11-01', 'gg1.vu@example.com', 23500000, 'KD');




INSERT INTO APPOINTMENT (AppointmentID, PatientID, DoctorID, DepartmentID, AppointmentDateTime, AppointmentStatus)
VALUES
('AP0001', 'PA0001', 'ST0001', 'KN', '2025-01-04 08:00:00', N'Chấp thuận'),
('AP0002', 'PA0002', 'ST0002', 'KN', '2025-01-04 08:30:00', N'Đang chờ xử lý'),
('AP0003', 'PA0003', 'ST0003', 'KNg', '2025-01-04 09:00:00', N'Từ chối'),
('AP0004', 'PA0004', 'ST0004', 'KTM', '2025-01-04 09:30:00', N'Chấp thuận'),
('AP0005', 'PA0005', 'ST0005', 'KTK', '2025-01-04 10:00:00', N'Đang chờ xử lý'),
('AP0006', 'PA0006', 'ST0007', 'KUB', '2025-01-05 10:30:00', N'Chấp thuận'),
('AP0007', 'PA0007', 'ST0008', 'KNh', '2025-01-05 11:00:00', N'Từ chối'),
('AP0008', 'PA0008', 'ST0014', 'KVLTL', '2025-01-05 11:30:00', N'Đang chờ xử lý'),
('AP0009', 'PA0009', 'ST0018', 'KM', '2025-01-05 12:00:00', N'Chấp thuận'),
('AP0010', 'PA0010', 'ST0001', 'KN', '2025-01-06 12:30:00', N'Từ chối'),
('AP0011', 'PA0011', 'ST0002', 'KN', '2025-01-06 13:00:00', N'Đang chờ xử lý'),
('AP0012', 'PA0012', 'ST0003', 'KNg', '2025-01-06 13:30:00', N'Chấp thuận'),
('AP0013', 'PA0013', 'ST0004', 'KTM', '2025-01-07 14:00:00', N'Từ chối'),
('AP0014', 'PA0014', 'ST0005', 'KTK', '2025-01-07 14:30:00', N'Đang chờ xử lý'),
('AP0015', 'PA0015', 'ST0007', 'KUB', '2025-01-07 15:00:00', N'Chấp thuận'),
('AP0016', 'PA0016', 'ST0008', 'KNh', '2025-01-08 15:30:00', N'Từ chối'),
('AP0017', 'PA0017', 'ST0014', 'KVLTL', '2025-01-08 16:00:00', N'Chấp thuận'),
('AP0018', 'PA0018', 'ST0018', 'KM', '2025-01-08 16:30:00', N'Đang chờ xử lý'),
('AP0019', 'PA0019', 'ST0001', 'KN', '2025-01-09 17:00:00', N'Từ chối'),
('AP0020', 'PA0020', 'ST0002', 'KN', '2025-01-09 17:30:00', N'Chấp thuận'),
('AP0021', 'PA0021', 'ST0003', 'KNg', '2025-01-09 18:00:00', N'Đang chờ xử lý'),
('AP0022', 'PA0022', 'ST0004', 'KTM', '2025-01-10 18:30:00', N'Từ chối'),
('AP0023', 'PA0023', 'ST0005', 'KTK', '2025-01-10 19:00:00', N'Chấp thuận'),
('AP0024', 'PA0024', 'ST0007', 'KUB', '2025-01-10 19:30:00', N'Đang chờ xử lý'),
('AP0025', 'PA0025', 'ST0008', 'KNh', '2025-01-11 20:00:00', N'Từ chối'),
('AP0026', 'PA0026', 'ST0014', 'KVLTL', '2025-01-11 20:30:00', N'Chấp thuận'),
('AP0027', 'PA0027', 'ST0018', 'KM', '2025-01-11 21:00:00', N'Đang chờ xử lý'),
('AP0028', 'PA0028', 'ST0001', 'KN', '2025-01-12 21:30:00', N'Từ chối'),
('AP0029', 'PA0029', 'ST0002', 'KN', '2025-01-12 08:00:00', N'Chấp thuận'),
('AP0030', 'PA0030', 'ST0003', 'KNg', '2025-01-13 08:30:00', N'Đang chờ xử lý'),
('AP0031', 'PA0031', 'ST0004', 'KTM', '2025-01-13 09:00:00', N'Từ chối'),
('AP0032', 'PA0032', 'ST0005', 'KTK', '2025-01-13 09:30:00', N'Chấp thuận'),
('AP0033', 'PA0033', 'ST0007', 'KUB', '2025-01-14 10:00:00', N'Đang chờ xử lý'),
('AP0034', 'PA0034', 'ST0008', 'KNh', '2025-01-14 10:30:00', N'Từ chối'),
('AP0035', 'PA0035', 'ST0014', 'KVLTL', '2025-01-14 11:00:00', N'Chấp thuận'),
('AP0036', 'PA0036', 'ST0018', 'KM', '2025-01-15 11:30:00', N'Đang chờ xử lý'),
('AP0037', 'PA0037', 'ST0001', 'KN', '2025-01-15 12:00:00', N'Từ chối'),
('AP0038', 'PA0038', 'ST0002', 'KN', '2025-01-16 12:30:00', N'Chấp thuận'),
('AP0039', 'PA0039', 'ST0003', 'KNg', '2025-01-16 13:00:00', N'Đang chờ xử lý'),
('AP0040', 'PA0040', 'ST0004', 'KTM', '2025-01-17 13:30:00', N'Từ chối');





INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan) 
VALUES 
('MR0001', 'PA0001', 'ST0001', '2023-12-01', N'Viêm phổi', N'X-ray bình thường', N'Điều trị kháng sinh'), 
('MR0002', 'PA0003', 'ST0007', '2023-12-03', N'Đau dạ dày', N'Siêu âm bình thường', N'Thuốc giảm đau'), 
('MR0003', 'PA0004', 'ST0008', '2023-12-04', N'Cảm cúm', N'Xét nghiệm máu', N'Thức ăn nhẹ, uống nước nhiều'), 
('MR0004', 'PA0006', 'ST0006', '2023-12-06', N'Bệnh tim mạch', N'ECG bình thường', N'Thuốc tim mạch'), 
('MR0005', 'PA0005', 'ST0002', '2023-12-03', N'Dau dạ dày', N'Nội soi: loét dạ dày', N'Dùng thuốc giảm tiết axit, kiêng đồ cay'), 
('MR0006', 'PA0006', 'ST0004', '2023-11-29', N'Đau đầu mãn tính', N'CT scan không phát hiện bất thường', N'Dùng thuốc giảm đau, giảm căng thẳng'), 
('MR0007', 'PA0007', 'ST0001', '2023-12-06', N'Cảm lạnh', N'Không có dấu hiệu nguy hiểm', N'Uống vitamin C và nghỉ ngơi'), 
('MR0008', 'PA0008', 'ST0005', '2023-12-09', N'Suy nhược cơ thể', N'Chỉ số máu thấp', N'Tăng cường dinh dưỡng, vitamin'), 
('MR0009', 'PA0009', 'ST0003', '2023-11-28', N'Hen suyễn', N'Chỉ số phổi giảm', N'Dùng thuốc hít và theo dõi định kỳ'), 
('MR0010', 'PA0010', 'ST0004', '2023-12-11', N'Đau lưng', N'X-quang: thoái hóa cột sống', N'Vật lý trị liệu 3 tuần'), 
('MR0011', 'PA0011', 'ST0001', '2023-11-21', N'Cảm cúm', N'Huyết áp bình thường, không có triệu chứng nặng', N'Nghỉ ngơi, uống thuốc giảm đau'), 
('MR0012', 'PA0012', 'ST0002', '2023-12-02', N'Sốt xuất huyết', N'Giảm tiểu cầu, sốt cao', N'Nhập viện, theo dõi hàng ngày'), 
('MR0013', 'PA0003', 'ST0001', '2023-11-26', N'Viêm phổi', N'X-quang phổi phát hiện tổn thương nhỏ', N'Dùng kháng sinh 7 ngày'), 
('MR0014', 'PA0004', 'ST0003', '2023-12-01', N'Gãy tay phải', N'Chụp X-quang: gãy xương quay', N'Bó bột và nghỉ ngơi 4 tuần'), 
('MR0015', 'PA0001', 'ST0001', '2023-01-15', N'Sốt xuất huyết', N'Tiểu cầu thấp, sốt cao', N'Nhập viện, truyền dịch, điều trị sốt'), 
('MR0016', 'PA0002', 'ST0002', '2023-02-20', N'Viêm phổi', N'Chụp X-quang phát hiện viêm phổi nhẹ', N'Dùng kháng sinh, theo dõi nhiệt độ'), 
('MR0017', 'PA0003', 'ST0003', '2023-03-10', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, mất nước nhẹ', N'Uống thuốc chống tiêu chảy, bù nước'), 
('MR0018', 'PA0004', 'ST0001', '2023-04-05', N'Sốt xuất huyết', N'Tiểu cầu giảm mạnh', N'Truyền dịch và thuốc hạ sốt'), 
('MR0019', 'PA0005', 'ST0002', '2023-05-11', N'Viêm phổi', N'Phổi tổn thương nhẹ', N'Kháng sinh, nghỉ ngơi tại nhà'), 
('MR0020', 'PA0006', 'ST0004', '2023-06-21', N'Tiêu chảy do nhiễm khuẩn', N'Phân có máu, sốt cao', N'Thực hiện xét nghiệm, uống thuốc kháng sinh'), 
('MR0021', 'PA0007', 'ST0003', '2023-07-07', N'Sốt xuất huyết', N'Huyết áp ổn định, tiểu cầu thấp', N'Nhập viện, theo dõi sức khỏe'), 
('MR0022', 'PA0008', 'ST0005', '2023-08-18', N'Viêm phổi', N'Khó thở, ho nhiều', N'Kháng sinh và điều trị hỗ trợ'), 
('MR0023', 'PA0009', 'ST0002', '2023-09-05', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, không có máu', N'Uống thuốc kháng sinh, bù nước'), 
('MR0024', 'PA0010', 'ST0004', '2023-10-12', N'Sốt xuất huyết', N'Phát ban, sốt cao', N'Chăm sóc tại bệnh viện, điều trị triệu chứng'), 
('MR0025', 'PA0011', 'ST0001', '2023-11-22', N'Viêm phổi', N'Chụp X-quang phát hiện phổi viêm', N'Dùng kháng sinh, nghỉ ngơi'), 
('MR0026', 'PA0012', 'ST0002', '2023-12-15', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'), 
-- Thêm năm 2024 từ đây
('MR0027', 'PA0013', 'ST0003', '2024-01-02', N'Sốt xuất huyết', N'Tiểu cầu giảm, huyết áp thấp', N'Truyền dịch, hạ sốt'), 
('MR0028', 'PA0014', 'ST0005', '2024-02-17', N'Viêm phổi', N'Khó thở nhẹ, X-quang không phát hiện bất thường', N'Kháng sinh, chăm sóc tại nhà'), 
('MR0029', 'PA0015', 'ST0004', '2024-03-13', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, mất nước nhẹ', N'Uống thuốc kháng sinh, bù nước'), 
('MR0030', 'PA0016', 'ST0001', '2024-04-09', N'Sốt xuất huyết', N'Tiểu cầu giảm, sốt cao', N'Nhập viện, truyền dịch, thuốc hạ sốt'), 
('MR0031', 'PA0017', 'ST0002', '2024-05-17', N'Viêm phổi', N'Phổi có dấu hiệu viêm nhẹ', N'Kháng sinh, uống thuốc giảm ho'), 
('MR0032', 'PA0011', 'ST0003', '2024-06-02', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, sốt nhẹ', N'Uống thuốc kháng sinh, bù nước'), 
('MR0033', 'PA0012', 'ST0005', '2024-07-25', N'Sốt xuất huyết', N'Tiểu cầu thấp, xuất huyết dưới da', N'Truyền dịch, điều trị hỗ trợ'), 
('MR0034', 'PA0013', 'ST0004', '2024-08-11', N'Viêm phổi', N'Khó thở nhẹ, X-quang phát hiện viêm phổi nhẹ', N'Dùng thuốc kháng sinh và thuốc giảm ho'), 
('MR0035', 'PA0002', 'ST0002', '2024-11-30', N'Covid-19', N'Xét nghiệm PCR dương tính, sốt, khó thở', N'Chăm sóc tại nhà, uống thuốc hạ sốt'), 
('MR0036', 'PA0003', 'ST0003', '2024-12-01', N'Viêm phổi', N'Khó thở, ho có đờm, đau ngực', N'Dùng kháng sinh, nghỉ ngơi'), 
('MR0037', 'PA0004', 'ST0001', '2024-12-02', N'Cảm cúm', N'Sốt cao, đau đầu, nghẹt mũi', N'Uống thuốc giảm đau, xịt mũi'), 
('MR0038', 'PA0005', 'ST0002', '2024-12-03', N'Covid-19', N'Khó thở, ho, mệt mỏi', N'Chăm sóc tại nhà, điều trị triệu chứng'), 
('MR0039', 'PA0006', 'ST0004', '2024-12-30', N'Viêm phổi', N'Phổi tổn thương nhẹ, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0040', 'PA0007', 'ST0003', '2024-12-26', N'Cảm cúm', N'Ho, đau họng, sốt nhẹ', N'Uống thuốc giảm đau, uống nhiều nước'), 
('MR0041', 'PA0008', 'ST0005', '2024-12-24', N'Covid-19', N'Xét nghiệm PCR dương tính, mệt mỏi', N'Chăm sóc tại nhà, điều trị hỗ trợ'), 
('MR0042', 'PA0009', 'ST0002', '2024-12-24', N'Viêm phổi', N'Khó thở, ho nhiều, đau ngực', N'Kháng sinh, theo dõi tại bệnh viện'), 
('MR0043', 'PA0010', 'ST0004', '2024-12-24', N'Cảm cúm', N'Sốt cao, ho, đau cơ', N'Uống thuốc hạ sốt, nghỉ ngơi'), 
('MR0044', 'PA0011', 'ST0001', '2024-12-24', N'Covid-19', N'Mệt mỏi, ho, sốt nhẹ', N'Chăm sóc tại nhà, thuốc giảm sốt'), 
('MR0045', 'PA0012', 'ST0002', '2024-12-25', N'Covid-19', N'Phổi có dấu hiệu viêm, ho, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0046', 'PA0013', 'ST0003', '2024-12-25', N'Cảm cúm', N'Ho, sốt, đau họng', N'Uống thuốc hạ sốt, xịt mũi'), 
('MR0047', 'PA0014', 'ST0005', '2024-12-24', N'Covid-19', N'Mệt mỏi, khó thở nhẹ', N'Chăm sóc tại nhà, thuốc giảm đau'), 
('MR0048', 'PA0015', 'ST0004', '2024-12-30', N'Viêm phổi', N'Khó thở, đau ngực, ho khan', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0049', 'PA0016', 'ST0001', '2024-12-20', N'Cảm cúm', N'Sốt, ho, mệt mỏi', N'Uống thuốc giảm đau, uống nhiều nước'), 
('MR0050', 'PA0017', 'ST0002', '2024-12-20', N'Covid-19', N'Mệt mỏi, khó thở nhẹ, ho', N'Chăm sóc tại nhà, điều trị triệu chứng'), 
('MR0051', 'PA0001', 'ST0001', '2024-12-30', N'Cảm cúm', N'Sốt, ho khan, mệt mỏi', N'Ist, uống thuốc hạ sốt, nghỉ ngơi'),
('MR0052', 'PA0012', 'ST0002', '2024-12-20', N'Cảm cúm', N'Phổi có dấu hiệu viêm, ho, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0053', 'PA0013', 'ST0003', '2024-12-20', N'Cảm cúm', N'Ho, sốt, đau họng', N'Uống thuốc hạ sốt, xịt mũi'), 
('MR0054', 'PA0014', 'ST0005', '2024-12-21', N'Cảm cúm', N'Mệt mỏi, khó thở nhẹ', N'Chăm sóc tại nhà, thuốc giảm đau'), 
('MR0055', 'PA0015', 'ST0004', '2024-12-21', N'Viêm phổi', N'Khó thở, đau ngực, ho khan', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0056', 'PA0016', 'ST0001', '2024-12-21', N'Cảm cúm', N'Sốt, ho, mệt mỏi', N'Uống thuốc giảm đau, uống nhiều nước'), 
('MR0057', 'PA0017', 'ST0002', '2024-12-20', N'Covid-19', N'Mệt mỏi, khó thở nhẹ, ho', N'Chăm sóc tại nhà, điều trị triệu chứng'), 
('MR0058', 'PA0001', 'ST0001', '2024-12-30', N'Cảm cúm', N'Sốt, ho khan, mệt mỏi', N'Ist, uống thuốc hạ sốt, nghỉ ngơi'),
('MR0059', 'PA0012', 'ST0002', '2024-12-20', N'Cảm cúm', N'Phổi có dấu hiệu viêm, ho, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0060', 'PA0013', 'ST0003', '2024-12-25', N'Cảm cúm', N'Ho, sốt, đau họng', N'Uống thuốc hạ sốt, xịt mũi'), 
('MR0061', 'PA0014', 'ST0005', '2024-12-26', N'Cảm cúm', N'Mệt mỏi, khó thở nhẹ', N'Chăm sóc tại nhà, thuốc giảm đau'), 
('MR0062', 'PA0015', 'ST0004', '2024-12-25', N'Viêm phổi', N'Khó thở, đau ngực, ho khan', N'Kháng sinh, nghỉ ngơi tại bệnh viện'), 
('MR0063', 'PA0016', 'ST0001', '2024-12-26', N'Cảm cúm', N'Sốt, ho, mệt mỏi', N'Uống thuốc giảm đau, uống nhiều nước'), 
('MR0064', 'PA0017', 'ST0002', '2024-12-25', N'Covid-19', N'Mệt mỏi, khó thở nhẹ, ho', N'Chăm sóc tại nhà, điều trị triệu chứng'), 
('MR0065', 'PA0001', 'ST0001', '2024-12-30', N'Cảm cúm', N'Sốt, ho khan, mệt mỏi', N'Ist, uống thuốc hạ sốt, nghỉ ngơi'),
('MR0066', 'PA0012', 'ST0002', '2025-11-12', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0067', 'PA0012', 'ST0002', '2024-11-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0068', 'PA0012', 'ST0002', '2024-12-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0069', 'PA0012', 'ST0002', '2024-12-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0070', 'PA0012', 'ST0002', '2024-12-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0071', 'PA0012', 'ST0002', '2024-11-12', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0072', 'PA0012', 'ST0002', '2024-12-12', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
--2025--
('MR0073', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0074', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0075', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0076', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0077', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0078', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0079', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0080', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0081', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0082', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0083', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0084', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0085', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0086', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0087', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0088', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0089', 'PA0012', 'ST0002', '2025-1-1', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh')




INSERT INTO MEDICATION (MedicationID, MedicationName, Dosage, DosageUnit, Category, QuantityInStock, Price, ExpiryDate, ManufacturingDate, Manufacturer)
VALUES
('ME0001', N'Paracetamol', N'500mg', N'viên', N'Giảm đau', 100, 50000, '2025-12-01', '2024-01-01', N'Việt Nam'),
('ME0002', N'Amoxicillin', N'250mg', N'viên', N'Kháng sinh', 50, 80000, '2025-06-01', '2024-02-01', N'Ấn Độ'),
('ME0003', N'Ciprofloxacin', N'500mg', N'viên', N'Kháng sinh', 70, 120000, '2025-09-01', '2024-03-01', N'Mỹ'),
('ME0004', N'Ibuprofen', N'400mg', N'viên', N'Giảm đau', 150, 60000, '2025-07-01', '2024-04-01', N'Anh'),
('ME0005', N'Omeprazole', N'20mg', N'viên', N'Tiêu hóa', 200, 40000, '2025-11-01', '2024-05-01', N'Nhật Bản'),
('ME0006', N'Diclofenac', N'50mg', N'viên', N'Giảm đau', 120, 70000, '2025-08-01', '2024-06-01', N'Trung Quốc'),
('ME0007', N'Aspirin', N'300mg', N'viên', N'Giảm đau', 90, 45000, '2024-04-01', '2024-03-15', N'Đức'),
('ME0008', N'Metformin', N'850mg', N'viên', N'Tiểu đường', 110, 90000, '2025-10-01', '2024-02-10', N'Hàn Quốc'),
('ME0009', N'Loratadine', N'10mg', N'viên', N'Dị ứng', 75, 30000, '2025-06-15', '2024-01-20', N'Việt Nam'),
('ME0010', N'Vitamin C', N'500mg', N'viên', N'Bổ sung', 300, 25000, '2025-12-20', '2024-03-05', N'Ấn Độ'),
('ME0011', N'Acyclovir', N'200mg', N'viên', N'Kháng virus', 60, 100000, '2025-09-25', '2024-04-15', N'Nhật Bản'),
('ME0012', N'Albuterol', N'2mg', N'chai', N'Hô hấp', 80, 150000, '2025-08-10', '2024-02-25', N'Pháp'),
('ME0013', N'Clopidogrel', N'75mg', N'viên', N'Tim mạch', 65, 120000, '2024-07-20', '2024-03-18', N'Mỹ'),
('ME0014', N'Atorvastatin', N'10mg', N'viên', N'Tim mạch', 100, 80000, '2025-06-05', '2024-02-12', N'Trung Quốc'),
('ME0015', N'Diazepam', N'5mg', N'viên', N'An thần', 50, 60000, '2025-05-10', '2024-03-01', N'Việt Nam'),
('ME0016', N'Dextromethorphan', N'15mg', N'gói', N'Hô hấp', 70, 45000, '2025-11-10', '2024-01-05', N'Hàn Quốc'),
('ME0017', N'Furosemide', N'40mg', N'viên', N'Lợi tiểu', 120, 55000, '2025-08-30', '2024-02-01', N'Đức'),
('ME0018', N'Itraconazole', N'100mg', N'viên', N'Nấm', 55, 90000, '2025-10-25', '2024-04-18', N'Nhật Bản'),
('ME0019', N'Azithromycin', N'250mg', N'viên', N'Kháng sinh', 80, 95000, '2025-09-15', '2024-03-25', N'Ấn Độ'),
('ME0020', N'Lisinopril', N'10mg', N'viên', N'Tim mạch', 90, 85000, '2025-07-15', '2024-03-10', N'Pháp'),
('ME0021', N'Losartan', N'50mg', N'viên', N'Tim mạch', 100, 100000, '2025-11-10', '2024-02-20', N'Mỹ'),
('ME0022', N'Montelukast', N'10mg', N'viên', N'Hô hấp', 85, 120000, '2025-06-01', '2024-04-12', N'Nhật Bản'),
('ME0023', N'Prednisone', N'5mg', N'viên', N'Dị ứng', 75, 60000, '2025-08-15', '2024-03-05', N'Anh'),
('ME0024', N'Ranitidine', N'150mg', N'viên', N'Tiêu hóa', 95, 75000, '2024-10-01', '2024-03-20', N'Việt Nam'),
('ME0025', N'Simvastatin', N'20mg', N'viên', N'Tim mạch', 110, 70000, '2025-12-01', '2024-01-18', N'Hàn Quốc'),
('ME0026', N'Tramadol', N'50mg', N'viên', N'Giảm đau', 60, 85000, '2025-07-20', '2024-04-10', N'Pháp'),
('ME0027', N'Warfarin', N'2mg', N'viên', N'Chống đông', 80, 65000, '2025-09-05', '2024-03-30', N'Mỹ'),
('ME0028', N'Zinc', N'50mg', N'viên', N'Bổ sung', 200, 25000, '2025-05-10', '2024-02-25', N'Ấn Độ'),
('ME0029', N'Amphotericin', N'100mg', N'chai', N'Nấm', 55, 120000, '2025-06-15', '2024-03-12', N'Nhật Bản'),
('ME0030', N'Amitriptyline', N'25mg', N'viên', N'Trầm cảm', 70, 95000, '2025-08-20', '2024-04-22', N'Trung Quốc');





INSERT INTO BILLDETAIL (TransactionID, MedicationID, MedicationName, Amount)
VALUES
('BI0001', 'ME0001', N'Paracetamol', 2), -- 2 viên Paracetamol
('BI0001', 'ME0002', N'Amoxicillin', 1), -- 1 viên Amoxicillin
('BI0002', 'ME0003', N'Ciprofloxacin', 1), -- 1 viên Ciprofloxacin
('BI0002', 'ME0004', N'Ibuprofen', 3), -- 3 viên Ibuprofen
('BI0003', 'ME0005', N'Omeprazole', 5), -- 5 gói Omeprazole
('BI0003', 'ME0006', N'Diclofenac', 2), -- 2 viên Diclofenac
('BI0004', 'ME0007', N'Aspirin', 4), -- 4 viên Aspirin
('BI0004', 'ME0008', N'Metformin', 2), -- 2 viên Metformin
('BI0005', 'ME0009', N'Loratadine', 3), -- 3 viên Loratadine
('BI0005', 'ME0010', N'Vitamin C', 4), -- 4 viên Vitamin C
('BI0006', 'ME0011', N'Acyclovir', 2), -- 2 viên Acyclovir
('BI0006', 'ME0012', N'Albuterol', 1), -- 1 viên Albuterol
('BI0007', 'ME0013', N'Clopidogrel', 1), -- 1 viên Clopidogrel
('BI0007', 'ME0014', N'Atorvastatin', 3), -- 3 viên Atorvastatin
('BI0008', 'ME0015', N'Diazepam', 2), -- 2 viên Diazepam
('BI0008', 'ME0016', N'Dextromethorphan', 4), -- 4 viên Dextromethorphan
('BI0009', 'ME0017', N'Furosemide', 2), -- 2 viên Furosemide
('BI0009', 'ME0018', N'Itraconazole', 2), -- 2 viên Itraconazole
('BI0010', 'ME0019', N'Azithromycin', 3), -- 3 viên Azithromycin
('BI0010', 'ME0020', N'Lisinopril', 2), -- 2 viên Lisinopril
('BI0011', 'ME0021', N'Losartan', 1), -- 1 viên Losartan
('BI0011', 'ME0022', N'Montelukast', 2), -- 2 viên Montelukast
('BI0012', 'ME0023', N'Prednisone', 4), -- 4 viên Prednisone
('BI0012', 'ME0024', N'Ranitidine', 2), -- 2 viên Ranitidine
('BI0013', 'ME0025', N'Simvastatin', 2), -- 2 viên Simvastatin
('BI0013', 'ME0026', N'Tramadol', 1), -- 1 viên Tramadol
('BI0014', 'ME0027', N'Warfarin', 1), -- 1 viên Warfarin
('BI0014', 'ME0028', N'Zinc', 8), -- 8 viên Zinc
('BI0015', 'ME0029', N'Amphotericin', 1), -- 1 viên Amphotericin
('BI0015', 'ME0030', N'Amitriptyline', 3), -- 3 viên Amitriptyline
('BI0016', 'ME0001', N'Paracetamol', 3), -- 3 viên Paracetamol
('BI0016', 'ME0004', N'Ibuprofen', 1), -- 1 viên Ibuprofen
('BI0017', 'ME0002', N'Amoxicillin', 2), -- 2 viên Amoxicillin
('BI0017', 'ME0005', N'Omeprazole', 3), -- 3 gói Omeprazole
('BI0018', 'ME0003', N'Ciprofloxacin', 1), -- 1 viên Ciprofloxacin
('BI0018', 'ME0006', N'Diclofenac', 2), -- 2 viên Diclofenac
('BI0019', 'ME0007', N'Aspirin', 6), -- 6 viên Aspirin
('BI0019', 'ME0008', N'Metformin', 3), -- 3 viên Metformin
('BI0020', 'ME0009', N'Loratadine', 2), -- 2 viên Loratadine
('BI0020', 'ME0010', N'Vitamin C', 6), -- 6 viên Vitamin C
('BI0021', 'ME0011', N'Acyclovir', 3), -- 3 viên Acyclovir
('BI0021', 'ME0012', N'Albuterol', 1), -- 1 viên Albuterol
('BI0022', 'ME0013', N'Clopidogrel', 1), -- 1 viên Clopidogrel
('BI0022', 'ME0014', N'Atorvastatin', 4), -- 4 viên Atorvastatin
('BI0023', 'ME0015', N'Diazepam', 3), -- 3 viên Diazepam
('BI0023', 'ME0016', N'Dextromethorphan', 5), -- 5 viên Dextromethorphan
('BI0024', 'ME0017', N'Furosemide', 2), -- 2 viên Furosemide
('BI0024', 'ME0018', N'Itraconazole', 4), -- 4 viên Itraconazole
('BI0025', 'ME0019', N'Azithromycin', 1), -- 1 viên Azithromycin
('BI0025', 'ME0020', N'Lisinopril', 3), -- 3 viên Lisinopril
('BI0026', 'ME0021', N'Losartan', 2), -- 2 viên Losartan
('BI0026', 'ME0022', N'Montelukast', 4), -- 4 viên Montelukast
('BI0027', 'ME0023', N'Prednisone', 3), -- 3 viên Prednisone
('BI0027', 'ME0024', N'Ranitidine', 4), -- 4 viên Ranitidine
('BI0028', 'ME0025', N'Simvastatin', 3), -- 3 viên Simvastatin
('BI0028', 'ME0026', N'Tramadol', 2), -- 2 viên Tramadol
('BI0029', 'ME0027', N'Warfarin', 2), -- 2 viên Warfarin
('BI0029', 'ME0028', N'Zinc', 4), -- 4 viên Zinc
('BI0030', 'ME0029', N'Amphotericin', 4), -- 4 viên Amphotericin
('BI0030', 'ME0030', N'Amitriptyline', 2); -- 2 viên Amitriptyline





INSERT INTO BILL (TransactionID, RecordID, StaffID, TransactionDate, PaymentMethod, Total)
VALUES
('BI0001', 'MR0001', 'ST0004', '2023-01-15', N'Tiền mặt', 180000), -- Paracetamol (2 viên), Amoxicillin (1 viên)
('BI0002', 'MR0002', 'ST0002', '2023-02-20', N'Thẻ tín dụng', 300000), -- Ciprofloxacin (1 viên), Ibuprofen (3 viên)
('BI0003', 'MR0003', 'ST0002', '2023-03-10', N'Bảo hiểm', 340000), -- Omeprazole (5 gói), Diclofenac (2 viên)
('BI0004', 'MR0004', 'ST0004', '2023-04-05', N'Tiền mặt', 360000), -- Aspirin (4 viên), Metformin (2 viên)
('BI0005', 'MR0005', 'ST0002', '2023-05-18', N'Tiền mặt', 190000), -- Loratadine (3 viên), Vitamin C (4 viên)
('BI0006', 'MR0006', 'ST0005', '2023-06-25', N'Ví điện tử', 350000), -- Acyclovir (2 viên), Albuterol (1 viên)
('BI0007', 'MR0007', 'ST0003', '2023-07-02', N'Thẻ tín dụng', 360000), -- Clopidogrel (1 viên), Atorvastatin (3 viên)
('BI0008', 'MR0008', 'ST0004', '2023-08-14', N'Tiền mặt', 300000), -- Diazepam (2 viên), Dextromethorphan (4 viên)
('BI0009', 'MR0009', 'ST0006', '2023-09-22', N'Tiền mặt', 290000), -- Furosemide (2 viên), Itraconazole (2 viên)
('BI0010', 'MR0010', 'ST0007', '2023-10-30', N'Bảo hiểm', 455000), -- Azithromycin (3 viên), Lisinopril (2 viên)
('BI0011', 'MR0011', 'ST0001', '2023-11-11', N'Ví điện tử', 340000), -- Losartan (1 viên), Montelukast (2 viên)
('BI0012', 'MR0012', 'ST0002', '2023-12-24', N'Tiền mặt', 390000), -- Prednisone (4 viên), Ranitidine (2 viên)
('BI0013', 'MR0013', 'ST0004', '2024-01-12', N'Thẻ tín dụng', 225000), -- Simvastatin (2 viên), Tramadol (1 viên)
('BI0014', 'MR0014', 'ST0003', '2024-02-27', N'Ví điện tử', 265000), -- Warfarin (1 viên), Zinc (8 viên)
('BI0015', 'MR0015', 'ST0005', '2024-03-19', N'Tiền mặt', 470000), -- Amphotericin (4 viên), Amitriptyline (3 viên)
('BI0016', 'MR0016', 'ST0002', '2024-04-05', N'Bảo hiểm', 180000), -- Paracetamol (3 viên), Ibuprofen (1 viên)
('BI0017', 'MR0017', 'ST0006', '2024-05-22', N'Thẻ tín dụng', 300000), -- Amoxicillin (2 viên), Omeprazole (3 gói)
('BI0018', 'MR0018', 'ST0001', '2024-06-11', N'Ví điện tử', 240000), -- Ciprofloxacin (1 viên), Diclofenac (2 viên)
('BI0019', 'MR0019', 'ST0004', '2024-07-03', N'Tiền mặt', 540000), -- Aspirin (6 viên), Metformin (3 viên)
('BI0020', 'MR0020', 'ST0003', '2024-08-15', N'Thẻ tín dụng', 180000), -- Loratadine (2 viên), Vitamin C (6 viên)
('BI0021', 'MR0021', 'ST0007', '2024-09-26', N'Tiền mặt', 350000), -- Acyclovir (3 viên), Albuterol (1 viên)
('BI0022', 'MR0022', 'ST0005', '2024-10-05', N'Bảo hiểm', 380000), -- Clopidogrel (1 viên), Atorvastatin (4 viên)
('BI0023', 'MR0023', 'ST0006', '2024-11-13', N'Ví điện tử', 375000), -- Diazepam (3 viên), Dextromethorphan (5 viên)
('BI0024', 'MR0024', 'ST0004', '2024-12-07', N'Tiền mặt', 420000), -- Furosemide (2 viên), Itraconazole (3 viên)
('BI0025', 'MR0025', 'ST0001', '2024-12-25', N'Thẻ tín dụng', 295000), -- Azithromycin (1 viên), Lisinopril (3 viên)
('BI0026', 'MR0026', 'ST0002', '2024-12-28', N'Tiền mặt', 340000), -- Losartan (2 viên), Montelukast (2 viên)
('BI0027', 'MR0027', 'ST0005', '2024-12-29', N'Ví điện tử', 330000), -- Prednisone (3 viên), Ranitidine (2 viên)
('BI0028', 'MR0028', 'ST0006', '2024-12-30', N'Thẻ tín dụng', 395000), -- Simvastatin (3 viên), Tramadol (2 viên)
('BI0029', 'MR0029', 'ST0007', '2024-12-31', N'Bảo hiểm', 305000), -- Warfarin (2 viên), Zinc (4 viên)
('BI0030', 'MR0030', 'ST0003', '2024-12-31', N'Tiền mặt', 680000); -- Amphotericin (4 viên), Amitriptyline (2 viên)




INSERT INTO ROOM (RoomID, DepartmentID, BedCount, RoomType)
VALUES
('RO0001', 'KN', 10, N'Điều trị tổng quát'),
('RO0002', 'KNg', 12, N'VIP'),
('RO0003', 'KHSCC', 8, N'Hồi sức'),
('RO0004', 'KNh', 6, N'Sơ sinh'),
('RO0005', 'KS', 4, N'Chăm sóc đặc biệt'),
('RO0006', 'KUB', 5, N'Phẫu thuật'),
('RO0007', 'KTM', 7, N'Cách ly'),
('RO0008', 'KTK', 8, N'Khám ngoại trú'),
('RO0009', 'KDL', 6, N'Chăm sóc dài hạn'),
('RO0010', 'KVLTL', 4, N'Sản khoa'),
('RO0011', 'KM', 10, N'Điều trị tổng quát'),
('RO0012', 'KRHM', 12, N'VIP'),
('RO0013', 'KCDHA', 8, N'Hồi sức'),
('RO0014', 'KXN', 6, N'Sơ sinh'),
('RO0015', 'KTMH', 4, N'Chăm sóc đặc biệt'),
('RO0016', 'KUB', 5, N'Phẫu thuật'),
('RO0017', 'KTK', 7, N'Cách ly'),
('RO0018', 'KVLTL', 8, N'Khám ngoại trú'),
('RO0019', 'KN', 6, N'Chăm sóc dài hạn'),
('RO0020', 'KNg', 4, N'Sản khoa'),
('RO0021', 'KNh', 10, N'Điều trị tổng quát'),
('RO0022', 'KS', 12, N'VIP'),
('RO0023', 'KHSCC', 8, N'Hồi sức'),
('RO0024', 'KDL', 6, N'Sơ sinh'),
('RO0025', 'KTM', 4, N'Chăm sóc đặc biệt'),
('RO0026', 'KTK', 5, N'Phẫu thuật'),
('RO0027', 'KCDHA', 7, N'Cách ly'),
('RO0028', 'KM', 8, N'Khám ngoại trú'),
('RO0029', 'KXN', 6, N'Chăm sóc dài hạn'),
('RO0030', 'KVLTL', 4, N'Sản khoa'),
('RO0031', 'KRHM', 10, N'Điều trị tổng quát'),
('RO0032', 'KN', 12, N'VIP'),
('RO0033', 'KNg', 8, N'Hồi sức'),
('RO0034', 'KTM', 6, N'Sơ sinh'),
('RO0035', 'KDL', 4, N'Chăm sóc đặc biệt'),
('RO0036', 'KTK', 5, N'Phẫu thuật'),
('RO0037', 'KUB', 7, N'Cách ly'),
('RO0038', 'KS', 8, N'Khám ngoại trú'),
('RO0039', 'KNh', 6, N'Chăm sóc dài hạn'),
('RO0040', 'KHSCC', 4, N'Sản khoa'),
('RO0041', 'KTMH', 10, N'Điều trị tổng quát'),
('RO0042', 'KCDHA', 12, N'VIP'),
('RO0043', 'KXN', 8, N'Hồi sức'),
('RO0044', 'KRHM', 6, N'Sơ sinh'),
('RO0045', 'KM', 4, N'Chăm sóc đặc biệt'),
('RO0046', 'KVLTL', 5, N'Phẫu thuật'),
('RO0047', 'KTM', 7, N'Cách ly'),
('RO0048', 'KTK', 8, N'Khám ngoại trú'),
('RO0049', 'KDL', 6, N'Chăm sóc dài hạn'),
('RO0050', 'KN', 4, N'Sản khoa');




INSERT INTO HOSPITALIZATION (HospitalizationID, PatientID, RoomID, AdmissionDate, DischargeDate)
VALUES
('H00001', 'PA0001', 'RO0001', '2024-12-01 10:00:00', '2024-12-10 14:00:00'),
('H00002', 'PA0002', 'RO0002', '2024-12-02 11:30:00', '2024-12-11 15:30:00'),
('H00003', 'PA0003', 'RO0003', '2024-12-03 12:00:00', '2024-12-12 16:00:00'),
('H00004', 'PA0004', 'RO0004', '2024-12-04 13:00:00', '2024-12-13 17:00:00'),
('H00005', 'PA0005', 'RO0005', '2024-12-05 14:15:00', '2024-12-14 18:00:00'),
('H00006', 'PA0006', 'RO0006', '2024-12-06 15:30:00', '2024-12-15 19:00:00'),
('H00007', 'PA0007', 'RO0007', '2024-12-07 16:00:00', '2024-12-16 20:00:00'),
('H00008', 'PA0008', 'RO0008', '2024-12-08 17:15:00', '2024-12-17 21:00:00'),
('H00009', 'PA0009', 'RO0009', '2024-12-09 18:30:00', '2024-12-18 22:00:00'),
('H00010', 'PA0010', 'RO0010', '2024-12-10 19:00:00', '2024-12-19 23:00:00'),
('H00011', 'PA0011', 'RO0011', '2024-12-11 09:00:00', '2024-12-20 10:00:00'),
('H00012', 'PA0012', 'RO0012', '2024-12-12 10:30:00', '2024-12-21 11:30:00'),
('H00013', 'PA0013', 'RO0013', '2024-12-13 11:00:00', '2024-12-22 12:30:00'),
('H00014', 'PA0014', 'RO0014', '2024-12-14 12:15:00', '2024-12-23 13:00:00'),
('H00015', 'PA0015', 'RO0015', '2024-12-15 13:30:00', '2024-12-24 14:30:00'),
('H00016', 'PA0016', 'RO0016', '2024-12-16 14:45:00', '2024-12-25 15:30:00'),
('H00017', 'PA0017', 'RO0017', '2024-12-17 15:00:00', '2024-12-26 16:00:00'),
('H00018', 'PA0018', 'RO0018', '2024-12-18 16:30:00', '2024-12-27 17:30:00'),
('H00019', 'PA0019', 'RO0019', '2024-12-19 17:00:00', '2024-12-28 18:30:00'),
('H00020', 'PA0020', 'RO0020', '2024-12-20 18:15:00', '2024-12-29 19:00:00'),
('H00021', 'PA0021', 'RO0021', '2024-12-21 19:30:00', '2024-12-30 20:00:00'),
('H00022', 'PA0022', 'RO0022', '2024-12-22 20:00:00', '2024-12-31 21:30:00'),
('H00023', 'PA0023', 'RO0023', '2024-12-23 21:30:00', '2025-01-01 22:30:00'),
('H00024', 'PA0024', 'RO0024', '2024-12-24 22:00:00', '2025-01-02 23:00:00'),
('H00025', 'PA0025', 'RO0025', '2024-12-25 23:00:00', '2025-01-03 08:00:00'),
('H00026', 'PA0026', 'RO0026', '2024-12-26 08:30:00', '2025-01-04 09:00:00'),
('H00027', 'PA0027', 'RO0027', '2024-12-27 09:00:00', '2025-01-05 10:00:00'),
('H00028', 'PA0028', 'RO0028', '2024-12-28 10:30:00', '2025-01-06 11:00:00'),
('H00029', 'PA0029', 'RO0029', '2024-12-29 11:00:00', '2025-01-07 12:00:00'),
('H00030', 'PA0030', 'RO0030', '2024-12-30 12:00:00', '2025-01-08 13:00:00');




CREATE Trigger t1 ON ROOM
FOR INSERT
AS
BEGIN
		UPDATE ROOM 
		SET EmptyBed = BedCount
END


CREATE Trigger t2 ON ROOM
FOR UPDATE
AS
BEGIN
		IF UPDATE(EmptyBed) AND @@NestLevel = 1
				BEGIN
					print 'Khong duoc tu y sua EmptyBed'
					ROLLBACK TRAN
				END
END



CREATE Trigger t3 ON NURSECARE 
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
		UPDATE ROOM
		SET EmptyBed =BedCount - (SELECT COUNT (*)
						FROM NURSECARE N
						WHERE ROOM.RoomID = N.RoomID)
END





--Ngày sinh nhân viên nhỏ hơn ngày gia nhập--
ALTER TABLE STAFF
ADD CONSTRAINT CK_DOB_DOJ CHECK (DateOfBirth < DateOfJoining)

--Salary của nhân viên lớn hơn 0--
ALTER TABLE STAFF
ADD CONSTRAINT CK_SALARY CHECK (Salary > 0)



--Kiểm tra số nhân viên của 1 KHOA--
CREATE Trigger t5 ON DEPARTMENT
FOR INSERT
AS
BEGIN
		IF EXISTS (SELECT 1
					FROM inserted 
					WHERE EmployeeNumber <> 0 )
			BEGIN
				print N'Không được khởi tạo số lượng nhân viên khác 0 cho KHOA'
				ROLLBACK TRAN
			END
END



CREATE Trigger t6 ON DEPARTMENT
FOR UPDATE
AS
BEGIN
		IF UPDATE(EmployeeNumber) AND @@NestLevel = 1
			BEGIN
				print N'Không được tự ý sửa EmployeeNumber'
				ROLLBACK TRAN
			END
END 


CREATE Trigger t7 ON STAFF
FOR INSERT, DELETE, UPDATE
AS
BEGIN
		UPDATE DEPARTMENT
		SET EmployeeNumber = (SELECT COUNT(*)
								FROM STAFF S
								WHERE DEPARTMENT.DepartmentID = S.DepartmentID )
END





---------Tính tiền hoá đơn khi thêm/sửa/xoá CTHD
CREATE TRIGGER TOTALMONEY_BD_IU
ON BILLDETAIL
AFTER INSERT, DELETE, UPDATE
AS
	UPDATE BILL
	SET Total = (SELECT SUM(Amount*Price) 
				FROM BILLDETAIL b JOIN MEDICATION m ON b.MedicationID = m.MedicationID 
				WHERE BILL.TransactionID=b.TransactionID)



----Khi tạo mới hoặc sửa số lượng thuốc trong BillDetail thì cập nhật số lượng tồn kho
CREATE TRIGGER QUANTITYINSTOCK_MEDICATION_BILL
ON BILLDETAIL
AFTER INSERT, UPDATE
AS
	UPDATE MEDICATION
	SET QuantityInStock = QuantityInStock + (SELECT SUM(d.Amount)               
											FROM deleted d JOIN MEDICATION m1 
											ON d.MedicationID = m1.MedicationID)
	WHERE MedicationID IN (SELECT MedicationID FROM deleted)

	IF (EXISTS(SELECT QuantityInStock FROM MEDICATION m JOIN inserted i ON m.MedicationID = i.MedicationID WHERE i.Amount > QuantityInStock))
	BEGIN
		ROLLBACK TRAN
		PRINT N'Số lượng thuốc tồn kho không đủ!'
	END
	ELSE
	BEGIN
		UPDATE MEDICATION
		SET QuantityInStock = QuantityInStock - (SELECT SUM(i.Amount)               
												FROM inserted i JOIN MEDICATION m2 
												ON i.MedicationID = m2.MedicationID)
		WHERE MedicationID IN (SELECT MedicationID FROM inserted)
	END




---- Kiểm tra ngày tháng
ALTER TABLE MEDICATION
ADD CONSTRAINT CHK_DATE_MEDICATION CHECK (ExpiryDate > ManufacturingDate)



ALTER TABLE STAFF
ADD CONSTRAINT CHK_DATE_STAFF CHECK (DateOfBirth < DateOfJoining)



-----Ngày hoá đơn phải lớn hơn ngày khám bệnh
CREATE TRIGGER CHK_TransactionDate_BILL_MEDICALRECORD
ON BILL
AFTER INSERT, UPDATE
AS 
	IF (EXISTS(SELECT * FROM inserted i JOIN MEDICALRECORD m ON i.RecordID = m.RecordID WHERE i.TransactionDate < m.VisitDate))
	BEGIN
		ROLLBACK TRAN
	END


----------------Giá thuốc phải > 0
ALTER TABLE MEDICATION
ADD CONSTRAINT CHK_PRICE_MEDICATION CHECK (Price > 0)

------Kiểm tra giường
ALTER TABLE ROOM
ADD CONSTRAINT CK_BED_ROOM CHECK (BedCount > 0 AND EmptyBed >= 0)



CREATE TRIGGER CreateAccount_USERLOGIN
ON STAFF   
AFTER INSERT
AS
BEGIN
    INSERT INTO USERLOGIN (UserID, Pass)
    SELECT StaffID, '1'
    FROM inserted
    WHERE TypeOfStaff LIKE N'ADMIN'
          OR TypeOfStaff LIKE N'%Bác sĩ%'
          OR TypeOfStaff LIKE N'%Điều dưỡng%'
          OR TypeOfStaff LIKE N'%Dược sĩ%'
          OR TypeOfStaff LIKE N'%Kế toán%';
	END;



