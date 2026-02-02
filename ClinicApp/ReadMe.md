**🏥 Clinic Flow App**
A modern, colorful console application for managing clinic appointments, patients, doctors, and treatments built with C# .NET and Spectre.Console.


**✨ Features**
📋 Core Management

👥 Patient Management - Create and view patient records
👨‍⚕️ Doctor Management - Browse all doctors and their specializations
💊 Treatment Management - View available treatments with pricing
📅 Appointment System - Create, update, and delete appointments
🔗 Treatment Linking - Add multiple treatments to appointments

**📊 Advanced Reports**

🏆 Top Patients - Track most active patients by visit count
👨‍⚕️ Doctor Workload - Monitor appointment distribution across doctors
⚠️ At-Risk Patients - Identify patients with high no-show rates
📰 Activity Feed - View latest appointment activity
⭐ Popular Treatments - Analyze treatment bookings and revenue
🏥 Clinic Summary - Overview of all clinic locations


**🎨 User Interface**
Built with Spectre.Console for a beautiful terminal experience:

🌈 Color-coded tables with themed borders
😊 Emoji indicators for visual clarity
📊 Interactive selection menus (no more typing IDs!)
🎯 Status-based coloring (green=completed, red=no-show, etc.)
🥇 Medal rankings for top performers
📦 Information panels for important data
✅ Success/Error messages with icons

**#Color Themes by Module**
ModuleColorIconPatientsCyan👥DoctorsBlue👨‍⚕️TreatmentsMagenta💊AppointmentsAqua📅ReportsVarious📊

**🏗️ Architectur**e
Database Schema
Clinic
├── Doctors
│   └── Appointments
│       ├── Patients
│       └── AppointmentTreatments
│           └── Treatments



**Project Structure**
ClinicFlowApp/
├── Models/
│   ├── Clinic.cs
│   ├── Doctor.cs
│   ├── Patient.cs
│   ├── Appointment.cs
│   ├── Treatment.cs
│   └── AppointmentTreatment.cs
├── Services/
│   ├── UIHelper.cs              
│   ├── MenuService.cs
│   ├── PatientService.cs
│   ├── DoctorService.cs
│   ├── TreatmentService.cs
│   ├── AppointmentService.cs
│   └── ReportService.cs
├── ER Diagrams/
│   ├── clinicFlow.png      
│   └── clinicFlow(1).png
├── SQL Queries/
│   ├── sql_01_create_database            
│   ├── sql_02_create_tables.sql
│   ├── sql_03_seed_data.sql
│   ├── sql_04_crud_operations.sql
│   ├── sql_05_query_operations.sql
│   ├── sql_06_views.sql
│   └── sql_07_security.sql
│   └── sql_08_cleanup.sql
├── Data/
│   └── ClinicFlowDbContext.cs
└── Program.cs

**🚀 Getting Started**
Prerequisites

.NET 6.0 SDK or higher
SQL Server or SQL Server Express
Visual Studio 2022 or VS Code

**Qassem Abdulghani**

GitHub: https://github.com/Qassem-AGH
Email: qassam.abdulghani@hotmail.com
