🏥 Clinic Flow App
A modern, colorful C# .NET console application for managing clinic operations — powered by Spectre.Console for a beautiful, interactive terminal experience.

✨ Features
📋 Core Management
- 👥 Patient Management – Create and view patient records
- 👨‍⚕️ Doctor Management – Browse doctors and their specializations
- 💊 Treatment Management – View treatments with pricing
- 📅 Appointment System – Create, update, and delete appointments
- 🔗 Treatment Linking – Attach multiple treatments to appointments

📊 Advanced Reports
- 🏆 Top Patients – Track most active patients by visit count
- 👨‍⚕️ Doctor Workload – Monitor appointment distribution
- ⚠️ At‑Risk Patients – Identify high no‑show rates
- 📰 Activity Feed – View recent appointment activity
- ⭐ Popular Treatments – Analyze treatment bookings & revenue
- 🏥 Clinic Summary – Overview of all clinic locations

🎨 User Interface (Spectre.Console)
Designed for a rich, engaging terminal experience:
- 🌈 Color‑coded tables with themed borders
- 😊 Emoji indicators for clarity
- 📊 Interactive selection menus (no manual ID typing!)
- 🎯 Status‑based coloring (green = completed, red = no‑show)
- 🥇 Medal rankings for top performers
- 📦 Information panels for key data
- ✅ Success & error messages with icons


🏗️ Architecture
📚 Database Schema
Clinic
├── Doctors
│   └── Appointments
├── Patients
│   └── AppointmentTreatments
└── Treatments


📁 Project Structure
ClinicFlowApp/
├── Models/
│   ├── Clinic.cs
│   ├── Doctor.cs
│   ├── Patient.cs
│   ├── Appointment.cs
│   ├── Treatment.cs
│   └── AppointmentTreatment.cs
│
├── Services/
│   ├── UIHelper.cs
│   ├── MenuService.cs
│   ├── PatientService.cs
│   ├── DoctorService.cs
│   ├── TreatmentService.cs
│   ├── AppointmentService.cs
│   └── ReportService.cs
│
├── ER Diagrams/
│   ├── clinicFlow.png
│   └── clinicFlow(1).png
│
├── SQL Queries/
│   ├── sql_01_create_database.sql
│   ├── sql_02_create_tables.sql
│   ├── sql_03_seed_data.sql
│   ├── sql_04_crud_operations.sql
│   ├── sql_05_query_operations.sql
│   ├── sql_06_views.sql
│   ├── sql_07_security.sql
│   └── sql_08_cleanup.sql
│
├── Data/
│   └── ClinicFlowDbContext.cs
│
└── Program.cs



🚀 Getting Started
🔧 Prerequisites
- .NET 6.0 SDK or higher
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

👤 Author
Qassem Abdulghani
📌 GitHub: https://github.com/Qassem-AGH
📧 Email: qassam.abdulghani@hotmail.com
