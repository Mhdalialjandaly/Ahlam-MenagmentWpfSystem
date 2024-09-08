using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Common;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model.Custom;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Management;
using EyeClinic.WPF.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Unity;
using WPFLocalizeExtension.Engine;

namespace EyeClinic.WPF.Components.Home.Setting.AppSetting
{
    public partial class AppSettingViewModel
    {
        public AppSettingViewModel() { }
    }
    public partial class AppSettingViewModel : BaseViewModel<AppSettingView>
    {
        private readonly IAppLanguageRepository _appLanguageRepository;
        private readonly EyeClinicContext _context;
        private readonly INotificationService _notificationService;
        private readonly IPrintService _printService;
        private readonly IUnityContainer _container;

        public AppSettingViewModel(IAppLanguageRepository appLanguageRepository,
            EyeClinicContext context,IUnityContainer unityContainer,
                INotificationService notificationService, IPrintService printService)
        {
            _appLanguageRepository = appLanguageRepository;
            _context = context;
            _container = unityContainer;
            _notificationService = notificationService;
            _printService = printService;

            SaveCommand = new RelayCommand(Save);
            SavePrinterCommand = new RelayCommand(SavePrinter);

            BackupDatabase = new RelayCommand(BackupDatabaseCommand);
            ResoreDataBase = new RelayCommand(RestoreDatabaseCommand);

        }

        private void RestoreDatabaseCommand()
        {
            try
            {
                var openFileDialog = new OpenFileDialog()
                {
                    Filter = "Backup Files (*.bak)|*.bak"
                };


                if (openFileDialog.ShowDialog() == true)
                {
                    var newFileName = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss")
                                      + Path.GetExtension(openFileDialog.SafeFileName);
                    SqlConnection connection = new SqlConnection(Global.GetValue(ConnectionHandler.ConnectionString).ToString());

                    string BackUpCommand = "USE EyeClinic;";
                    string BackUpCommand2 = "BACKUP DATABASE EyeClinic TO DISK ='D:\\EyeClinic-" + DateTime.Today.ToString() + "'";

                    SqlCommand sqlCommand = new SqlCommand(BackUpCommand, connection);
                    SqlCommand sqlCommand2 = new SqlCommand(BackUpCommand2, connection);

                    sqlCommand.ExecuteNonQuery();
                    sqlCommand2.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            }

        }


        private void BackupDatabaseCommand()
        {
            var connectionString = _context.Database.GetConnectionString();
            var connection = new SqlConnection(connectionString);

            // Choose database backup folder
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                BusyExecute(async () =>
                {
                    await BackupDatabaseToDrive(dialog.SelectedPath, connection.Database);
                    
                });
            }

            // Create the backup in the temp directory (the server should have access there)
            //using (var conn = new SqlConnection(connectionString))
            //using (var cmd = new SqlCommand(backupCommand, conn))
            //{
            //    conn.Open();
            //    cmd.Parameters.AddWithValue("@databaseName", databaseName);
            //    cmd.Parameters.AddWithValue("@backup", backup);
            //    cmd.ExecuteNonQuery();
            //}

            // File.Copy(backup, backupFilePath); // Copy file to final location
        }

        private async Task BackupDatabaseToDrive(string backupPath, string databaseName) 
        {
            var path = Path.Combine(backupPath, $"{databaseName}{DateTime.Now:dd-MM-yyyy}.bak");
            var backupCommand = $"USE [master] BACKUP DATABASE {databaseName} TO DISK = N'{path}' WITH NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10";
            try
            {
                await _context.Database.ExecuteSqlRawAsync(backupCommand);
                _notificationService.Success("The Data Has Saved");
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>().Error(ex.GetBaseException().Message);
            }
        }

        public override Task Initialize()
        {
            AvailableLanguages = new Language().GetLanguages();
            Printers = _printService.GetAllPrinters();
            return Task.CompletedTask;
        }

        public List<Language> AvailableLanguages { get; set; }
        public Language SelectedLanguage { get; set; }
        public List<Printer> Printers { get; set; }
        public Printer SelectedPrinter { get; set; }
        public string PathFile { get; set; }


        public ICommand SavePrinterCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand BackupDatabase { get; set; }
        public ICommand ResoreDataBase { get; set; }


        private void Save()
        {
            BusyExecute(async () =>
            {
                var deviceName = await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "//device_name.txt");
                await _appLanguageRepository.SaveCulture(SelectedLanguage.Code, deviceName);

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = new CultureInfo(SelectedLanguage.Code);
                CurrentLanguageCodeHandler.CurrentLanguageCode = SelectedLanguage.Code;


                _notificationService.Success("App settings updated");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EyeClinic.WPF.exe"))
                    {
                        UseShellExecute = true
                    }
                };
                process.Start();
                Application.Current.Shutdown();
            });
        }

        private void SavePrinter()
        {
            BusyExecute(async () =>
            {
                var deviceName = await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "//device_name.txt");
                await _appLanguageRepository.SetDevicePrinter(SelectedPrinter.Name, deviceName);
                Global.AddValue(GlobalKeys.SelectedPrinter, SelectedPrinter.Name);
            });
        }
    }
}
