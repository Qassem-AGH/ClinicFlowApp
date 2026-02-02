using System;
using System.Linq;
using Clinic.Models;
using ClinicFlowApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Clinic.Services
{
    public class AppointmentService
    {
        public void ListAllAppointments()
        {
            UIHelper.ShowFigletHeader("📅 Appointments", "List of all appointments", Color.Cyan, "grey", Color.CadetBlue_1);

            using var context = new ClinicFlowDbContext();
            var appointments = context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .Take(30)
                .ToList();

            if (!appointments.Any())
            {
                UIHelper.ShowWarning("No appointments found.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Aqua)
                .AddColumn(new TableColumn("[aqua]ID[/]").Centered())
                .AddColumn("[aqua]Date[/]")
                .AddColumn("[aqua]Patient[/]")
                .AddColumn("[aqua]Doctor[/]")
                .AddColumn(new TableColumn("[aqua]Status[/]").Centered());

            foreach (var a in appointments)
            {
                var statusColor = a.Status switch
                {
                    "Completed" => "green",
                    "Scheduled" => "yellow",
                    "Cancelled" => "grey",
                    "NoShow" => "red",
                    _ => "white"
                };

                table.AddRow(
                    $"[white]{a.AppointmentId}[/]",
                    $"[cyan]{a.AppointmentDate:yyyy-MM-dd HH:mm}[/]",
                    $"[green]{a.Patient?.FirstName} {a.Patient?.LastName}[/]",
                    $"[blue]Dr. {a.Doctor?.FirstName} {a.Doctor?.LastName}[/]",
                    $"[{statusColor}]{a.Status}[/]"
                );
            }

            AnsiConsole.Write(table);
            UIHelper.ShowInfo("Showing latest 30 appointments");
        }

        public void CreateAppointment()
        {
            UIHelper.ShowFigletHeader("➕ Create Appointment", "Schedule a new appointment", Color.Green, "grey", Color.Green);

            using var context = new ClinicFlowDbContext();

            try
            {
                var patients = context.Patients.OrderBy(p => p.LastName).Take(20).ToList();
                if (!patients.Any())
                {
                    UIHelper.ShowError("No patients found. Please create a patient first.");
                    return;
                }

                var selectPatientorback = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select Patient:[/]")
                        .AddChoices(patients.Select(p =>
                            $"{p.PatientId}. {p.FirstName} {p.LastName}").Concat(new[] { "Back To Main Menu" }).ToArray())
                );
                if (selectPatientorback == "Back To Main Menu")
                    return;
                var patientId = int.Parse(selectPatientorback.Split('.')[0]);

                var doctors = context.Doctors.OrderBy(d => d.LastName).ToList();
                var selectDoctororback = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select Doctor:[/]")
                        .AddChoices(doctors.Select(d =>
                            $"{d.DoctorId}. Dr. {d.FirstName} {d.LastName}").Concat(new[] { "Back To Main Menu" }).ToArray())
                );
                if (selectDoctororback == "Back To Main Menu")
                    return;
                var doctorId = int.Parse(selectDoctororback.Split('.')[0]);
                var appointmentDate = AnsiConsole.Ask<DateTime>("[cyan]Enter Appointment Date and Time (YYYY-MM-DD HH:MM):[/]");
                var newAppointment = new Appointment
                {
                    PatientId = patientId,
                    DoctorId = doctorId,
                    AppointmentDate = appointmentDate,
                    Status = "Scheduled"
                };
                context.Appointments.Add(newAppointment);
                context.SaveChanges();
                UIHelper.ShowSuccess("Appointment created successfully!");

            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }

        public void AddTreatmentToAppointment()
        {
            UIHelper.ShowFigletHeader("🔗 Add Treatment to Appointment", "Link a treatment to an existing appointment", Color.Yellow, "grey", Color.Aquamarine1);

            using var context = new ClinicFlowDbContext();

            try
            {
                var selectaddTreatment = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Would you like to add a treatment to an appointment?[/]")
                        .AddChoices(new[] { "Yes", "No" })
                );
                if (selectaddTreatment == "No")
                    return;

                var appointments = context.Appointments
                    .Include(a => a.Patient)
                    .Where(a => a.Status == "Scheduled")
                    .OrderByDescending(a => a.AppointmentDate)
                    .Take(15)
                    .ToList();

                if (!appointments.Any())
                {
                    UIHelper.ShowWarning("No scheduled appointments found.");
                    return;
                }

                var apptChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select Appointment:[/]")
                        .AddChoices(appointments.Select(a =>
                            $"{a.AppointmentId}. {a.AppointmentDate:yyyy-MM-dd HH:mm} - {a.Patient?.FirstName} {a.Patient?.LastName}").Concat(new[] { "Back To Main Menu" }).ToArray())
                );
                if (apptChoice == "Back To Main Menu")
                    return;
                var appointmentId = int.Parse(apptChoice.Split('.')[0]);

                var treatments = context.Treatments.OrderBy(t => t.TreatmentName).ToList();
                var treatmentChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select Treatment:[/]")
                        .AddChoices(treatments.Select(t =>
                            $"{t.TreatmentId}. {t.TreatmentName} - {t.Price:C} ({t.DurationMinutes} min)").ToArray())
                );
                var treatmentId = int.Parse(treatmentChoice.Split('.')[0]);

                if (context.AppointmentTreatments.Any(at => at.AppointmentId == appointmentId && at.TreatmentId == treatmentId))
                {
                    UIHelper.ShowWarning("This treatment is already added to the appointment.");
                    return;
                }

                context.AppointmentTreatments.Add(new AppointmentTreatment
                {
                    AppointmentId = appointmentId,
                    TreatmentId = treatmentId
                });
                context.SaveChanges();

                UIHelper.ShowSuccess("Treatment added to appointment successfully!");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }

        public void UpdateAppointmentStatus()
        {
            UIHelper.ShowFigletHeader("✏️  Update Appointment Status", "Change the status of an appointment", Color.Orange1, "grey", Color.DarkRed);

            using var context = new ClinicFlowDbContext();

            try
            {
                
                    // Show all appointments
                    var recentAppointments = context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .OrderByDescending(a => a.AppointmentDate)
                        .Take(20)
                        .ToList();
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .BorderColor(Color.Orange1)
                        .AddColumn(new TableColumn("[orange1]ID[/]").Centered())
                        .AddColumn("[orange1]Date[/]")
                        .AddColumn("[orange1]Patient[/]")
                        .AddColumn("[orange1]Doctor[/]")
                        .AddColumn(new TableColumn("[orange1]Status[/]").Centered());

                    foreach (var a in recentAppointments)
                    {
                        var statusColor = a.Status switch
                        {
                            "Completed" => "green",
                            "Scheduled" => "yellow",
                            "Cancelled" => "grey",
                            "NoShow" => "red",
                            _ => "white"
                        };
                        table.AddRow(
                            $"[white]{a.AppointmentId}[/]",
                            $"[cyan]{a.AppointmentDate:yyyy-MM-dd HH:mm}[/]",
                            $"[green]{a.Patient?.FirstName} {a.Patient?.LastName}[/]",
                            $"[blue]Dr. {a.Doctor?.FirstName} {a.Doctor?.LastName}[/]",
                            $"[{statusColor}]{a.Status}[/]"
                        );
                    }
                    AnsiConsole.Write(table);
                

                var appointmentId = AnsiConsole.Ask<int>("[cyan]Enter Appointment ID:[/]");

                var appointment = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefault(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    UIHelper.ShowError("Appointment not found.");
                    return;
                }

                var panel = new Panel($"""
                    [cyan]Patient:[/] {appointment.Patient?.FirstName} {appointment.Patient?.LastName}
                    [cyan]Doctor:[/] Dr. {appointment.Doctor?.FirstName} {appointment.Doctor?.LastName}
                    [cyan]Date:[/] {appointment.AppointmentDate:yyyy-MM-dd HH:mm}
                    [cyan]Current Status:[/] [yellow]{appointment.Status}[/]
                    """)
                    .Header("[yellow]Current Appointment[/]")
                    .BorderColor(Color.Yellow);

                AnsiConsole.Write(panel);
                Console.WriteLine();

                var newStatus = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select new status:[/]")
                        .AddChoices(new[] { "Scheduled", "Completed", "Cancelled", "NoShow", "Back to Main Menu" })
                );
                if (newStatus == "Back to Main Menu")
                {
                    UIHelper.ShowInfo("Operation cancelled.");
                    return;
                }

                appointment.Status = newStatus;
                context.SaveChanges();

                UIHelper.ShowSuccess($"Appointment status updated to: {newStatus}");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }

        public void DeleteAppointment()
        {
            UIHelper.ShowFigletHeader("🗑️  Delete Appointment", "Remove an appointment from the system", Color.Red, "grey", Color.DarkRed);

            using var context = new ClinicFlowDbContext();

            try
            {
                // Show all appointments
                var recentAppointments = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDate)
                    .Take(20)
                    .ToList();
                var selectToDeleteTable = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select an appointment to delete:[/]")
                        .AddChoices(recentAppointments.Select(a =>
                            $"{a.AppointmentId}. {a.AppointmentDate:yyyy-MM-dd HH:mm} - {a.Patient?.FirstName} {a.Patient?.LastName}").Concat(new[] { "Back To Main Menu" }).ToArray())
                );
                if (selectToDeleteTable == "Back To Main Menu")
                    return;
                var appointmentId = int.Parse(selectToDeleteTable.Split('.')[0]);
                var appointment = context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                {
                    UIHelper.ShowError("Appointment not found.");
                    return;
                }
                var confirmDelete = AnsiConsole.Confirm($"[red]Are you sure you want to delete appointment ID {appointment.AppointmentId} scheduled on {appointment.AppointmentDate:yyyy-MM-dd HH:mm}?[/]");
                if (!confirmDelete)
                {
                    UIHelper.ShowInfo("Deletion cancelled.");
                    return;
                }
                context.Appointments.Remove(appointment);
                context.SaveChanges();
                UIHelper.ShowSuccess("Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }
    }
}