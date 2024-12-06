DROP DATABASE HospitalDB

CREATE DATABASE HospitalDB

USE HospitalDB

SET DATEFORMAT DMY  --Thiết lập DATE theo thứ tự ngày->tháng->năm

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
    Amount INT,  -- Số lượng thuốc
    PRIMARY KEY (TransactionID, MedicationID)
)

CREATE TABLE MEDICATION (
    MedicationID CHAR(6) PRIMARY KEY,
    MedicationName NVARCHAR(255),
    Dosage NVARCHAR(100), --Liều lượng
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
ADD CONSTRAINT FK_BIDE_BI FOREIGN KEY (TransactionID) REFERENCES BILL(TransactionID) 

ALTER TABLE BILLDETAIL
ADD CONSTRAINT FK_BIDE_ME FOREIGN KEY (MedicationID) REFERENCES MEDICATION(MedicationID) 

ALTER TABLE ROOM
ADD CONSTRAINT FK_RO_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_STA FOREIGN KEY (StaffID) REFERENCES STAFF(StaffID) 

ALTER TABLE WEEKLYASSIGNMENT
ADD CONSTRAINT FK_WE_DE FOREIGN KEY (DepartmentID) REFERENCES DEPARTMENT(DepartmentID) 

