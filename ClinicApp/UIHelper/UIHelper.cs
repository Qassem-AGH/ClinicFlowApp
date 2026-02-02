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
    }
}