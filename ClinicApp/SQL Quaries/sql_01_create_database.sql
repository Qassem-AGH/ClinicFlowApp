-- ClinicFlow Database Creation Script
-- Creates the main database for the clinic booking system

-- Drop database if exists (for clean setup)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'ClinicDB')
BEGIN
    ALTER DATABASE ClinicDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ClinicDB;
END
GO

-- Create new database
CREATE DATABASE ClinicDB;
GO

-- Set context to new database
USE ClinicDB;
GO

PRINT 'ClinicDB database created successfully';