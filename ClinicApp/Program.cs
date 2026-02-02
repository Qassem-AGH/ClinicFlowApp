using Clinic.Services;
using Spectre.Console;

namespace ClinicFlowApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Initialize services
            var menuService = new MenuService();

            // Run main menu
            menuService.ShowMainMenu();
        }
    }
}