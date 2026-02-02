-- Clinic Security Script
-- Creates roles, users, and grants permissions

USE ClinicDB;
GO

PRINT '========================================';
PRINT 'CONFIGURING SECURITY FOR CLINICFLOW';
PRINT '========================================';
GO

-- ==========================================
-- CREATE DATABASE ROLES
-- ==========================================

-- Drop role if exists
IF DATABASE_PRINCIPAL_ID('ClinicReadRole') IS NOT NULL
    DROP ROLE ClinicReadRole;
GO

-- Create read-only role for report access
CREATE ROLE ClinicReadRole;
GO

PRINT 'Created role: ClinicReadRole';
GO

-- ==========================================
-- GRANT PERMISSIONS ON VIEWS ONLY
-- ==========================================

-- Grant SELECT on all views
GRANT SELECT ON vw_PublicPatients TO ClinicReadRole;
GRANT SELECT ON vw_PublicDoctors TO ClinicReadRole;
GRANT SELECT ON vw_ReportPatientVisits TO ClinicReadRole;
GRANT SELECT ON vw_ReportDoctorWorkload TO ClinicReadRole;
GRANT SELECT ON vw_ReportLatestActivity TO ClinicReadRole;
GRANT SELECT ON vw_ReportPopularTreatments TO ClinicReadRole;
GO

PRINT 'Granted SELECT on all views to ClinicReadRole';
GO

-- ==========================================
-- DENY DIRECT TABLE ACCESS
-- ==========================================

-- Explicitly deny direct access to tables
DENY SELECT ON Patients TO ClinicReadRole;
DENY SELECT ON Doctor TO ClinicReadRole;
DENY SELECT ON Clinic TO ClinicReadRole;
DENY SELECT ON Appointments TO ClinicReadRole;
DENY SELECT ON Treatments TO ClinicReadRole;
DENY SELECT ON AppointmentTreatments TO ClinicReadRole;
GO

PRINT 'Denied direct SELECT on all tables to ClinicReadRole';
GO

-- ==========================================
-- CREATE LOGIN AND USER (SQL Authentication)
-- ==========================================

-- Check if login exists and drop if needed
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'ClinicReportUser')
BEGIN
    DROP LOGIN ClinicReportUser;
    PRINT 'Dropped existing login: ClinicReportUser';
END
GO

-- Create SQL Server login
CREATE LOGIN ClinicReportUser WITH PASSWORD = 'Report123!Secure';
GO

PRINT 'Created login: ClinicReportUser';
GO

-- Create database user
IF DATABASE_PRINCIPAL_ID('ClinicReportUser') IS NOT NULL
    DROP USER ClinicReportUser;
GO

CREATE USER ClinicReportUser FOR LOGIN ClinicReportUser;
GO

PRINT 'Created user: ClinicReportUser';
GO

-- ==========================================
-- ASSIGN USER TO ROLE
-- ==========================================

-- Add user to role
ALTER ROLE ClinicReadRole ADD MEMBER ClinicReportUser;
GO

PRINT 'Added ClinicReportUser to ClinicReadRole';
GO

-- ==========================================
-- CREATE ADDITIONAL ROLE FOR APP USER
-- ==========================================

-- Drop role if exists
IF DATABASE_PRINCIPAL_ID('ClinicAppRole') IS NOT NULL
    DROP ROLE ClinicAppRole;
GO

-- Create application role with full CRUD access
CREATE ROLE ClinicAppRole;
GO

-- Grant full CRUD permissions to application role
GRANT SELECT, INSERT, UPDATE, DELETE ON Patients TO ClinicAppRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Doctor TO ClinicAppRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Clinic TO ClinicAppRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Appointments TO ClinicAppRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Treatments TO ClinicAppRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON AppointmentTreatments TO ClinicAppRole;
GO

-- Also grant view access
GRANT SELECT ON vw_PublicPatients TO ClinicAppRole;
GRANT SELECT ON vw_PublicDoctors TO ClinicAppRole;
GRANT SELECT ON vw_ReportPatientVisits TO ClinicAppRole;
GRANT SELECT ON vw_ReportDoctorWorkload TO ClinicAppRole;
GRANT SELECT ON vw_ReportLatestActivity TO ClinicAppRole;
GRANT SELECT ON vw_ReportPopularTreatments TO ClinicAppRole;
GO

PRINT 'Created role: ClinicAppRole with full CRUD permissions';
GO

-- Create application user
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'ClinicAppUser')
BEGIN
    DROP LOGIN ClinicAppUser;
END
GO

CREATE LOGIN ClinicAppUser WITH PASSWORD = 'App123!Secure';
GO

IF DATABASE_PRINCIPAL_ID('ClinicAppUser') IS NOT NULL
    DROP USER ClinicAppUser;
GO

CREATE USER ClinicAppUser FOR LOGIN ClinicAppUser;
GO

ALTER ROLE ClinicAppRole ADD MEMBER ClinicAppUser;
GO

PRINT 'Created user: ClinicAppUser with application permissions';
GO

-- ==========================================
-- SECURITY SUMMARY
-- ==========================================

PRINT '';
PRINT '========================================';
PRINT 'SECURITY CONFIGURATION SUMMARY';
PRINT '========================================';
PRINT '';
PRINT 'ROLES CREATED:';
PRINT '  - ClinicReadRole: View access only';
PRINT '  - ClinicAppRole: Full CRUD access';
PRINT '';
PRINT 'USERS CREATED:';
PRINT '  - ClinicReportUser (password: Report123!Secure)';
PRINT '    - Member of ClinicReadRole';
PRINT '    - Can access views only';
PRINT '    - Cannot access tables directly';
PRINT '';
PRINT '  - ClinicAppUser (password: App123!Secure)';
PRINT '    - Member of ClinicAppRole';
PRINT '    - Full CRUD on all tables';
PRINT '    - Can access views';
PRINT '';
PRINT 'PERMISSIONS:';
PRINT '  - ClinicReadRole: SELECT on views, DENY on tables';
PRINT '  - ClinicAppRole: Full CRUD on tables and views';
PRINT '';
PRINT '========================================';
PRINT 'SECURITY SETUP COMPLETED';
PRINT '========================================';