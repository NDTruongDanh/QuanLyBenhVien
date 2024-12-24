DROP DATABASE HospitalDB

CREATE DATABASE HospitalDB

USE HospitalDB

SET DATEFORMAT YMD  --Thiết lập DATE theo thứ tự ngày->tháng->năm

--Thống nhất tất cả các ID đều có đúng 6 ký tự bao gồm 2 chữ 4 số--
CREATE TABLE PATIENT (
    PatientID CHAR(6) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(15),
    AddressPatient NVARCHAR(255),
    Email VARCHAR(255),
    AdmissionDate DATE,
    DischargeDate DATE
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

SELECT * FROM NURSECARE

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
    RoomType NVARCHAR(50)  -- Loại phòng: thường, VIP, hồi sức, v.v.
)


CREATE TABLE WEEKLYASSIGNMENT (
    AssignmentID CHAR(6) PRIMARY KEY,
    StaffID CHAR(6),
    DepartmentID VARCHAR(6),
    WeekStartDate DATE,
    WeekEndDate DATE,
    ShiftType NVARCHAR(50)  -- Loại ca: Sáng, Chiều, Tối
)

CREATE TABLE USERLOGIN(
	UserID VARCHAR(6) PRIMARY KEY,
	Pass VARCHAR(20) NOT NULL,
	FLAG SMALLINT DEFAULT(0)
)


--THÊM KHOÁ NGOẠI--
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

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_ST FOREIGN KEY (NurseID) REFERENCES STAFF(StaffID)

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_PT FOREIGN KEY (PatientID) REFERENCES PATIENT(PatientID)

ALTER TABLE NURSECARE
ADD CONSTRAINT FK_N_R FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID)





-- Chèn dữ liệu vào bảng PATIENT
INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate)
VALUES 
('PA0001', N'Nguyễn Văn A', '1990-05-01', N'Nam', '0901234567', N'Hà Nội', 'a.nguyen@example.com', '2024-12-01', '2024-12-05'),
('PA0002', N'Phạm Thị B', '1985-10-15', N'Nữ', '0902345678', N'Đà Nẵng', 'b.pham@example.com', '2024-12-02', '2024-12-06'),
('PA0003', N'Ngô Văn C', '2000-07-20', N'Nam', '0903456789', N'Hồ Chí Minh', 'c.ngo@example.com', '2024-12-03', '2024-12-07'),
('PA0004', N'Trần Thị D', '1992-02-10', N'Nữ', '0904567890', N'Quảng Ninh', 'd.tran@example.com', '2024-12-04', '2024-12-08'),
('PA0005', N'Lê Minh E', '1980-11-11', N'Nam', '0905678901', N'Bình Dương', 'e.le@example.com', '2024-12-05', '2024-12-09'),
('PA0006', N'Vũ Thị F', '1995-03-22', N'Nữ', '0906789012', N'Vũng Tàu', 'f.vu@example.com', '2024-12-06', '2024-12-10');

-- Chèn dữ liệu vào bảng STAFF
INSERT INTO STAFF (StaffID, FullName, TypeOfStaff, Gender, DateOfBirth, PhoneNumber, DateOfJoining, Email, Salary, DepartmentID)
VALUES
('ST0001', N'Nguyễn Thiện G', N'Bác sĩ Đa khoa', N'Nam', '1980-01-15', '0912345678', '2020-05-01', 'g.nguyen@example.com', 15000000, 'DP0001'),
('ST0002', N'Phạm Minh H', N'Kế toán', N'Nữ', '1990-06-20', '0913456789', '2022-07-01', 'h.pham@example.com', 10000000, 'DP0002'),
('ST0003', N'Lê Thị I', N'Y tá', N'Nữ', '1985-09-30', '0914567890', '2018-08-15', 'i.le@example.com', 20000000, 'DP0003'),
('ST0004', N'Vũ Thái J', N'Kế toán', N'Nam', '1978-12-25', '0915678901', '2015-11-20', 'j.vu@example.com', 18000000, 'DP0004'),
('ST0005', N'Trần Minh K', N'Điều dưỡng Tổng quát', N'Nam', '1992-05-05', '0916789012', '2023-09-10', 'k.tran@example.com', 11000000, 'DP0005'),
('ST0006', N'Võ Quang L', N'Bác sĩ Ngoại khoa', N'Nam', '1982-03-11', '0917890123', '2019-06-01', 'l.vo@example.com', 17000000, 'DP0006'),
('ST0007', N'Võ Quang A', N'Dược sĩ', N'Nam', '1983-03-14', '0917890127', '2020-06-02', 'a.vo@example.com', 28000000, 'DP0001'),
('ST0008', N'Võ Quang B', N'Dược sĩ', N'Nam', '1985-03-11', '0917890121', '2018-07-01', 'b.vo@example.com', 17500000, 'DP0002');

-- Chèn dữ liệu vào bảng DEPARTMENT
INSERT INTO DEPARTMENT (DepartmentID, DepartmentName, EmployeeNumber, HeadDepartmentID, PhoneNumber, LocationDPM)
VALUES
('DP0001', N'Khoa Nội', 2, 'ST0007', '0241234567', N'Hà Nội'),
('DP0002', N'Khoa Ngoại', 2, 'ST0002', '0242345678', N'Đà Nẵng'),
('DP0003', N'Khoa Cấp Cứu', 1, 'ST0003', '0243456789', N'Hồ Chí Minh'),
('DP0004', N'Khoa Nhi', 1, 'ST0004', '0244567890', N'Quảng Ninh'),
('DP0005', N'Khoa Sản', 1, 'ST0005', '0245678901', N'Bình Dương'),
('DP0006', N'Khoa Tim Mạch', 1, 'ST0006', '0246789012', N'Vũng Tàu');

-- Chèn dữ liệu vào bảng APPOINTMENT
INSERT INTO APPOINTMENT (AppointmentID, PatientID, DoctorID, DepartmentID, AppointmentDateTime, AppointmentStatus)
VALUES
('AP0001', 'PA0001', 'ST0001', 'DP0001', '2024-12-01 08:00:00', N'Chấp thuận'),
('AP0002', 'PA0002', 'ST0006', 'DP0002', '2024-12-02 09:00:00', N'Đang chờ xử lý'),
('AP0003', 'PA0003', 'ST0007', 'DP0003', '2024-12-03 10:00:00', N'Chấp thuận'),
('AP0004', 'PA0004', 'ST0008', 'DP0004', '2024-12-04 11:00:00', N'Từ chối'),
('AP0005', 'PA0005', 'ST0001', 'DP0005', '2024-12-05 12:00:00', N'Đang chờ xử lý'),
('AP0006', 'PA0006', 'ST0006', 'DP0006', '2024-12-06 13:00:00', N'Từ chối');


-- Chèn dữ liệu vào bảng MEDICALRECORD
INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan)
VALUES
('MR0001', 'PA0001', 'ST0001', '2024-12-01', N'Viêm phổi', N'X-ray bình thường', N'Điều trị kháng sinh'),
('MR0002', 'PA0003', 'ST0007', '2024-12-03', N'Đau dạ dày', N'Siêu âm bình thường', N'Thuốc giảm đau'),
('MR0003', 'PA0004', 'ST0008', '2024-12-04', N'Cảm cúm', N'Xét nghiệm máu', N'Thức ăn nhẹ, uống nước nhiều'),
('MR0004', 'PA0006', 'ST0006', '2024-12-06', N'Bệnh tim mạch', N'ECG bình thường', N'Thuốc tim mạch')

-- Chèn dữ liệu vào bảng BILL
INSERT INTO BILL (TransactionID, RecordID, StaffID, TransactionDate, PaymentMethod, Total)
VALUES
('BI0001', 'MR0001', 'ST0004', '2024-12-01', N'Tiền mặt', 340000),
('BI0002', 'MR0002', 'ST0002', '2024-12-03', N'Thẻ tín dụng', 120000),
('BI0003', 'MR0003', 'ST0002', '2024-12-04', N'Bảo hiểm', 380000),
('BI0004', 'MR0004', 'ST0004', '2024-12-06', N'Tiền mặt', 280000)

-- Chèn dữ liệu vào bảng BILLDETAIL
INSERT INTO BILLDETAIL (TransactionID,MedicationID, MedicationName, Amount)
VALUES
('BI0001', 'ME0001', 'Paracetamol', 2),
('BI0001', 'ME0002', 'Amoxicillin',3),
('BI0002', 'ME0003', 'Ciprofloxacin',1),
('BI0003', 'ME0004', 'Ibuprofen',5),
('BI0003', 'ME0005', 'Omeprazole',2),
('BI0004', 'ME0006', 'Diclofenac',4);



-- Chèn dữ liệu vào bảng MEDICATION
INSERT INTO MEDICATION (MedicationID, MedicationName, Dosage, DosageUnit, Category, QuantityInStock, Price, ExpiryDate, ManufacturingDate, Manufacturer)
VALUES
('ME0001', N'Paracetamol', N'500mg','chai', N'Giảm đau', 100, 50000, '2025-12-01', '2024-01-01', N'Việt Nam'),
('ME0002', N'Amoxicillin', N'250mg',N'viên', N'Kháng sinh', 50, 80000, '2025-06-01', '2024-02-01', N'Ấn Độ'),
('ME0003', N'Ciprofloxacin', N'500mg',N'ống', N'Kháng sinh', 70, 120000, '2025-09-01', '2024-03-01', N'Mỹ'),
('ME0004', N'Ibuprofen', N'400mg',N'gói', N'Giảm đau', 150, 60000, '2025-07-01', '2024-04-01', N'Anh'),
('ME0005', N'Omeprazole', N'20mg',N'gói', N'Tiêu hóa', 200, 40000, '2025-11-01', '2024-05-01', N'Nhật Bản'),
('ME0006', N'Diclofenac', N'50mg',N'viên', N'Giảm đau', 120, 70000, '2025-08-01', '2024-06-01', N'Trung Quốc');


-- Chèn dữ liệu vào bảng ROOM
INSERT INTO ROOM (RoomID, DepartmentID, BedCount, RoomType)
VALUES
('RO0001', 'DP0001', 10, N'Điều trị tổng quát'),
('RO0002', 'DP0002', 12, N'VIP'),
('RO0003', 'DP0003', 8, N'Hồi sức'),
('RO0004', 'DP0004', 6, N'Điều trị tổng quát'),
('RO0005', 'DP0005', 4, N'VIP'),
('RO0006', 'DP0006', 5, N'Sản khoa');


-- Chèn dữ liệu vào bảng WEEKLYASSIGNMENT
INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, DepartmentID, WeekStartDate, WeekEndDate, ShiftType)
VALUES
('WA0001', 'ST0001', 'DP0001', '2024-12-01', '2024-12-07', N'Sáng'),
('WA0002', 'ST0002', 'DP0002', '2024-12-01', '2024-12-07', N'Chiều'),
('WA0003', 'ST0003', 'DP0003', '2024-12-01', '2024-12-07', N'Tối'),
('WA0004', 'ST0004', 'DP0004', '2024-12-01', '2024-12-07', N'Sáng'),
('WA0005', 'ST0005', 'DP0005', '2024-12-01', '2024-12-07', N'Chiều'),
('WA0006', 'ST0006', 'DP0006', '2024-12-01', '2024-12-07', N'Tối');

CREATE TRIGGER TOTALMONEY_BD_IU
ON BILLDETAIL
AFTER INSERT, DELETE, UPDATE
AS
	UPDATE BILL
	SET Total = (SELECT SUM(Amount*Price) 
				FROM BILLDETAIL b JOIN MEDICATION m ON b.MedicationID = m.MedicationID 
				WHERE BILL.TransactionID=b.TransactionID)


--Them data cho patient va medical 

INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate)
VALUES
('PA0007', N'Hồng Hài N', '1999-04-12', N'Nữ', '0962456786', N'Hải Phòng', 'h.hong@example.com', '2023-03-08', NULL),
('PA0008', N'Đỗ Thị H', '1999-04-12', N'Nữ', '0962456786', N'Hải Phòng', 'h.do@example.com', '2024-12-08', NULL),
('PA0009', N'Ngô Minh I', '1975-05-10', N'Nam', '0983456785', N'Lào Cai', 'i.ngo@example.com', '2024-11-27', '2024-12-09'),
('PA0010', N'Thái Hồng K', '2000-10-25', N'Nữ', '0943456784', N'Hà Giang', 'k.thai@example.com', '2024-12-10', NULL),
('PA0011', N'Nguyễn Văn A', '1985-06-12', N'Nam', '0987654321', N'Hà Nội', 'a.nguyen@example.com', '2024-11-20', '2024-12-05'),
('PA0012', N'Lê Thị B', '1990-03-15', N'Nữ', '0978654322', N'Đà Nẵng', 'b.le@example.com', '2024-12-01', NULL),
('PA0013', N'Phạm Minh C', '2002-09-20', N'Nam', '0902456789', N'Hồ Chí Minh', 'c.pham@example.com', '2024-11-25', NULL),
('PA0014', N'Trần Văn D', '1978-11-10', N'Nam', '0945654321', N'Cần Thơ', 'd.tran@example.com', '2024-11-30', '2024-12-10'),
('PA0015', N'Hoàng Thị E', '1987-02-07', N'Nữ', '0913456789', N'Bắc Ninh', 'e.hoang@example.com', '2024-12-02', NULL),
('PA0016', N'Vũ Ngọc F', '1995-08-17', N'Nữ', '0932456788', N'Nam Định', 'f.vu@example.com', '2024-11-28', '2024-12-08'),
('PA0017', N'Bùi Thanh G', '1982-12-30', N'Nam', '0923456787', N'Quảng Ninh', 'g.bui@example.com', '2024-12-05', NULL)

INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan)
VALUES
('MR0005', 'PA0005', 'ST0002', '2024-12-03', N'Dau dạ dày', N'Nội soi: loét dạ dày', N'Dùng thuốc giảm tiết axit, kiêng đồ cay'),
('MR0006', 'PA0006', 'ST0004', '2024-11-29', N'Đau đầu mãn tính', N'CT scan không phát hiện bất thường', N'Dùng thuốc giảm đau, giảm căng thẳng'),
('MR0007', 'PA0007', 'ST0001', '2024-12-06', N'Cảm lạnh', N'Không có dấu hiệu nguy hiểm', N'Uống vitamin C và nghỉ ngơi'),
('MR0008', 'PA0008', 'ST0005', '2024-12-09', N'Suy nhược cơ thể', N'Chỉ số máu thấp', N'Tăng cường dinh dưỡng, vitamin'),
('MR0009', 'PA0009', 'ST0003', '2024-11-28', N'Hen suyễn', N'Chỉ số phổi giảm', N'Dùng thuốc hít và theo dõi định kỳ'),
('MR0010', 'PA0010', 'ST0004', '2024-12-11', N'Đau lưng', N'X-quang: thoái hóa cột sống', N'Vật lý trị liệu 3 tuần'),
('MR0011', 'PA0011', 'ST0001', '2024-11-21', N'Cảm cúm', N'Huyết áp bình thường, không có triệu chứng nặng', N'Nghỉ ngơi, uống thuốc giảm đau'),
('MR0012', 'PA0012', 'ST0002', '2024-12-02', N'Sốt xuất huyết', N'Giảm tiểu cầu, sốt cao', N'Nhập viện, theo dõi hàng ngày'),
('MR0013', 'PA0003', 'ST0001', '2024-11-26', N'Viêm phổi', N'X-quang phổi phát hiện tổn thương nhỏ', N'Dùng kháng sinh 7 ngày'),
('MR0014', 'PA0004', 'ST0003', '2024-12-01', N'Gãy tay phải', N'Chụp X-quang: gãy xương quay', N'Bó bột và nghỉ ngơi 4 tuần'),
('MR0015', 'PA0001', 'ST0001', '2024-01-15', N'Sốt xuất huyết', N'Tiểu cầu thấp, sốt cao', N'Nhập viện, truyền dịch, điều trị sốt'),
('MR0016', 'PA0002', 'ST0002', '2024-02-20', N'Viêm phổi', N'Chụp X-quang phát hiện viêm phổi nhẹ', N'Dùng kháng sinh, theo dõi nhiệt độ'),
('MR0017', 'PA0003', 'ST0003', '2024-03-10', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, mất nước nhẹ', N'Uống thuốc chống tiêu chảy, bù nước'),
('MR0018', 'PA0004', 'ST0001', '2024-04-05', N'Sốt xuất huyết', N'Tiểu cầu giảm mạnh', N'Truyền dịch và thuốc hạ sốt'),
('MR0019', 'PA0005', 'ST0002', '2024-05-11', N'Viêm phổi', N'Phổi tổn thương nhẹ', N'Kháng sinh, nghỉ ngơi tại nhà'),
('MR0020', 'PA0006', 'ST0004', '2024-06-21', N'Tiêu chảy do nhiễm khuẩn', N'Phân có máu, sốt cao', N'Thực hiện xét nghiệm, uống thuốc kháng sinh'),
('MR0021', 'PA0007', 'ST0003', '2024-07-07', N'Sốt xuất huyết', N'Huyết áp ổn định, tiểu cầu thấp', N'Nhập viện, theo dõi sức khỏe'),
('MR0022', 'PA0008', 'ST0005', '2024-08-18', N'Viêm phổi', N'Khó thở, ho nhiều', N'Kháng sinh và điều trị hỗ trợ'),
('MR0023', 'PA0009', 'ST0002', '2024-09-05', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, không có máu', N'Uống thuốc kháng sinh, bù nước'),
('MR0024', 'PA0010', 'ST0004', '2024-10-12', N'Sốt xuất huyết', N'Phát ban, sốt cao', N'Chăm sóc tại bệnh viện, điều trị triệu chứng'),
('MR0025', 'PA0011', 'ST0001', '2024-11-22', N'Viêm phổi', N'Chụp X-quang phát hiện phổi viêm', N'Dùng kháng sinh, nghỉ ngơi'),
('MR0026', 'PA0012', 'ST0002', '2024-12-15', N'Tiêu chảy do nhiễm khuẩn', N'Mất nước nhẹ, sốt', N'Bù nước, dùng thuốc kháng sinh'),
('MR0027', 'PA0013', 'ST0003', '2024-01-02', N'Sốt xuất huyết', N'Tiểu cầu giảm, huyết áp thấp', N'Truyền dịch, hạ sốt'),
('MR0028', 'PA0014', 'ST0005', '2024-02-17', N'Viêm phổi', N'Khó thở nhẹ, X-quang không phát hiện bất thường', N'Kháng sinh, chăm sóc tại nhà'),
('MR0029', 'PA0015', 'ST0004', '2024-03-13', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, mất nước nhẹ', N'Uống thuốc kháng sinh, bù nước'),
('MR0030', 'PA0016', 'ST0001', '2024-04-09', N'Sốt xuất huyết', N'Tiểu cầu giảm, sốt cao', N'Nhập viện, truyền dịch, thuốc hạ sốt'),
('MR0031', 'PA0017', 'ST0002', '2024-05-17', N'Viêm phổi', N'Phổi có dấu hiệu viêm nhẹ', N'Kháng sinh, uống thuốc giảm ho'),
('MR0032', 'PA0011', 'ST0003', '2024-06-02', N'Tiêu chảy do nhiễm khuẩn', N'Phân lỏng, sốt nhẹ', N'Uống thuốc kháng sinh, bù nước'),
('MR0033', 'PA0012', 'ST0005', '2024-07-25', N'Sốt xuất huyết', N'Tiểu cầu thấp, xuất huyết dưới da', N'Truyền dịch, điều trị hỗ trợ'),
('MR0034', 'PA0013', 'ST0004', '2024-08-11', N'Viêm phổi', N'Khó thở nhẹ, X-quang phát hiện viêm phổi nhẹ', N'Dùng thuốc kháng sinh và thuốc giảm ho');


INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan)
VALUES
('MR0035', 'PA0002', 'ST0002', '2024-11-30', N'Covid-19', N'Xét nghiệm PCR dương tính, sốt, khó thở', N'Chăm sóc tại nhà, uống thuốc hạ sốt'),
('MR0036', 'PA0003', 'ST0003', '2024-12-01', N'Viêm phổi', N'Khó thở, ho có đờm, đau ngực', N'Dùng kháng sinh, nghỉ ngơi'),
('MR0037', 'PA0004', 'ST0001', '2024-12-02', N'Cảm cúm', N'Sốt cao, đau đầu, nghẹt mũi', N'Uống thuốc giảm đau, xịt mũi'),
('MR0038', 'PA0005', 'ST0002', '2024-12-03', N'Covid-19', N'Khó thở, ho, mệt mỏi', N'Chăm sóc tại nhà, điều trị triệu chứng'),
('MR0039', 'PA0006', 'ST0004', '2024-12-04', N'Viêm phổi', N'Phổi tổn thương nhẹ, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'),
('MR0040', 'PA0007', 'ST0003', '2024-12-05', N'Cảm cúm', N'Ho, đau họng, sốt nhẹ', N'Uống thuốc giảm đau, uống nhiều nước'),
('MR0041', 'PA0008', 'ST0005', '2024-12-06', N'Covid-19', N'Xét nghiệm PCR dương tính, mệt mỏi', N'Chăm sóc tại nhà, điều trị hỗ trợ'),
('MR0042', 'PA0009', 'ST0002', '2024-12-07', N'Viêm phổi', N'Khó thở, ho nhiều, đau ngực', N'Kháng sinh, theo dõi tại bệnh viện'),
('MR0043', 'PA0010', 'ST0004', '2024-12-08', N'Cảm cúm', N'Sốt cao, ho, đau cơ', N'Uống thuốc hạ sốt, nghỉ ngơi'),
('MR0044', 'PA0011', 'ST0001', '2024-12-09', N'Covid-19', N'Mệt mỏi, ho, sốt nhẹ', N'Chăm sóc tại nhà, thuốc giảm sốt'),
('MR0045', 'PA0012', 'ST0002', '2024-12-10', N'Viêm phổi', N'Phổi có dấu hiệu viêm, ho, khó thở', N'Kháng sinh, nghỉ ngơi tại bệnh viện'),
('MR0046', 'PA0013', 'ST0003', '2024-12-11', N'Cảm cúm', N'Ho, sốt, đau họng', N'Uống thuốc hạ sốt, xịt mũi'),
('MR0047', 'PA0014', 'ST0005', '2024-12-12', N'Covid-19', N'Mệt mỏi, khó thở nhẹ', N'Chăm sóc tại nhà, thuốc giảm đau'),
('MR0048', 'PA0015', 'ST0004', '2024-12-13', N'Viêm phổi', N'Khó thở, đau ngực, ho khan', N'Kháng sinh, nghỉ ngơi tại bệnh viện'),
('MR0049', 'PA0016', 'ST0001', '2024-12-14', N'Cảm cúm', N'Sốt, ho, mệt mỏi', N'Uống thuốc giảm đau, uống nhiều nước'),
('MR0050', 'PA0017', 'ST0002', '2024-12-14', N'Covid-19', N'Mệt mỏi, khó thở nhẹ, ho', N'Chăm sóc tại nhà, điều trị triệu chứng'),
('MR0051', 'PA0001', 'ST0001', '2024-11-30', N'Cảm cúm', N'Sốt, ho khan, mệt mỏi', N'Ist, uống thuốc hạ sốt, nghỉ ngơi');

-- Insert sample data into the APPOINTMENT table
INSERT INTO APPOINTMENT (AppointmentID, PatientID, DoctorID, DepartmentID, AppointmentDateTime, AppointmentStatus)
VALUES
('A00001', 'P00001', 'D00001', 'DEP001', GETDATE() - 2, 'Confirmed'), -- 2 days ago
('A00002', 'P00002', 'D00002', 'DEP002', GETDATE() + 1, 'Confirmed'), -- Tomorrow
('A00003', 'P00003', 'D00003', 'DEP003', GETDATE() + 3, 'Cancelled'), -- 3 days later
('A00004', 'P00004', 'D00001', 'DEP004', GETDATE() - 6, 'Completed'), -- Last week
('A00005', 'P00005', 'D00002', 'DEP005', '2024-12-16', 'Confirmed'), -- Current week's Monday
('A00006', 'P00006', 'D00003', 'DEP006', '2024-12-22', 'Completed'); -- Current week's Sunday

INSERT INTO APPOINTMENT (AppointmentID, PatientID, DoctorID, DepartmentID, AppointmentDateTime, AppointmentStatus)
VALUES
('A00007', 'P00005', 'D00002', 'DEP005', '2024-12-15', 'Confirmed'),
('A00008', 'P00005', 'D00002', 'DEP005', '2024-12-24', 'Confirmed'),
('A00009', 'P00005', 'D00002', 'DEP005', '2024-12-23', 'Confirmed'),
('A00010', 'P00005', 'D00002', 'DEP005', '2024-12-17', 'Confirmed'),
('A00011', 'P00005', 'D00002', 'DEP005', '2024-12-20', 'Confirmed'),
('A00012', 'P00005', 'D00002', 'DEP005', '2024-12-14', 'Confirmed')


DELETE FROM APPOINTMENT
WHERE AppointmentID IN ('A00005', 'A00006')

ALTER TABLE APPOINTMENT
DROP CONSTRAINT FK_AP_DE

SELECT * FROM APPOINTMENT

SELECT AppointmentDateTime, FullName
FROM APPOINTMENT a JOIN PATIENT p ON a.PatientID = p.PatientID
WHERE AppointmentDateTime > DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- Start of the week (Monday)
  AND AppointmentDateTime <= DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) -- End of the week (Sunday);
ORDER BY AppointmentDateTime

SELECT *

SELECT st.StaffID, FullName , ShiftType, WeekStartDate, WeekEndDate 
                               FROM WEEKLYASSIGNMENT w JOIN STAFF st ON w.StaffID = st.StaffID
                               WHERE WeekStartDate > DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) 
                                      AND WeekEndDate <= DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
	                                  AND w.DepartmentID IN (SELECT st1.DepartmentID FROM STAFF st1 WHERE st1.StaffID = 'ST0001')
                               ORDER BY ShiftType, WeekStartDate

							   SELECT * FROM STAFF
							   WHERE DepartmentID = 'DP0001'

							   SELECT * FROM WEEKLYASSIGNMENT
							   WHERE DepartmentID = 'DP0001'

INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, DepartmentID, WeekStartDate, WeekEndDate, ShiftType)
VALUES
('A00001', 'ST0001', 'DP0001', '2024-12-17', '2024-12-21', N'Sáng'),  
('A00002', 'ST0007', 'DP0001', '2024-12-16', '2024-12-22', N'Sáng'), 
('A00003', 'ST0001', 'DP0001', '2024-12-17', '2024-12-21', N'Chiều') 

-- Example data for NURSECARE table
INSERT INTO NURSECARE (CareID, NurseID, PatientID, RoomID, CareDateTime, CareType, Notes)
VALUES 
('C00001', 'N00001', 'P00001', 'R0001', '2024-12-20 09:00:00', 'Medication Administration', 'Administered antibiotics as prescribed.'),
('C00002', 'N00002', 'P00002', 'R0002', '2024-12-20 11:30:00', 'Wound Dressing', 'Changed dressing and cleaned wound. Healing well.'),
('C00003', 'N00003', 'P00003', 'R0003', '2024-12-20 14:00:00', 'Vital Signs Check', 'Blood pressure: 120/80, Temperature: 37°C.'),
('C00004', 'N00001', 'P00004', 'R0001', '2024-12-21 08:00:00', 'Patient Hygiene', 'Assisted patient with morning hygiene routine.'),
('C00005', 'N00004', 'P00005', 'R0004', '2024-12-21 10:15:00', 'IV Drip Monitoring', 'Checked IV line and adjusted flow rate as needed.'),
('C00006', 'N00002', 'P00006', 'R0005', '2024-12-21 13:30:00', 'Nutrition Assistance', 'Helped patient with lunch and ensured adequate hydration.'),
('C00007', 'N00005', 'P00007', 'R0006', '2024-12-21 15:45:00', 'Patient Education', 'Explained post-discharge care and medication regimen.'),
('C00008', 'N00003', 'P00008', 'R0003', '2024-12-22 09:00:00', 'Pain Management', 'Provided prescribed painkillers after confirming with the doctor.'),
('C00009', 'N00001', 'P00009', 'R0001', '2024-12-22 11:00:00', 'Patient Monitoring', 'Monitored patient’s vitals during post-surgery recovery.'),
('C00010', 'N00004', 'P00010', 'R0004', '2024-12-22 14:30:00', 'Emergency Response', 'Responded to a sudden drop in blood pressure, stabilized the patient.');

INSERT INTO USERLOGIN (UserID,Pass) VALUES ('admin','1') ---ADMIN
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('N00001','1') ----Y tá
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0001','1') ---- Bác sĩ
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0002','1') ---- Kế toán
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0007','1') ---- Dược sĩ (trưởng khoa)
INSERT INTO USERLOGIN (UserID,Pass) VALUES ('ST0008','1') ---- Dược sĩ




INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate)
VALUES
    ('P00001', N'Nguyễn Văn A', '1985-01-15', N'Nam', '0912345678', N'123 Đường ABC, Quận 1, TP.HCM', 'nguyenvana@example.com', '2024-12-01', NULL),
    ('P00002', N'Trần Thị B', '1990-02-25', N'Nữ', '0923456789', N'45 Đường DEF, Quận 2, TP.HCM', 'tranthib@example.com', '2024-12-02', NULL),
    ('P00003', N'Lê Văn C', '1980-03-10', N'Nam', '0934567890', N'78 Đường GHI, Quận 3, TP.HCM', 'levanc@example.com', '2024-12-03', NULL),
    ('P00004', N'Hoàng Thị D', '2000-04-05', N'Nữ', '0945678901', N'90 Đường JKL, Quận 4, TP.HCM', 'hoangthid@example.com', '2024-12-04', NULL),
    ('P00005', N'Phạm Văn E', '1995-05-20', N'Nam', '0956789012', N'56 Đường MNO, Quận 5, TP.HCM', 'phamvane@example.com', '2024-12-05', NULL),
    ('P00006', N'Nguyễn Thị F', '1988-06-15', N'Nữ', '0967890123', N'34 Đường PQR, Quận 6, TP.HCM', 'nguyenthif@example.com', '2024-12-06', NULL),
    ('P00007', N'Đinh Văn G', '1992-07-30', N'Nam', '0978901234', N'12 Đường STU, Quận 7, TP.HCM', 'dinhvang@example.com', '2024-12-07', NULL),
    ('P00008', N'Bùi Thị H', '2002-08-20', N'Nữ', '0989012345', N'67 Đường VWX, Quận 8, TP.HCM', 'buithih@example.com', '2024-12-08', NULL),
    ('P00009', N'Trịnh Văn I', '1985-09-12', N'Nam', '0990123456', N'89 Đường YZ, Quận 9, TP.HCM', 'trinhvani@example.com', '2024-12-09', NULL),
    ('P00010', N'Lý Thị K', '1998-10-05', N'Nữ', '0901234567', N'23 Đường ABC, Quận 10, TP.HCM', 'lythik@example.com', '2024-12-10', NULL);