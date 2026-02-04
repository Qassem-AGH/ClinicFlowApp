# 🏥 ClinicFlow App

A modern, colorful console application for managing clinic appointments, patients, doctors, and treatments built with C# .NET and Spectre.Console.

---

## ✨ Features

### 📋 Core Management
- 👥 **Patient Management** - Create and view patient records
- 👨‍⚕️ **Doctor Management** - Browse all doctors and their specializations
- 💊 **Treatment Management** - View available treatments with pricing
- 📅 **Appointment System** - Create, update, and delete appointments
- 🔗 **Treatment Linking** - Add multiple treatments to appointments

### 📊 Advanced Reports
- 🏆 **Top Patients** - Track most active patients by visit count
- 👨‍⚕️ **Doctor Workload** - Monitor appointment distribution across doctors
- ⚠️ **At-Risk Patients** - Identify patients with high no-show rates
- 📰 **Activity Feed** - View latest appointment activity
- ⭐ **Popular Treatments** - Analyze treatment bookings and revenue
- 🏥 **Clinic Summary** - Overview of all clinic locations

---

## 🎨 User Interface

Built with **Spectre.Console** for a beautiful terminal experience:

- 🌈 Color-coded tables with themed borders
- 😊 Emoji indicators for visual clarity
- 📊 Interactive selection menus (no more typing IDs!)
- 🎯 Status-based coloring (green=completed, red=no-show, etc.)
- 🥇 Medal rankings for top performers
- 📦 Information panels for important data
- ✅ Success/Error messages with icons

### Color Themes by Module

| Module       | Color   | Icon |
|--------------|---------|------|
| Patients     | Cyan    | 👥   |
| Doctors      | Blue    | 👨‍⚕️   |
| Treatments   | Magenta | 💊   |
| Appointments | Aqua    | 📅   |
| Reports      | Various | 📊   |

---

## 🏗️ Architecture

### Database Schema
```
Clinic
├── Doctors
│   └── Appointments
│       ├── Patients
│       └── AppointmentTreatments
│           └── Treatments
```

### Project Structure
```
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
│   ├── sql_01_create_database.sql
│   ├── sql_02_create_tables.sql
│   ├── sql_03_seed_data.sql
│   ├── sql_04_crud_operations.sql
│   ├── sql_05_query_operations.sql
│   ├── sql_06_views.sql
│   ├── sql_07_security.sql
│   └── sql_08_cleanup.sql
├── Data/
│   └── ClinicFlowDbContext.cs
└── Program.cs
```

---

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 SDK or higher
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Qassem-AGH/ClinicFlow.git
   cd ClinicFlow
   ```

2. **Create the database**
   
   Open **SQL Server Management Studio (SSMS)** and run scripts in order:
   ```sql
   -- Run these in order:
   SQL Queries/sql_01_create_database.sql
   SQL Queries/sql_02_create_tables.sql
   SQL Queries/sql_03_seed_data.sql
   SQL Queries/sql_06_views.sql
   SQL Queries/sql_07_security.sql
   ```

3. **Update connection string**
   
   Edit `Data/ClinicFlowDbContext.cs`:
   ```csharp
   optionsBuilder.UseSqlServer(
       "Server=localhost;Database=ClinicFlowDB;Integrated Security=true;TrustServerCertificate=true;"
   );
   ```

4. **Restore packages and run**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```


## 🎮 Usage

### Main Menu
```
"👥 List all patients",
"👨‍⚕️ List all doctors",
"📅 List all appointments",
"💊 List all treatments",
"➕ Create new appointment",
"🔗 Add treatment to appointment",
"✏️ Update appointment status",
"👤 Create new patient",
"🗑️ Delete appointment",
"📊 REPORTS MENU",
"❌ Exit"

```

### Example Workflows

**Book an Appointment:**
1. Select `➕ 5. Create new appointment`
2. Choose patient from interactive list (or create new with option 8)
3. Choose doctor from interactive list
4. Enter date and time
5. Select `🔗 6. Add treatment to appointment` to link treatments

**Create a New Patient and Book:**
1. Select `👤 8. Create new patient`
2. Enter patient details (name, email, phone)
3. Select `➕ 5. Create new appointment`
4. Choose the newly created patient
5. Select doctor and set appointment date

**Manage Appointments:**
1. Select `📅 3. List all appointments` to view current bookings
2. Select `✏️ 7. Update appointment status` to change status (Scheduled → Completed/NoShow/Cancelled)
3. Select `🗑️ 9. Delete appointment` to remove cancelled bookings

**View Reports:**
1. Select `📊 10. REPORTS MENU`
2. Choose from 6 different reports:
   - 🏆 Top Patients by visits
   - 👨‍⚕️ Doctor Workload analysis
   - ⚠️ At-Risk Patients (high no-show rates)
   - 📰 Latest Activity Feed
   - ⭐ Popular Treatments & Revenue
   - 🏥 Clinic Summary
3. View color-coded results with emoji indicators

**Browse Data:**
- `👥 1. List all patients` - View all registered patients
- `👨‍⚕️ 2. List all doctors` - See doctors and specializations
- `💊 4. List all treatments` - Check treatments and pricing

## 📁 SQL Scripts

| Script                        | Description                                                  |
|-------------------------------|--------------------------------------------------------------|
| `sql_01_create_database.sql`  | Creates ClinicFlowDB database                                |
| `sql_02_create_tables.sql`    | Creates all tables with constraints                          |
| `sql_03_seed_data.sql`        | Inserts test data (12 patients, 8 doctors, 30+ appointments) |
| `sql_04_crud_operations.sql`  | Example CRUD operations                                      |
| `sql_05_query_operations.sql` | Advanced queries with JOINs and aggregates                   |
| `sql_06_views.sql`            | Creates 6 views for reports                                  |
| `sql_07_security.sql`         | Sets up roles and users                                      |
| `sql_08_cleanup.sql`          | Cleans up database for fresh start                           |

---

## 🗂️ Database Tables

| Table                      | Description                       | Records |
|----------------------------|-----------------------------------|---------|
| **Clinic**                 | Clinic locations                  | 3       |
| **Doctor**                 | Doctors with specializations      | 8       |
| **Patients**               | Patient records                   | 12+     |
| **Treatments**             | Available treatments with pricin  | 8       |
| **Appointments**           | Scheduled appointments            | 30+     |
| **AppointmentTreatments**  | Many-to-many junction table       | 35+     |

### Relationships
- Clinic 1:N Doctor
- Doctor 1:N Appointments
- Patients 1:N Appointments
- Appointments N:N Treatments (via AppointmentTreatments)

---

## 🔒 Security Features

- Role-based access control (RBAC)
- `ClinicReadRole` - View-only access to reports
- `ClinicAppRole` - Full CRUD operations
- Views to hide sensitive patient data
- Input validation and SQL injection protection

---

## 📸 ER Diagrams

See full database design in:
- `ER Diagrams/clinicFlow.png`
- `ER Diagrams/clinicFlow(1).png`

---

## 🛠️ Technologies

- **Backend:** C# .NET 6.0+
- **Database:** SQL Server 2019+
- **ORM:** Entity Framework Core (Database First)
- **UI Framework:** Spectre.Console
- **Query Language:** LINQ

---

## 👨‍💻 Author

**Qassem Abdulghani**

- GitHub: [@Qassem-AGH](https://github.com/Qassem-AGH)
- Email: qassam.abdulghani@hotmail.com



