using System;
using System.Linq;
using Clinic.Models;
using ClinicFlowApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Clinic.Services
{
    public class ReportService
    {
        public void ShowTopPatients()
        {
            UIHelper.ShowFigletHeader("🏆 Top Patients Report", "Most frequent patients", Color.Gold1, "yellow", Color.DarkGoldenrod);

            using var context = new ClinicFlowDbContext();
            var topPatients = context.Patients
                .Select(p => new
                {
                    p.PatientId,
                    PatientName = p.FirstName + " " + p.LastName,
                    TotalVisits = p.Appointments.Count(),
                    CompletedVisits = p.Appointments.Count(a => a.Status == "Completed"),
                    NoShows = p.Appointments.Count(a => a.Status == "NoShow"),
                    Cancelled = p.Appointments.Count(a => a.Status == "Cancelled")
                })
                .Where(p => p.TotalVisits > 0)
                .OrderByDescending(p => p.TotalVisits)
                .Take(10)
                .ToList();

            if (!topPatients.Any())
            {
                UIHelper.ShowWarning("No patient data available.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Gold1)
                .AddColumn(new TableColumn("[gold1]Rank[/]").Centered())
                .AddColumn(new TableColumn("[gold1]ID[/]").Centered())
                .AddColumn("[gold1]Patient Name[/]")
                .AddColumn(new TableColumn("[gold1]Total[/]").Centered())
                .AddColumn(new TableColumn("[gold1]Completed[/]").Centered())
                .AddColumn(new TableColumn("[gold1]NoShow[/]").Centered())
                .AddColumn(new TableColumn("[gold1]Cancelled[/]").Centered());

            var rank = 1;
            foreach (var p in topPatients)
            {
                var medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"[grey]{rank}[/]" };
                table.AddRow(medal, $"[white]{p.PatientId}[/]", $"[green]{p.PatientName}[/]",
                    $"[cyan]{p.TotalVisits}[/]", $"[green]{p.CompletedVisits}[/]",
                    $"[red]{p.NoShows}[/]", $"[grey]{p.Cancelled}[/]");
                rank++;
            }

            AnsiConsole.Write(table);
            UIHelper.ShowInfo($"Showing top {topPatients.Count} patients");
        }

        public void ShowDoctorWorkload()
        {
            UIHelper.ShowFigletHeader("👨‍⚕️ Doctor Workload Report", "Appointments by doctor", Color.Blue, "grey", Color.DarkBlue);

            using var context = new ClinicFlowDbContext();
            var doctorWorkload = context.Doctors
                .Include(d => d.Clinic)
                .Select(d => new
                {
                    d.DoctorId,
                    DoctorName = d.FirstName + " " + d.LastName,
                    d.Specialization,
                    TotalAppointments = d.Appointments.Count(),
                    Completed = d.Appointments.Count(a => a.Status == "Completed"),
                    NoShows = d.Appointments.Count(a => a.Status == "NoShow"),
                    Upcoming = d.Appointments.Count(a => a.Status == "Scheduled" && a.AppointmentDate > DateTime.Now)
                })
                .OrderByDescending(d => d.TotalAppointments)
                .ToList();

            if (!doctorWorkload.Any())
            {
                UIHelper.ShowWarning("No doctor data available.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Blue)
                .AddColumn(new TableColumn("[blue]ID[/]").Centered())
                .AddColumn("[blue]Doctor Name[/]")
                .AddColumn("[blue]Specialization[/]")
                .AddColumn(new TableColumn("[blue]Total[/]").Centered())
                .AddColumn(new TableColumn("[blue]✅ Done[/]").Centered())
                .AddColumn(new TableColumn("[blue]❌ NoShow[/]").Centered())
                .AddColumn(new TableColumn("[blue]📅 Upcoming[/]").Centered());

            foreach (var d in doctorWorkload)
            {
                table.AddRow($"[white]{d.DoctorId}[/]", $"[green]Dr. {d.DoctorName}[/]",
                    $"[yellow]{d.Specialization}[/]", $"[cyan]{d.TotalAppointments}[/]",
                    $"[green]{d.Completed}[/]", $"[red]{d.NoShows}[/]", $"[blue]{d.Upcoming}[/]");
            }

            AnsiConsole.Write(table);
            UIHelper.ShowInfo($"Total: {doctorWorkload.Count} doctors");
        }

        public void ShowAtRiskPatients()
        {
            UIHelper.ShowFigletHeader("⚠️  At Risk Patients", "Patients with multiple no-shows", Color.Red, "grey", Color.DarkRed);

            using var context = new ClinicFlowDbContext();
            var atRiskPatients = context.Patients
                .Select(p => new
                {
                    p.PatientId,
                    PatientName = p.FirstName + " " + p.LastName,
                    p.Email,
                    TotalVisits = p.Appointments.Count(),
                    NoShows = p.Appointments.Count(a => a.Status == "NoShow"),
                    NoShowPercentage = p.Appointments.Count() > 0
                        ? (p.Appointments.Count(a => a.Status == "NoShow") * 100.0 / p.Appointments.Count())
                        : 0
                })
                .Where(p => p.NoShows >= 2)
                .OrderByDescending(p => p.NoShows)
                .ThenByDescending(p => p.NoShowPercentage)
                .ToList();

            if (!atRiskPatients.Any())
            {
                UIHelper.ShowSuccess("No at-risk patients found. Great!");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Red)
                .AddColumn(new TableColumn("[red]Risk[/]").Centered())
                .AddColumn(new TableColumn("[red]ID[/]").Centered())
                .AddColumn("[red]Patient Name[/]")
                .AddColumn("[red]Email[/]")
                .AddColumn(new TableColumn("[red]Total[/]").Centered())
                .AddColumn(new TableColumn("[red]NoShows[/]").Centered())
                .AddColumn(new TableColumn("[red]Rate %[/]").Centered());

            foreach (var p in atRiskPatients)
            {
                var risk = p.NoShows >= 3 ? "🔴" : "🟡";
                table.AddRow(risk, $"[white]{p.PatientId}[/]", $"[yellow]{p.PatientName}[/]",
                    $"[grey]{p.Email}[/]", $"[cyan]{p.TotalVisits}[/]",
                    $"[red]{p.NoShows}[/]", $"[red]{p.NoShowPercentage:F1}%[/]");
            }

            AnsiConsole.Write(table);
            Console.WriteLine();
            AnsiConsole.MarkupLine($"[red]⚠️  {atRiskPatients.Count} at-risk patients identified[/]");
            AnsiConsole.MarkupLine("[grey]🔴 High Risk (3+ NoShows)  🟡 Medium Risk (2 NoShows)[/]");
        }

        public void ShowLatestActivity()
        {
            UIHelper.ShowFigletHeader("🕒 Latest Activity", "Most recent appointments", Color.Cyan1, "grey", Color.DarkCyan);

            using var context = new ClinicFlowDbContext();
            var latestActivity = context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Clinic)
                .OrderByDescending(a => a.CreatedAt)
                .Take(20)
                .ToList();

            if (!latestActivity.Any())
            {
                UIHelper.ShowWarning("No activity found.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Cyan1)
                .AddColumn("[cyan1]Date[/]")
                .AddColumn("[cyan1]Patient[/]")
                .AddColumn("[cyan1]Doctor[/]")
                .AddColumn("[cyan1]Clinic[/]")
                .AddColumn(new TableColumn("[cyan1]Status[/]").Centered());

            foreach (var a in latestActivity)
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
                    $"[cyan]{a.AppointmentDate:yyyy-MM-dd HH:mm}[/]",
                    $"[green]{a.Patient?.FirstName} {a.Patient?.LastName}[/]",
                    $"[blue]Dr. {a.Doctor?.FirstName} {a.Doctor?.LastName}[/]",
                    $"[grey]{a.Doctor?.Clinic?.ClinicName ?? "N/A"}[/]",
                    $"[{statusColor}]{a.Status}[/]"
                );
            }

            AnsiConsole.Write(table);
            UIHelper.ShowInfo("Showing latest 20 appointments");
        }

        public void ShowPopularTreatments()
        {
            UIHelper.ShowFigletHeader("💊 Popular Treatments", "Most booked treatments", Color.Magenta1, "grey", Color.DarkMagenta);

            using var context = new ClinicFlowDbContext();
            var popularTreatments = context.Treatments
                .Select(t => new
                {
                    t.TreatmentId,
                    t.TreatmentName,
                    t.DurationMinutes,
                    t.Price,
                    TimesBooked = t.AppointmentTreatments.Count(),
                    TotalRevenue = t.AppointmentTreatments.Count() * t.Price
                })
                .OrderByDescending(t => t.TimesBooked)
                .ToList();

            if (!popularTreatments.Any())
            {
                UIHelper.ShowWarning("No treatment data available.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Magenta1)
                .AddColumn(new TableColumn("[magenta1]ID[/]").Centered())
                .AddColumn("[magenta1]Treatment Name[/]")
                .AddColumn(new TableColumn("[magenta1]Duration[/]").Centered())
                .AddColumn(new TableColumn("[magenta1]Price[/]") { Alignment = Justify.Right })
                .AddColumn(new TableColumn("[magenta1]📊 Booked[/]").Centered())
                .AddColumn(new TableColumn("[magenta1]💰 Revenue[/]") { Alignment = Justify.Right });

            foreach (var t in popularTreatments)
            {
                table.AddRow($"[white]{t.TreatmentId}[/]", $"[green]{t.TreatmentName}[/]",
                    $"[yellow]{t.DurationMinutes} min[/]", $"[cyan]{t.Price:C}[/]",
                    $"[blue]{t.TimesBooked}[/]", $"[green]{t.TotalRevenue:C}[/]");
            }

            AnsiConsole.Write(table);
            Console.WriteLine();
            var totalRevenue = popularTreatments.Sum(t => t.TotalRevenue);
            AnsiConsole.MarkupLine($"[grey]📊 Total treatments: {popularTreatments.Count}[/]");
            AnsiConsole.MarkupLine($"[green]💰 Total revenue: {totalRevenue:C}[/]");
        }

        public void ShowClinicSummary()
        {
            UIHelper.ShowFigletHeader("🏥 Clinic Summary", "Overview of clinics", Color.Green1, "grey", Color.DarkGreen);

            using var context = new ClinicFlowDbContext();
            var clinicSummary = context.Clinics
                .Select(c => new
                {
                    c.ClinicId,
                    c.ClinicName,
                    c.City,
                    NumberOfDoctors = c.Doctors.Count(),
                    TotalAppointments = c.Doctors.SelectMany(d => d.Appointments).Count(),
                    CompletedAppointments = c.Doctors.SelectMany(d => d.Appointments).Count(a => a.Status == "Completed")
                })
                .OrderByDescending(c => c.TotalAppointments)
                .ToList();

            if (!clinicSummary.Any())
            {
                UIHelper.ShowWarning("No clinic data available.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .AddColumn(new TableColumn("[green]ID[/]").Centered())
                .AddColumn("[green]Clinic Name[/]")
                .AddColumn("[green]City[/]")
                .AddColumn(new TableColumn("[green]👨‍⚕️ Doctors[/]").Centered())
                .AddColumn(new TableColumn("[green]📅 Total[/]").Centered())
                .AddColumn(new TableColumn("[green]✅ Completed[/]").Centered());

            foreach (var c in clinicSummary)
            {
                table.AddRow($"[white]{c.ClinicId}[/]", $"[cyan]{c.ClinicName}[/]",
                    $"[yellow]{c.City}[/]", $"[blue]{c.NumberOfDoctors}[/]",
                    $"[magenta]{c.TotalAppointments}[/]", $"[green]{c.CompletedAppointments}[/]");
            }

            AnsiConsole.Write(table);
            Console.WriteLine();
            AnsiConsole.MarkupLine($"[grey]🏥 Total clinics: {clinicSummary.Count}[/]");
            AnsiConsole.MarkupLine($"[grey]👨‍⚕️ Total doctors: {clinicSummary.Sum(c => c.NumberOfDoctors)}[/]");
            AnsiConsole.MarkupLine($"[grey]📅 Total appointments: {clinicSummary.Sum(c => c.TotalAppointments)}[/]");
        }
    }
}