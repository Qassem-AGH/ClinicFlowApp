using System;
using System.Linq;
using Clinic.Models;
using ClinicFlowApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Clinic.Services
{
    public class DoctorService
    {
        public void ListAllDoctors()
        {
            UIHelper.ShowFigletHeader("👨‍⚕️ All Doctors", "List of all registered doctors", Color.Green, "cyan", Color.DarkCyan);

            using var context = new ClinicFlowDbContext();
            var doctors = context.Doctors
                .Include(d => d.Clinic)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToList();

            if (!doctors.Any())
            {
                UIHelper.ShowWarning("No doctors found.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Blue)
                .AddColumn(new TableColumn("[blue]ID[/]").Centered())
                .AddColumn("[blue]Name[/]")
                .AddColumn("[blue]Specialization[/]")
                .AddColumn("[blue]Clinic[/]")
                .AddColumn("[blue]City[/]");

            foreach (var d in doctors)
                table.AddRow($"[white]{d.DoctorId}[/]", $"[green]Dr. {d.FirstName} {d.LastName}[/]",
                    $"[yellow]{d.Specialization}[/]", $"[cyan]{d.Clinic?.ClinicName ?? "N/A"}[/]",
                    $"[grey]{d.Clinic?.City ?? "N/A"}[/]");

            AnsiConsole.Write(table);
            UIHelper.ShowInfo($"Total: {doctors.Count} doctors");
        }
    }
}