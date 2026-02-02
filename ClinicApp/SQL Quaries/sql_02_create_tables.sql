-- ClinicFlow Tables Creation Script
-- Creates all tables with PK, FK, constraints, and defaults

USE ClinicDB;
GO

-- Table 1: Clinic
CREATE TABLE Clinic (
    ClinicID INT PRIMARY KEY IDENTITY(1,1),
    ClinicName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table 2: Doctor
CREATE TABLE Doctor (
    DoctorID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    ClinicID INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Doctor_Clinic FOREIGN KEY (ClinicID) REFERENCES Clinic(ClinicID)
);
GO

-- Table 3: Patients
CREATE TABLE Patients (
    PatientID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table 4: Treatments
CREATE TABLE Treatments (
    TreatmentID INT PRIMARY KEY IDENTITY(1,1),
    TreatmentName NVARCHAR(100) NOT NULL,
    DurationMinutes INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    CONSTRAINT CHK_DurationMinutes CHECK (DurationMinutes > 0),
    CONSTRAINT CHK_Price CHECK (Price >= 0)
);
GO

-- Table 5: Appointments
CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY IDENTITY(1,1),
    PatientID INT NOT NULL,
    DoctorID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Scheduled',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Appointments_Patient FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    CONSTRAINT FK_Appointments_Doctor FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID),
    CONSTRAINT CHK_Status CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled', 'NoShow'))
);
GO

-- Table 6: AppointmentTreatments (Many-to-Many junction table)
CREATE TABLE AppointmentTreatments (
    AppointmentID INT NOT NULL,
    TreatmentID INT NOT NULL,
    CONSTRAINT PK_AppointmentTreatments PRIMARY KEY (AppointmentID, TreatmentID),
    CONSTRAINT FK_AppointmentTreatments_Appointment FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID) ON DELETE CASCADE,
    CONSTRAINT FK_AppointmentTreatments_Treatment FOREIGN KEY (TreatmentID) REFERENCES Treatments(TreatmentID)
);
GO

PRINT 'All tables created successfully with constraints';