using Spectre.Console;
using System;

namespace Clinic.Services
{
    public class MenuService
    {
        private readonly PatientService _patientService = new();
        private readonly DoctorService _doctorService = new();
        private readonly AppointmentService _appointmentService = new();
        private readonly TreatmentService _treatmentService = new();
        private readonly ReportService _reportService = new();

        public void ShowMainMenu()
        {
            while (true)
            {
                UIHelper.ShowFigletHeader("Clinic Flow App", "all your clinic needs", Color.Green, "cyan", Color.DarkCyan);

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]📋 Select an option:[/]")
                        .PageSize(15)
                        .AddChoices(new[]
                        {
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
                        }));

                try
                {
                    switch (choice)
                    {
                        case "👥 List all patients": _patientService.ListAllPatients(); break;
                        case "👨‍⚕️ List all doctors": _doctorService.ListAllDoctors(); break;
                        case "📅 List all appointments": _appointmentService.ListAllAppointments(); break;
                        case "💊 List all treatments": _treatmentService.ListAllTreatments(); break;
                        case "➕ Create new appointment": _appointmentService.CreateAppointment(); break;
                        case "🔗 Add treatment to appointment": _appointmentService.AddTreatmentToAppointment(); break;
                        case "✏️ Update appointment status": _appointmentService.UpdateAppointmentStatus(); break;
                        case "👤 Create new patient": _patientService.CreatePatient(); break;
                        case "🗑️ Delete appointment": _appointmentService.DeleteAppointment(); break;
                        case "📊 REPORTS MENU": ShowReportsMenu(); break;
                        case "❌ Exit": return;
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowError(ex.Message);
                }

                if (choice != "📊 REPORTS MENU" && choice != "❌ Exit")
                    UIHelper.PressAnyKey();
            }
        }

        private void ShowReportsMenu()
        {
            while (true)
            {
                UIHelper.ShowFigletHeader("Reports", "Analytics & Insights", Color.Yellow, "cyan", Color.DarkCyan);

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select a report:[/]")
                        .AddChoices(new[]
                        {
                            "🏆 Top Patients by Visits",
                            "👨‍⚕️ Doctor Workload Report",
                            "⚠️ At Risk Patients (NoShow Analysis)",
                            "📰 Latest Activity Feed",
                            "⭐ Popular Treatments",
                            "🏥 Clinic Summary",
                            "⬅️ Back to Main Menu"
                        }));

                try
                {
                    switch (choice)
                    {
                        case "🏆 Top Patients by Visits": _reportService.ShowTopPatients(); break;
                        case "👨‍⚕️ Doctor Workload Report": _reportService.ShowDoctorWorkload(); break;
                        case "⚠️ At Risk Patients (NoShow Analysis)": _reportService.ShowAtRiskPatients(); break;
                        case "📰 Latest Activity Feed": _reportService.ShowLatestActivity(); break;
                        case "⭐ Popular Treatments": _reportService.ShowPopularTreatments(); break;
                        case "🏥 Clinic Summary": _reportService.ShowClinicSummary(); break;
                        case "⬅️ Back to Main Menu": return;
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowError(ex.Message);
                }

                if (choice != "⬅️ Back to Main Menu")
                    UIHelper.PressAnyKey();
            }
        }
    }
}