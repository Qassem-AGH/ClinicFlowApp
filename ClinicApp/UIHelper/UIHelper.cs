using Clinic.Models;
using Spectre.Console;
using System;

namespace Clinic.Services
{
    public static class UIHelper
    {
        public static void ShowFigletHeader(string title, string subtitle, Color titleColor, string subtitleColor ,Color subcolor)
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText(title).Centered().Color(titleColor));
            AnsiConsole.Write(new Rule($"[{subtitleColor}]{subtitle}[/]").RuleStyle(subcolor).Centered());
            Console.WriteLine();
        }

        public static void ShowSuccess(string message) =>
            AnsiConsole.MarkupLine($"[green]✅ {message}[/]");

        public static void ShowError(string message) =>
            AnsiConsole.MarkupLine($"[red]❌ {message}[/]");

        public static void ShowWarning(string message) =>
            AnsiConsole.MarkupLine($"[yellow]⚠️  {message}[/]");

        public static void ShowInfo(string message) =>
            AnsiConsole.MarkupLine($"[grey]📊 {message}[/]");

        public static void PressAnyKey()
        {
            Console.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey();
        }

        public static void TestDatabaseConnection()
        {
              AnsiConsole.Status()
                .StartAsync("Testing database connection...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    try
                    {
                        using var context = new ClinicFlowDbContext();
                        context.Database.CanConnect();

                        AnsiConsole.MarkupLine("[green]✅ Database connection successful![/]");
                        Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]❌ Connection failed: {ex.Message}[/]");
                    }
                });
            PressAnyKey();
        }
    }
}