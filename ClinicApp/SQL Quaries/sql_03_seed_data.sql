-- ClinicFlow Seed Data Script
-- Inserts test data into all tables

USE ClinicDB;
GO

-- Seed Clinics (3 clinics)
INSERT INTO Clinic (ClinicName, Address, City) VALUES
('City Health Clinic', 'Storgatan 10', 'Stockholm'),
('Family Care Center', 'Västra Hamngatan 5', 'Göteborg'),
('MediCare Plus', 'Kungsgatan 25', 'Malmö');
GO

-- Seed Doctors (8 doctors)
INSERT INTO Doctor (FirstName, LastName, Specialization, ClinicID) VALUES
('Anna', 'Andersson', 'General Practice', 1),
('Erik', 'Eriksson', 'Cardiology', 1),
('Maria', 'Johansson', 'Pediatrics', 1),
('Lars', 'Svensson', 'Orthopedics', 2),
('Karin', 'Nilsson', 'Dermatology', 2),
('Peter', 'Berg', 'General Practice', 2),
('Sofia', 'Lundqvist', 'Neurology', 3),
('Johan', 'Karlsson', 'General Practice', 3);
GO

-- Seed Patients (12 patients - more than required 10)
INSERT INTO Patients (FirstName, LastName, Email, Phone) VALUES
('Emma', 'Larsson', 'emma.larsson@email.com', '0701234567'),
('Oscar', 'Pettersson', 'oscar.pettersson@email.com', '0701234568'),
('Alice', 'Gustafsson', 'alice.gustafsson@email.com', '0701234569'),
('Lucas', 'Olsson', 'lucas.olsson@email.com', '0701234570'),
('Maja', 'Lindberg', 'maja.lindberg@email.com', '0701234571'),
('William', 'Jakobsson', 'william.jakobsson@email.com', '0701234572'),
('Ella', 'Bergström', 'ella.bergstrom@email.com', '0701234573'),
('Liam', 'Sandberg', 'liam.sandberg@email.com', '0701234574'),
('Olivia', 'Holm', 'olivia.holm@email.com', '0701234575'),
('Noah', 'Engström', 'noah.engstrom@email.com', '0701234576'),
('Astrid', 'Lundgren', 'astrid.lundgren@email.com', '0701234577'),
('Elias', 'Forsberg', 'elias.forsberg@email.com', '0701234578');
GO

-- Seed Treatments (8 treatments - more than required 6)
INSERT INTO Treatments (TreatmentName, DurationMinutes, Price) VALUES
('General Checkup', 30, 500.00),
('Blood Test', 15, 300.00),
('X-Ray', 20, 800.00),
('Physical Therapy', 45, 600.00),
('Vaccination', 10, 250.00),
('ECG Test', 25, 450.00),
('Consultation', 60, 700.00),
('Skin Examination', 30, 550.00);
GO

-- Seed Appointments (30 appointments)
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, Status) VALUES
-- Past completed appointments
(1, 1, '2026-01-15 09:00', 'Completed'),
(2, 1, '2026-01-15 10:00', 'Completed'),
(3, 2, '2026-01-16 11:00', 'Completed'),
(4, 3, '2026-01-16 14:00', 'Completed'),
(5, 4, '2026-01-17 09:30', 'Completed'),
(6, 5, '2026-01-18 10:30', 'NoShow'),
(7, 6, '2026-01-19 13:00', 'Completed'),
(8, 7, '2026-01-20 15:00', 'Completed'),
(9, 8, '2026-01-21 09:00', 'NoShow'),
(10, 1, '2026-01-22 10:00', 'Completed'),
-- Recent appointments
(1, 2, '2026-01-25 11:00', 'Completed'),
(2, 3, '2026-01-26 14:00', 'Completed'),
(3, 1, '2026-01-27 09:00', 'Cancelled'),
(11, 4, '2026-01-28 10:30', 'Completed'),
(12, 5, '2026-01-29 13:00', 'NoShow'),
(4, 6, '2026-01-30 15:00', 'Completed'),
(5, 7, '2026-01-31 09:30', 'Completed'),
-- Current and future appointments
(6, 8, '2026-02-01 10:00', 'Completed'),
(7, 1, '2026-02-01 14:00', 'Scheduled'),
(8, 2, '2026-02-02 09:00', 'Scheduled'),
(9, 3, '2026-02-02 11:00', 'Scheduled'),
(10, 4, '2026-02-03 13:00', 'Scheduled'),
(11, 5, '2026-02-03 15:00', 'Scheduled'),
(12, 6, '2026-02-04 09:30', 'Scheduled'),
(1, 7, '2026-02-05 10:30', 'Scheduled'),
(2, 8, '2026-02-06 14:00', 'Scheduled'),
(3, 1, '2026-02-07 09:00', 'Scheduled'),
(4, 2, '2026-02-08 11:00', 'Scheduled'),
(5, 3, '2026-02-09 13:00', 'Scheduled'),
(6, 4, '2026-02-10 15:00', 'Scheduled');
GO

-- Seed AppointmentTreatments (35+ records - more than required 25)
INSERT INTO AppointmentTreatments (AppointmentID, TreatmentID) VALUES
-- Appointment 1
(1, 1), (1, 2),
-- Appointment 2
(2, 1),
-- Appointment 3
(3, 6), (3, 7),
-- Appointment 4
(4, 1), (4, 5),
-- Appointment 5
(5, 4),
-- Appointment 6
(6, 8),
-- Appointment 7
(7, 1), (7, 2),
-- Appointment 8
(8, 7),
-- Appointment 9
(9, 1),
-- Appointment 10
(10, 1), (10, 3),
-- Appointment 11
(11, 6),
-- Appointment 12
(12, 1), (12, 5),
-- Appointment 13
(13, 7),
-- Appointment 14
(14, 4), (14, 1),
-- Appointment 15
(15, 8),
-- Appointment 16
(16, 1), (16, 2), (16, 3),
-- Appointment 17
(17, 4),
-- Appointment 18
(18, 1),
-- Appointment 19
(19, 7),
-- Appointment 20
(20, 1), (20, 2),
-- More appointments
(21, 1),
(22, 4),
(23, 7),
(24, 1), (24, 5);
GO

PRINT 'All seed data inserted successfully';
PRINT '- 3 Clinics';
PRINT '- 8 Doctors';
PRINT '- 12 Patients';
PRINT '- 8 Treatments';
PRINT '- 30 Appointments';
PRINT '- 35+ AppointmentTreatments relations';