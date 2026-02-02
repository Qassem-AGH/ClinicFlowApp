-- Clinic CRUD Examples
-- Demonstrates CREATE, READ, UPDATE, DELETE operations

USE ClinicDB;
GO

PRINT '========================================';
PRINT 'CRUD EXAMPLES FOR CLINICFLOW';
PRINT '========================================';
GO

-- ==========================================
-- CREATE EXAMPLES
-- ==========================================
PRINT '';
PRINT '--- CREATE EXAMPLES ---';

-- Insert a new patient
INSERT INTO Patients (FirstName, LastName, Email, Phone)
VALUES ('Test', 'Testsson', 'test.testsson@email.com', '0709999999');
PRINT 'Created new patient: Test Testsson';
GO

-- Insert a new treatment
INSERT INTO Treatments (TreatmentName, DurationMinutes, Price)
VALUES ('Ultrasound Scan', 30, 750.00);
PRINT 'Created new treatment: Ultrasound Scan';
GO

-- Insert a new appointment
DECLARE @NewPatientID INT = (SELECT PatientID FROM Patients WHERE Email = 'test.testsson@email.com');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, Status)
VALUES (@NewPatientID, 1, '2026-02-15 10:00', 'Scheduled');
PRINT 'Created new appointment for Test Testsson';
GO

-- ==========================================
-- READ EXAMPLES
-- ==========================================
PRINT '';
PRINT '--- READ EXAMPLES ---';

-- Select all patients
SELECT PatientID, FirstName, LastName, Email, Phone, CreatedAt
FROM Patients;
PRINT 'Retrieved all patients';
GO

-- Select all appointments with status
SELECT AppointmentID, PatientID, DoctorID, AppointmentDate, Status
FROM Appointments
WHERE Status = 'Scheduled'
ORDER BY AppointmentDate;
PRINT 'Retrieved all scheduled appointments';
GO

-- Select specific treatment
SELECT TreatmentID, TreatmentName, DurationMinutes, Price
FROM Treatments
WHERE TreatmentName LIKE '%Checkup%';
PRINT 'Retrieved specific treatments';
GO

-- Select all doctors in a specific clinic
SELECT DoctorID, FirstName, LastName, Specialization
FROM Doctor
WHERE ClinicID = 1;
PRINT 'Retrieved doctors from Clinic 1';
GO

-- ==========================================
-- UPDATE EXAMPLES
-- ==========================================
PRINT '';
PRINT '--- UPDATE EXAMPLES ---';

-- Update appointment status
UPDATE Appointments
SET Status = 'Completed'
WHERE AppointmentID = 20;
PRINT 'Updated appointment status to Completed';
GO

-- Update patient phone number
UPDATE Patients
SET Phone = '0701111111'
WHERE Email = 'test.testsson@email.com';
PRINT 'Updated patient phone number';
GO

-- Update treatment price
UPDATE Treatments
SET Price = 550.00
WHERE TreatmentName = 'General Checkup';
PRINT 'Updated treatment price';
GO

-- Update doctor specialization
UPDATE Doctor
SET Specialization = 'General Practice & Family Medicine'
WHERE DoctorID = 1;
PRINT 'Updated doctor specialization';
GO

-- ==========================================
-- DELETE EXAMPLES
-- ==========================================
PRINT '';
PRINT '--- DELETE EXAMPLES ---';

-- Delete appointment treatment (from junction table)
DECLARE @TestAppointmentID INT = (
    SELECT TOP 1 AppointmentID 
    FROM Appointments 
    WHERE PatientID = (SELECT PatientID FROM Patients WHERE Email = 'test.testsson@email.com')
);

IF @TestAppointmentID IS NOT NULL
BEGIN
    DELETE FROM AppointmentTreatments
    WHERE AppointmentID = @TestAppointmentID;
    PRINT 'Deleted appointment treatments';
END
GO

-- Delete appointment (note: FK constraints must be handled)
DECLARE @TestAppointmentID INT = (
    SELECT TOP 1 AppointmentID 
    FROM Appointments 
    WHERE PatientID = (SELECT PatientID FROM Patients WHERE Email = 'test.testsson@email.com')
);

IF @TestAppointmentID IS NOT NULL
BEGIN
    DELETE FROM Appointments
    WHERE AppointmentID = @TestAppointmentID;
    PRINT 'Deleted test appointment';
END
GO

-- Delete patient (only if no appointments exist)
-- First, check if patient has appointments
DECLARE @TestPatientID INT = (SELECT PatientID FROM Patients WHERE Email = 'test.testsson@email.com');
DECLARE @AppointmentCount INT = (SELECT COUNT(*) FROM Appointments WHERE PatientID = @TestPatientID);

IF @AppointmentCount = 0
BEGIN
    DELETE FROM Patients
    WHERE PatientID = @TestPatientID;
    PRINT 'Deleted test patient (no appointments found)';
END
ELSE
BEGIN
    PRINT 'Cannot delete patient - appointments exist';
END
GO

-- Delete a treatment (only if not used in appointments)
DECLARE @TestTreatmentID INT = (SELECT TreatmentID FROM Treatments WHERE TreatmentName = 'Ultrasound Scan');
DECLARE @TreatmentUsageCount INT = (SELECT COUNT(*) FROM AppointmentTreatments WHERE TreatmentID = @TestTreatmentID);

IF @TreatmentUsageCount = 0
BEGIN
    DELETE FROM Treatments
    WHERE TreatmentID = @TestTreatmentID;
    PRINT 'Deleted test treatment (not used in any appointments)';
END
ELSE
BEGIN
    PRINT 'Cannot delete treatment - it is used in appointments';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'CRUD EXAMPLES COMPLETED';
PRINT '========================================';