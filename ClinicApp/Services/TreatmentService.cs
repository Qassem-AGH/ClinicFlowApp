using System;
using System.Linq;
using Clinic.Models;
using ClinicFlowApp.Models;
using Spectre.Console;

namespace Clinic.Services
{
    public class TreatmentService
    {
        public void ListAllTreatments()
        {
            UIHelper.ShowFigletHeader("💊 All Treatments", "List of all available treatments", Color.Magenta, "cyan", Color.DarkCyan);

            using var context = new ClinicFlowDbContext();
            var treatments = context.Treatments.OrderBy(t => t.TreatmentName).ToList();

            if (!treatments.Any())
            {
                UIHelper.ShowWarning("No treatments found.");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Magenta)
                .AddColumn(new TableColumn("[magenta]ID[/]").Centered())
                .AddColumn("[magenta]Treatment Name[/]")
                .AddColumn(new TableColumn("[magenta]Duration (min)[/]").Centered())
                .AddColumn(new TableColumn("[magenta]Price[/]") { Alignment = Justify.Right });

            foreach (var t in treatments)
                table.AddRow($"[white]{t.TreatmentId}[/]", $"[green]{t.TreatmentName}[/]",
                    $"[yellow]{t.DurationMinutes}[/]", $"[cyan]{t.Price:C}[/]");

            AnsiConsole.Write(table);
            UIHelper.ShowInfo($"Total: {treatments.Count} treatments");
        }
    }
}