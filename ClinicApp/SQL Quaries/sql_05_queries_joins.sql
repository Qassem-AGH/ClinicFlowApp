-- Clinic Queries and Joins
-- Contains 8+ SELECT queries with JOINs, GROUP BY, WHERE, ORDER BY

USE ClinicDB;
GO

PRINT '========================================';
PRINT 'ADVANCED QUERIES FOR CLINICFLOW';
PRINT '========================================';
GO

-- ==========================================
-- JOIN QUERIES (4 required)
-- ==========================================

-- Query 1: List all appointments with patient and doctor names (INNER JOIN)
PRINT '';
PRINT '--- Query 1: All appointments with patient and doctor details ---';
SELECT 
    a.AppointmentID,
    a.AppointmentDate,
    a.Status,
    p.FirstName + ' ' + p.LastName AS PatientName,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    d.Specialization
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
INNER JOIN Doctor d ON a.DoctorID = d.DoctorID
ORDER BY a.AppointmentDate DESC;
GO

-- Query 2: Appointments with treatments and prices (MULTIPLE JOINS)
PRINT '';
PRINT '--- Query 2: Appointments with treatments and total cost ---';
SELECT 
    a.AppointmentID,
    p.FirstName + ' ' + p.LastName AS PatientName,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    a.AppointmentDate,
    t.TreatmentName,
    t.Price,
    a.Status
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
INNER JOIN Doctor d ON a.DoctorID = d.DoctorID
INNER JOIN AppointmentTreatments at ON a.AppointmentID = at.AppointmentID
INNER JOIN Treatments t ON at.TreatmentID = t.TreatmentID
WHERE a.Status = 'Completed'
ORDER BY a.AppointmentDate DESC;
GO

-- Query 3: Doctors with their clinics (JOIN)
PRINT '';
PRINT '--- Query 3: Doctors and their clinic locations ---';
SELECT 
    d.DoctorID,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    d.Specialization,
    c.ClinicName,
    c.City
FROM Doctor d
INNER JOIN Clinic c ON d.ClinicID = c.ClinicID
ORDER BY c.City, d.LastName;
GO

-- Query 4: Patients with their appointment history (LEFT JOIN)
PRINT '';
PRINT '--- Query 4: Patients and their appointment count ---';
SELECT 
    p.PatientID,
    p.FirstName + ' ' + p.LastName AS PatientName,
    p.Email,
    COUNT(a.AppointmentID) AS TotalAppointments
FROM Patients p
LEFT JOIN Appointments a ON p.PatientID = a.PatientID
GROUP BY p.PatientID, p.FirstName, p.LastName, p.Email
ORDER BY TotalAppointments DESC;
GO

-- ==========================================
-- GROUP BY AND AGGREGATE QUERIES (2 required)
-- ==========================================

-- Query 5: Count of appointments per doctor
PRINT '';
PRINT '--- Query 5: Doctor workload (appointments per doctor) ---';
SELECT 
    d.DoctorID,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    d.Specialization,
    COUNT(a.AppointmentID) AS TotalAppointments,
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedAppointments,
    SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) AS NoShowCount
FROM Doctor d
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY d.DoctorID, d.FirstName, d.LastName, d.Specialization
ORDER BY TotalAppointments DESC;
GO

-- Query 6: Revenue per treatment type
PRINT '';
PRINT '--- Query 6: Treatment popularity and revenue ---';
SELECT 
    t.TreatmentID,
    t.TreatmentName,
    COUNT(at.AppointmentID) AS TimesBooked,
    t.Price,
    COUNT(at.AppointmentID) * t.Price AS TotalRevenue
FROM Treatments t
LEFT JOIN AppointmentTreatments at ON t.TreatmentID = at.TreatmentID
GROUP BY t.TreatmentID, t.TreatmentName, t.Price
ORDER BY TimesBooked DESC;
GO

-- ==========================================
-- WHERE + ORDER BY QUERY (1 required)
-- ==========================================

-- Query 7: Upcoming appointments for a specific date range
PRINT '';
PRINT '--- Query 7: Upcoming scheduled appointments (next 14 days) ---';
SELECT 
    a.AppointmentID,
    p.FirstName + ' ' + p.LastName AS PatientName,
    d.FirstName + ' ' + d.LastName AS DoctorName,
    a.AppointmentDate,
    a.Status
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
INNER JOIN Doctor d ON a.DoctorID = d.DoctorID
WHERE a.Status = 'Scheduled' 
  AND a.AppointmentDate BETWEEN GETDATE() AND DATEADD(DAY, 14, GETDATE())
ORDER BY a.AppointmentDate ASC;
GO

-- ==========================================
-- REPORT QUERY (1 required - for Console App)
-- ==========================================

-- Query 8: REPORT - Top patients with most visits (for "At Risk" analysis)
PRINT '';
PRINT '--- Query 8: REPORT - Top patients with visit summary ---';
SELECT TOP 10
    p.PatientID,
    p.FirstName + ' ' + p.LastName AS PatientName,
    p.Email,
    COUNT(a.AppointmentID) AS TotalVisits,
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedVisits,
    SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) AS NoShows,
    SUM(CASE WHEN a.Status = 'Cancelled' THEN 1 ELSE 0 END) AS Cancelled,
    CAST(SUM(CASE WHEN a.Status = 'NoShow' THEN 1 ELSE 0 END) * 100.0 / COUNT(a.AppointmentID) AS DECIMAL(5,2)) AS NoShowPercentage
FROM Patients p
INNER JOIN Appointments a ON p.PatientID = a.PatientID
GROUP BY p.PatientID, p.FirstName, p.LastName, p.Email
HAVING COUNT(a.AppointmentID) > 0
ORDER BY TotalVisits DESC, NoShows DESC;
GO

-- ==========================================
-- BONUS QUERIES
-- ==========================================

-- Query 9: Appointments by status summary
PRINT '';
PRINT '--- Query 9: Appointment status summary ---';
SELECT 
    Status,
    COUNT(*) AS Count,
    CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Appointments) AS DECIMAL(5,2)) AS Percentage
FROM Appointments
GROUP BY Status
ORDER BY Count DESC;
GO

-- Query 10: Clinic performance summary
PRINT '';
PRINT '--- Query 10: Clinic performance overview ---';
SELECT 
    c.ClinicID,
    c.ClinicName,
    c.City,
    COUNT(DISTINCT d.DoctorID) AS NumberOfDoctors,
    COUNT(a.AppointmentID) AS TotalAppointments,
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedAppointments
FROM Clinic c
LEFT JOIN Doctor d ON c.ClinicID = d.ClinicID
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY c.ClinicID, c.ClinicName, c.City
ORDER BY TotalAppointments DESC;
GO

PRINT '';
PRINT '========================================';
PRINT 'ALL QUERIES COMPLETED SUCCESSFULLY';
PRINT '========================================';