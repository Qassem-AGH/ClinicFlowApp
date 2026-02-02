-- Clinic Cleanup Script
-- Drops all objects and database for fresh start

USE master;
GO

PRINT '========================================';
PRINT 'CLEANUP SCRIPT FOR CLINIC';
PRINT 'WARNING: This will delete everything!';
PRINT '========================================';
GO

-- ==========================================
-- DROP LOGINS
-- ==========================================

IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'ClinicReportUser')
BEGIN
    DROP LOGIN ClinicReportUser;
    PRINT 'Dropped login: ClinicReportUser';
END
GO

IF EXISTS (SELECT * FROM sys.server_principals WHERE name = 'ClinicAppUser')
BEGIN
    DROP LOGIN ClinicAppUser;
    PRINT 'Dropped login: ClinicAppUser';
END
GO

-- ==========================================
-- DROP DATABASE
-- ==========================================

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'ClinicFlowDB')
BEGIN
    -- Force single user mode to disconnect all users
    ALTER DATABASE ClinicFlowDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Drop the database
    DROP DATABASE ClinicFlowDB;
    
    PRINT 'Dropped database: ClinicFlowDB';
END
ELSE
BEGIN
    PRINT 'Database ClinicFlowDB does not exist';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'CLEANUP COMPLETED';
PRINT '========================================';
PRINT '';
PRINT 'To recreate the database, run scripts in order:';
PRINT '  1. 01_create_database.sql';
PRINT '  2. 02_create_tables.sql';
PRINT '  3. 03_seed_data.sql';
PRINT '  4. 04_crud_examples.sql';
PRINT '  5. 05_queries_joins.sql';
PRINT '  6. 06_views.sql';
PRINT '  7. 07_security.sql';
PRINT '';
GO

-- ==========================================
-- ALTERNATIVE: CLEANUP ONLY DATA
-- ==========================================

-- Uncomment below to only delete data, keep structure
/*
USE ClinicFlowDB;
GO

PRINT 'Cleaning data only (keeping structure)...';

-- Delete data in correct order (respecting FK constraints)
DELETE FROM AppointmentTreatments;
DELETE FROM Appointments;
DELETE FROM Patients;
DELETE FROM Doctor;
DELETE FROM Clinic;
DELETE FROM Treatments;

PRINT 'All data deleted. Structure preserved.';
GO
*/