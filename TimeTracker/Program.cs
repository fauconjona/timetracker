using JiraTracker.Interaces;
using JiraTracker.Services;
using KeyLogger.Interfaces;
using KeyLogger.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using time.layer.objet.Interfaces;
using time.layer.objet.Services;
using TimeTracker.Forms;
using TimeTracker.Interfaces;
using TimeTracker.Services;

namespace TimeTracker
{
    internal static class Program
    {
#if DEBUG
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
#endif

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            AllocConsole();
#endif
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            IEventService eventService = ServiceProvider.GetRequiredService<IEventService>();
            IJiraService jiraService = ServiceProvider.GetRequiredService<IJiraService>();

            try
            {
                var config = eventService.GetConfig();
                jiraService.Initialize(config.url, config.login, config.token, config.project);
            }
            catch (Exception e)
            {
                Console.WriteLine("JiraService.Initialize Error");
                Console.WriteLine(e.Message);
            }

            TimeTracker form = ServiceProvider.GetRequiredService<TimeTracker>();

            ITrackerManager trackerManager = ServiceProvider.GetRequiredService<ITrackerManager>();
            trackerManager.Start(form);
            
            Application.Run(form);
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IEventService, EventService>();
                    services.AddSingleton<IKeyLoggerService, KeyLoggerService>();
                    services.AddSingleton<ITrackerManager, TrackerManager>();
                    services.AddSingleton<IJiraService, JiraService>();
                    services.AddTransient<TimeTracker>();
                    services.AddTransient<ConfigForm>();
                });
        }

        public static void OpenConfig()
        {
            ConfigForm config = ServiceProvider.GetRequiredService<ConfigForm>();
            config.ShowDialog();
        }
    }
}