-- Clinic Views
-- Creates public views and report views

USE ClinicDB;
GO

PRINT '========================================';
PRINT 'CREATING VIEWS FOR CLINICFLOW';
PRINT '========================================';
GO

-- ==========================================
-- VIEW 1: Public Patient View (hides sensitive data)
-- ==========================================

-- Drop view if exists
IF OBJECT_ID('vw_PublicPatients', 'V') IS NOT NULL
    DROP VIEW vw_PublicPatients;
GO

CREATE VIEW vw_PublicPatients AS
SELECT 
    PatientID,
    FirstName,
    LastName,
    -- Hide full email, show only domain
    LEFT(Email, 2) + '***@' + RIGHT(Email, CHARINDEX('@', REVERSE(Email)) - 1) AS MaskedEmail,
    -- Hide phone number except last 4 digits
    '***-' + RIGHT(Phone, 4) AS MaskedPhone,
    CONVERT(DATE, CreatedAt) AS RegistrationDate
FROM Patients;
GO

PRINT 'Created vw_PublicPatients - hides sensitive patient information';
GO

-- ==========================================
-- VIEW 2: Public Doctor View (safe information)
-- ==========================================

IF OBJECT_ID('vw_PublicDoctors', 'V') IS NOT NULL
    DROP VIEW vw_PublicDoctors;
GO

CREATE VIEW vw_PublicDoctors AS
SELECT 
    d.DoctorID,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    d.Specialization,
    c.ClinicName,
    c.City,
    COUNT(a.AppointmentID) AS TotalAppointments
FROM Doctor d
INNER JOIN Clinic c ON d.ClinicID = c.ClinicID
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY d.DoctorID, d.FirstName, d.LastName, d.Specialization, c.ClinicName, c.City;
GO

PRINT 'Created vw_PublicDoctors - public doctor information';
GO

-- ==========================================
-- VIEW 3: Report View - Patient Visit Summary
-- ==========================================

IF OBJECT_ID('vw_ReportPatientVisits', 'V') IS NOT NULL
    DROP VIEW vw_ReportPatientVisits;
GO

CREATE VIEW vw_ReportPatientVisits AS
SELECT 
    p.PatientID,
    p.FirstName + ' ' + p.LastName AS PatientName,
    p.Email,
    COUNT(a.AppointmentID) AS TotalVisits,
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedVisits,
    SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) AS NoShows,
    SUM(CASE WHEN a.Status = 'Cancelled' THEN 1 ELSE 0 END) AS Cancelled,
    SUM(CASE WHEN a.Status = 'Scheduled' THEN 1 ELSE 0 END) AS Upcoming,
    MAX(a.AppointmentDate) AS LastVisitDate,
    CASE 
        WHEN SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) >= 3 THEN 'High Risk'
        WHEN SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) >= 2 THEN 'Medium Risk'
        ELSE 'Low Risk'
    END AS RiskLevel
FROM Patients p
LEFT JOIN Appointments a ON p.PatientID = a.PatientID
GROUP BY p.PatientID, p.FirstName, p.LastName, p.Email;
GO

PRINT 'Created vw_ReportPatientVisits - comprehensive patient visit report';
GO

-- ==========================================
-- VIEW 4: Report View - Doctor Workload
-- ==========================================

IF OBJECT_ID('vw_ReportDoctorWorkload', 'V') IS NOT NULL
    DROP VIEW vw_ReportDoctorWorkload;
GO

CREATE VIEW vw_ReportDoctorWorkload AS
SELECT 
    d.DoctorID,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    d.Specialization,
    c.ClinicName,
    COUNT(a.AppointmentID) AS TotalAppointments,
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedAppointments,
    SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) AS NoShows,
    SUM(CASE WHEN a.Status = 'Scheduled' AND a.AppointmentDate > GETDATE() THEN 1 ELSE 0 END) AS UpcomingAppointments,
    CAST(
        CASE 
            WHEN COUNT(a.AppointmentID) > 0 
            THEN (SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) * 100.0 / COUNT(a.AppointmentID))
            ELSE 0 
        END AS DECIMAL(5,2)
    ) AS CompletionRate
FROM Doctor d
INNER JOIN Clinic c ON d.ClinicID = c.ClinicID
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY d.DoctorID, d.FirstName, d.LastName, d.Specialization, c.ClinicName;
GO

PRINT 'Created vw_ReportDoctorWorkload - doctor performance metrics';
GO

-- ==========================================
-- VIEW 5: Report View - Latest Activity Feed
-- ==========================================

IF OBJECT_ID('vw_ReportLatestActivity', 'V') IS NOT NULL
    DROP VIEW vw_ReportLatestActivity;
GO

CREATE VIEW vw_ReportLatestActivity AS
SELECT TOP 20
    a.AppointmentID,
    a.AppointmentDate,
    p.FirstName + ' ' + p.LastName AS PatientName,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    c.ClinicName,
    a.Status,
    a.CreatedAt AS BookedAt
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
INNER JOIN Doctor d ON a.DoctorID = d.DoctorID
INNER JOIN Clinic c ON d.ClinicID = c.ClinicID
ORDER BY a.CreatedAt DESC;
GO

PRINT 'Created vw_ReportLatestActivity - latest 20 appointments';
GO

-- ==========================================
-- VIEW 6: Report View - Popular Treatments
-- ==========================================

IF OBJECT_ID('vw_ReportPopularTreatments', 'V') IS NOT NULL
    DROP VIEW vw_ReportPopularTreatments;
GO

CREATE VIEW vw_ReportPopularTreatments AS
SELECT 
    t.TreatmentID,
    t.TreatmentName,
    t.DurationMinutes,
    t.Price,
    COUNT(at.AppointmentID) AS TimesBooked,
    COUNT(at.AppointmentID) * t.Price AS TotalRevenue
FROM Treatments t
LEFT JOIN AppointmentTreatments at ON t.TreatmentID = at.TreatmentID
GROUP BY t.TreatmentID, t.TreatmentName, t.DurationMinutes, t.Price;
GO

PRINT 'Created vw_ReportPopularTreatments - treatment statistics';
GO

-- ==========================================
-- TEST VIEWS
-- ==========================================

PRINT '';
PRINT '--- Testing Views ---';

-- Test public patient view
SELECT TOP 5 * FROM vw_PublicPatients;
PRINT 'Tested vw_PublicPatients';

-- Test report view
SELECT TOP 5 * FROM vw_ReportPatientVisits ORDER BY TotalVisits DESC;
PRINT 'Tested vw_ReportPatientVisits';

PRINT '';
PRINT '========================================';
PRINT 'ALL VIEWS CREATED SUCCESSFULLY';
PRINT '========================================';