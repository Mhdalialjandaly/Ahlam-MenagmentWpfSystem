using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model.Custom;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Shell;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using EyeClinic.WPF.Setup;
using Microsoft.EntityFrameworkCore;
using Unity;
using WPFLocalizeExtension.Engine;

namespace EyeClinic.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {       
        protected override async void OnStartup(StartupEventArgs e) {
           
            if (ApplicationAlreadyRunning)
            {              
                System.Diagnostics.Process.GetCurrentProcess().Kill();                
            }
            //Activation
            if (DateTime.Now.Day >= 1 && DateTime.Now.Month >= 1 && DateTime.Now.Year > 2025)
            {               
                this.Shutdown();
            }
            base.OnStartup(e);
            var container = new UnityContainer();

            await using (var context = new EyeClinicContext()) {
                await context.Database.MigrateAsync();

                // To increase patient age one year un comment code
                //var patients = await context.Patients.ToListAsync();
                //foreach (var patient in patients)
                //{
                //    patient.Age += 1;
                //}

                //await context.SaveChangesAsync();
            }

            AppDomain.CurrentDomain.UnhandledException += (_, x) =>
                LogUnhandledException((Exception)x.ExceptionObject);

            DispatcherUnhandledException += (_, x) => {
                LogUnhandledException(x.Exception);
                x.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (_, x) => {
                LogUnhandledException(x.Exception);
                x.SetObserved();
            };

            container.RegisterServices();

            var deviceName = await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "//device_name.txt");
            Global.DeviceName = deviceName;

            var languageCode = await container.Resolve<IAppLanguageRepository>()
                .GetCurrentCulture(deviceName);
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(languageCode);
            CurrentLanguageCodeHandler.CurrentLanguageCode = languageCode;

            Global.AddValue(GlobalKeys.SelectedPrinter,
                await container.Resolve<IAppLanguageRepository>().GetDevicePrinter(deviceName));

            ContainerHandler.Container = container;

            //var queueWindow = container.Resolve<QueueWindowViewModel>();
            //await queueWindow.Initialize();
            //container.RegisterInstance(queueWindow);

            //(queueWindow.GetView() as QueueWindowView)?.Show();

            var shell = container.Resolve<ShellViewModel>();
            await shell.Initialize();
            container.RegisterInstance(shell);
            (shell.GetView() as Window)?.Show();

            var queueWindow = container.Resolve<QueueWindowViewModel>();
            container.RegisterInstance(queueWindow);
        }

        private void LogUnhandledException(Exception exception) {
            try
            {
                    File.WriteAllText("C:\\Data.txt",
                         exception.GetBaseException().Message);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }          
        }

        public bool ApplicationAlreadyRunning =>
            System.Diagnostics.Process.GetProcessesByName(
                Path.GetFileNameWithoutExtension(
                    System.Reflection.Assembly.GetEntryAssembly()?.Location)).Length > 1;
    }
}
