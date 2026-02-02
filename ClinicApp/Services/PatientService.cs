using System;
using System.Linq;
using Clinic.Models;
using ClinicFlowApp.Models;
using Spectre.Console;

namespace Clinic.Services
{
    public class PatientService
    {
        public void ListAllPatients()
        {
            UIHelper.ShowFigletHeader("👥 All Patients", "List of all registered patients", Color.Green, "cyan", Color.DarkCyan);

            using var context = new ClinicFlowDbContext();
            var patients = context.Patients.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();

            if (!patients.Any())
            {
                UIHelper.ShowWarning("No patients found.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Cyan)
                .AddColumn(new TableColumn("[cyan]ID[/]").Centered())
                .AddColumn("[cyan]First Name[/]")
                .AddColumn("[cyan]Last Name[/]")
                .AddColumn("[cyan]Email[/]")
                .AddColumn("[cyan]Phone[/]");

            foreach (var p in patients)
                table.AddRow($"[white]{p.PatientId}[/]", $"[green]{p.FirstName}[/]",
                    $"[green]{p.LastName}[/]", $"[yellow]{p.Email}[/]", $"[grey]{p.Phone ?? "N/A"}[/]");

            AnsiConsole.Write(table);
            UIHelper.ShowInfo($"Total: {patients.Count} patients");
        }

        public void CreatePatient()
        {
            UIHelper.ShowFigletHeader("➕ Create Patient", "Enter patient details", Color.Green, "cyan", Color.DarkCyan);

            try
            {
                var selectAdd = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Select an option:[/]")
                        .AddChoices(new[] { "Add New Patient", "Cancel" }));
                if (selectAdd == "Cancel")
                    return;

                var firstName = AnsiConsole.Ask<string>("[cyan]First Name:[/]").Trim();
                var lastName = AnsiConsole.Ask<string>("[cyan]Last Name:[/]").Trim();

                using var context = new ClinicFlowDbContext();
                string email;
                while (true)
                {
                    email = AnsiConsole.Ask<string>("[cyan]Email:[/]").Trim();

                    if (!email.Contains("@"))
                    {
                        UIHelper.ShowError("Invalid email format.");
                        continue;
                    }

                    if (context.Patients.Any(p => p.Email == email))
                    {
                        UIHelper.ShowError("Email already exists.");
                        continue;
                    }
                    break;
                }

                var phone = AnsiConsole.Prompt(new TextPrompt<string>("[cyan]Phone (optional):[/]").AllowEmpty()).Trim();

                var newPatient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                    CreatedAt = DateTime.Now
                };

                context.Patients.Add(newPatient);
                context.SaveChanges();

                UIHelper.ShowSuccess($"Patient created successfully! ID: {newPatient.PatientId}");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }
    }
}