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
    DischargeDate DATE,
    RoomID CHAR(6)
)

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


--THÊM KHOÁ NGOẠI--
ALTER TABLE PATIENT
ADD CONSTRAINT FK_PA_RO FOREIGN KEY (RoomID) REFERENCES ROOM(RoomID) 

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



ALTER TABLE ROOM
ADD CONSTRAINT FK_RO_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_STA FOREIGN KEY (StaffID) REFERENCES STAFF(StaffID) 

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 




-- Chèn dữ liệu vào bảng PATIENT
INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate, RoomID)
VALUES 
('PA0001', N'Nguyễn Văn A', '1990-05-01', N'Nam', '0901234567', N'Hà Nội', 'a.nguyen@example.com', '2024-12-01', '2024-12-05', 'RO0001'),
('PA0002', N'Phạm Thị B', '1985-10-15', N'Nữ', '0902345678', N'Đà Nẵng', 'b.pham@example.com', '2024-12-02', '2024-12-06', 'RO0002'),
('PA0003', N'Ngô Văn C', '2000-07-20', N'Nam', '0903456789', N'Hồ Chí Minh', 'c.ngo@example.com', '2024-12-03', '2024-12-07', 'RO0003'),
('PA0004', N'Trần Thị D', '1992-02-10', N'Nữ', '0904567890', N'Quảng Ninh', 'd.tran@example.com', '2024-12-04', '2024-12-08', 'RO0004'),
('PA0005', N'Lê Minh E', '1980-11-11', N'Nam', '0905678901', N'Bình Dương', 'e.le@example.com', '2024-12-05', '2024-12-09', 'RO0005'),
('PA0006', N'Vũ Thị F', '1995-03-22', N'Nữ', '0906789012', N'Vũng Tàu', 'f.vu@example.com', '2024-12-06', '2024-12-10', 'RO0006');

-- Chèn dữ liệu vào bảng STAFF
INSERT INTO STAFF (StaffID, FullName, TypeOfStaff, Gender, DateOfBirth, PhoneNumber, DateOfJoining, Email, Salary, DepartmentID)
VALUES
('ST0001', N'Nguyễn Thiện G', N'Bác sĩ Đa khoa', N'Nam', '1980-01-15', '0912345678', '2020-05-01', 'g.nguyen@example.com', 15000000, 'DP0001'),
('ST0002', N'Phạm Minh H', N'Kế toán', N'Nữ', '1990-06-20', '0913456789', '2022-07-01', 'h.pham@example.com', 10000000, 'DP0002'),
('ST0003', N'Lê Thị I', N'Quản lý', N'Nữ', '1985-09-30', '0914567890', '2018-08-15', 'i.le@example.com', 20000000, 'DP0003'),
('ST0004', N'Vũ Thái J', N'Kế toán', N'Nam', '1978-12-25', '0915678901', '2015-11-20', 'j.vu@example.com', 18000000, 'DP0004'),
('ST0005', N'Trần Minh K', N'Điều dưỡng Tổng quát', N'Nam', '1992-05-05', '0916789012', '2023-09-10', 'k.tran@example.com', 11000000, 'DP0005'),
('ST0006', N'Võ Quang L', N'Bác sĩ Ngoại khoa', N'Nam', '1982-03-11', '0917890123', '2019-06-01', 'l.vo@example.com', 17000000, 'DP0006'),
('ST0007', N'Võ Quang A', N'Bác sĩ Thần kinh', N'Nam', '1983-03-14', '0917890127', '2020-06-02', 'a.vo@example.com', 28000000, 'DP0001'),
('ST0008', N'Võ Quang B', N'Bác sĩ Ung bứu', N'Nam', '1985-03-11', '0917890121', '2018-07-01', 'b.vo@example.com', 17500000, 'DP0002');

-- Chèn dữ liệu vào bảng DEPARTMENT
INSERT INTO DEPARTMENT (DepartmentID, DepartmentName, EmployeeNumber, HeadDepartmentID, PhoneNumber, LocationDPM)
VALUES
('DP0001', N'Khoa Nội', 2, 'ST0001', '0241234567', N'Hà Nội'),
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

INSERT INTO PATIENT (PatientID, FullName, DateOfBirth, Gender, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate, RoomID)
VALUES
('PA0007', N'Hồng Hài N', '1999-04-12', N'Nữ', '0962456786', N'Hải Phòng', 'h.hong@example.com', '2023-03-08', NULL, 'RO0002'),
('PA0008', N'Đỗ Thị H', '1999-04-12', N'Nữ', '0962456786', N'Hải Phòng', 'h.do@example.com', '2024-12-08', NULL, 'RO0002'),
('PA0009', N'Ngô Minh I', '1975-05-10', N'Nam', '0983456785', N'Lào Cai', 'i.ngo@example.com', '2024-11-27', '2024-12-09', 'RO0006'),
('PA0010', N'Thái Hồng K', '2000-10-25', N'Nữ', '0943456784', N'Hà Giang', 'k.thai@example.com', '2024-12-10', NULL, 'RO0006');
('PA0011', N'Nguyễn Văn A', '1985-06-12', N'Nam', '0987654321', N'Hà Nội', 'a.nguyen@example.com', '2024-11-20', '2024-12-05', 'RO0001'),
('PA0012', N'Lê Thị B', '1990-03-15', N'Nữ', '0978654322', N'Đà Nẵng', 'b.le@example.com', '2024-12-01', NULL, 'RO0002'),
('PA0013', N'Phạm Minh C', '2002-09-20', N'Nam', '0902456789', N'Hồ Chí Minh', 'c.pham@example.com', '2024-11-25', NULL, 'RO0001'),
('PA0014', N'Trần Văn D', '1978-11-10', N'Nam', '0945654321', N'Cần Thơ', 'd.tran@example.com', '2024-11-30', '2024-12-10', 'RO0003'),
('PA0015', N'Hoàng Thị E', '1987-02-07', N'Nữ', '0913456789', N'Bắc Ninh', 'e.hoang@example.com', '2024-12-02', NULL, 'RO0004'),
('PA0016', N'Vũ Ngọc F', '1995-08-17', N'Nữ', '0932456788', N'Nam Định', 'f.vu@example.com', '2024-11-28', '2024-12-08', 'RO0005'),
('PA0017', N'Bùi Thanh G', '1982-12-30', N'Nam', '0923456787', N'Quảng Ninh', 'g.bui@example.com', '2024-12-05', NULL, 'RO0001'),

INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan)
VALUES
('MR0005', 'PA0005', 'ST0002', '2024-12-03', N'Dau dạ dày', N'Nội soi: loét dạ dày', N'Dùng thuốc giảm tiết axit, kiêng đồ cay'),
('MR0006', 'PA0006', 'ST0004', '2024-11-29', N'Đau đầu mãn tính', N'CT scan không phát hiện bất thường', N'Dùng thuốc giảm đau, giảm căng thẳng'),
('MR0007', 'PA0007', 'ST0001', '2024-12-06', N'Cảm lạnh', N'Không có dấu hiệu nguy hiểm', N'Uống vitamin C và nghỉ ngơi'),
('MR0008', 'PA0008', 'ST0005', '2024-12-09', N'Suy nhược cơ thể', N'Chỉ số máu thấp', N'Tăng cường dinh dưỡng, vitamin'),
('MR0009', 'PA0009', 'ST0003', '2024-11-28', N'Hen suyễn', N'Chỉ số phổi giảm', N'Dùng thuốc hít và theo dõi định kỳ'),
('MR0010', 'PA0010', 'ST0004', '2024-12-11', N'Đau lưng', N'X-quang: thoái hóa cột sống', N'Vật lý trị liệu 3 tuần');
('MR0011', 'PA0011', 'ST0001', '2024-11-21', N'Cảm cúm', N'Huyết áp bình thường, không có triệu chứng nặng', N'Nghỉ ngơi, uống thuốc giảm đau'),
('MR0012', 'PA0012', 'ST0002', '2024-12-02', N'Sốt xuất huyết', N'Giảm tiểu cầu, sốt cao', N'Nhập viện, theo dõi hàng ngày'),
('MR0013', 'PA0003', 'ST0001', '2024-11-26', N'Viêm phổi', N'X-quang phổi phát hiện tổn thương nhỏ', N'Dùng kháng sinh 7 ngày'),
('MR0014', 'PA0004', 'ST0003', '2024-12-01', N'Gãy tay phải', N'Chụp X-quang: gãy xương quay', N'Bó bột và nghỉ ngơi 4 tuần'),



